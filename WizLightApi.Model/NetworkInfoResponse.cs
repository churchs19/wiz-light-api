namespace WizLightApi.Model;

public class NetworkInfoResponse
{
    public NetworkInfoResponse()
    {
        IpAddress = "";
        MacAddress = "";
    }

    public NetworkInfoResponse(string ipAddress, string macAddress)
    {
        IpAddress = ipAddress;
        MacAddress = macAddress;
    }

    public string IpAddress { get; }
    public string MacAddress { get; }
}