using System.Xml.Serialization;

[XmlRoot("ValCurs")]
public class ValCurs
{
    [XmlAttribute("Date")]
    public required string Date { get; set; }

    [XmlAttribute("name")]
    public required string Name { get; set; }

    [XmlElement("Valute")]
    public List<Valute> Valutes { get; set; } = new List<Valute>();
}