namespace WizLightApi.Model;

public class LightRequest
{
    public bool On { get; set; } = true;
    public int? Red { get; set; }
    public int? Green { get; set; }
    public int? Blue { get; set; }
    public int? Temperature { get; set; }
    public int Dimming { get; set; } = 100;

    public bool IsValid()
    {
        if (Red.HasValue && (Red.Value < 0 || Red.Value > 255)) { return false; }
        if (Green.HasValue && (Green.Value < 0 || Green.Value > 255)) { return false; }
        if (Blue.HasValue && (Blue.Value < 0 || Blue.Value > 255)) { return false; }
        if (Dimming < 0 || Dimming > 100) { return false; }
        if (Temperature.HasValue && (Temperature.Value < 2700 || Temperature.Value > 6500)) { return false; }
        return true;
    }
}