namespace WizLightApi.Model;

public class WizConfiguration
{
    public WizConfiguration()
    {
        WizHome = int.MinValue;
        Lights = new WizLight[0];
    }

    public int WizHome { get; set; }
    public WizLight[] Lights { get; set; }
}