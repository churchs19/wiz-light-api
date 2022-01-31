using System.Net;
using System.Net.NetworkInformation;

namespace WizLightApi.Model;

public class NetworkInfo
{
    public NetworkInfo()
    {
        HostIP = null;
        HostNIC = null;
    }

    public NetworkInfo(IPAddress? hostIP, NetworkInterface? hostNIC)
    {
        HostIP = hostIP;
        HostNIC = hostNIC;
    }

    public IPAddress? HostIP { get; }
    public NetworkInterface? HostNIC { get; }

    public byte[] MacAddress => HostNIC?.GetPhysicalAddress().GetAddressBytes() ?? new byte[0];

    public NetworkInfoResponse ToResponse()
    {
        return new NetworkInfoResponse(HostIP?.ToString() ?? "", String.Join(' ', MacAddress.Select(b => b.ToString("X2"))));
    }
}
