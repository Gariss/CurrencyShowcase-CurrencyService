using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

public class Valute
{
    [XmlAttribute("ID")]
    public required string Id { get; set; }

    [XmlElement("NumCode")]
    public required string NumCode { get; set; }

    [XmlElement("CharCode")]
    public required string CharCode { get; set; }

    [XmlElement("Nominal")]
    public int Nominal { get; set; }

    [XmlElement("Name")]
    public required string Name { get; set; }

    [XmlIgnore]
    public decimal ValueDecimal { get; set; }

    [XmlIgnore]
    public decimal VunitRateDecimal { get; set; }

    [XmlElement("Value")]
    public string Value
    {
        get
        {
            return ValueDecimal.ToString(CultureInfo.InvariantCulture);
        }
        set
        {
            if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("ru-RU"), out decimal parsedValue))
            {
                ValueDecimal = parsedValue;
            }
        }
    }

    [XmlElement("VunitRate")]
    public string VunitRate
    {
        get
        {
            return VunitRateDecimal.ToString(CultureInfo.InvariantCulture); ;
        }
        set
        {
            if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("ru-RU"), out decimal parsedVunitRate))
            {
                VunitRateDecimal = parsedVunitRate;
            }
        }
    }
}