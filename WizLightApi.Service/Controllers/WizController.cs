using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using OpenWiz;

using WizLightApi.Model;
using WizLightApi.Utility;

namespace WizLightApi.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class WizController : ControllerBase
{
    private IConfiguration _config;
    private readonly ILogger<WizController> _logger;
    private readonly WizConfiguration _wizConfig;

    public WizController(IConfiguration config, ILogger<WizController> logger)
    {
        _config = config;
        _logger = logger;
        _wizConfig = _config.GetSection("WizConfiguration").Get<WizConfiguration>();
    }

    [HttpPost(Name = "SetLights")]
    public void SetLights([FromQuery] LightRequest request)
    {
        if (!request.IsValid())
        {
            throw new BadHttpRequestException("Invalid request");
        }

        using (WizSocket wizSocket = new WizSocket())
        {
            wizSocket.GetSocket().SendTimeout = 1000;
            wizSocket.GetSocket().ReceiveTimeout = 1000;

            foreach (var light in _wizConfig.Lights)
            {
                IPAddress? lightIP;
                if (!string.IsNullOrWhiteSpace(light.MacAddress)
                    && !string.IsNullOrWhiteSpace(light.IpAddress)
                    && IPAddress.TryParse(light.IpAddress, out lightIP))
                {
                    var wizHandle = new WizHandle(light.MacAddress, lightIP);
                    var state = new WizState();
                    state.Method = WizMethod.setPilot;
                    state.Params = new WizParams
                    {
                        State = request.On,
                        Dimming = request.Dimming
                    };
                    if (request.Red.HasValue) { state.Params.R = request.Red.Value; }
                    if (request.Green.HasValue) { state.Params.G = request.Green.Value; }
                    if (request.Blue.HasValue) { state.Params.B = request.Blue.Value; }
                    if (request.Temperature.HasValue) { state.Params.Temp = request.Temperature.Value; }

                    wizSocket.SendTo(state, wizHandle);

                    WizResult pilot;
                    while (true)
                    {
                        state = wizSocket.ReceiveFrom(wizHandle);
                        pilot = state.Result;
                        break;
                    }
                }
            }
        }
    }

    [HttpGet("network-info", Name = "GetNetworkInfo")]
    public NetworkInfoResponse GetNetworkInfo()
    {
        return NetworkInterfaceDiscovery.GetNetworkInfo().ToResponse();
    }

    [HttpGet("light-status", Name = "GetLightStatus")]
    public IEnumerable<WizResult> GetLightStatus()
    {
        List<WizResult> results = new List<WizResult>();
        using (WizSocket wizSocket = new WizSocket())
        {
            wizSocket.GetSocket().SendTimeout = 1000;
            wizSocket.GetSocket().ReceiveTimeout = 1000;

            foreach (var light in _wizConfig.Lights)
            {
                IPAddress? lightIP;
                if (!string.IsNullOrWhiteSpace(light.MacAddress)
                    && !string.IsNullOrWhiteSpace(light.IpAddress)
                    && IPAddress.TryParse(light.IpAddress, out lightIP))
                {
                    var wizHandle = new WizHandle(light.MacAddress, lightIP);
                    var state = WizState.MakeGetPilot();

                    wizSocket.SendTo(state, wizHandle);

                    WizResult pilot;
                    while (true)
                    {
                        state = wizSocket.ReceiveFrom(wizHandle);
                        pilot = state.Result;
                        break;
                    }

                    results.Add(pilot);
                }
            }

            return results.AsEnumerable();
        }
    }
}
