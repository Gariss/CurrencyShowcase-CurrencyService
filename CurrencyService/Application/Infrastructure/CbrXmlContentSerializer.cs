using Refit;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

/// <summary>
/// Read-only Serializer for CBRF
/// </summary>
public class CbrXmlContentSerializer : IHttpContentSerializer
{
    public async Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
    {
        var bytes = await content.ReadAsByteArrayAsync(cancellationToken);
        var encoding = Encoding.GetEncoding("windows-1251");
        var xmlContent = encoding.GetString(bytes);
        var originalCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(xmlContent);

        try
        {
            return (T?)serializer.Deserialize(reader);
        }
        finally
        {
            Thread.CurrentThread.CurrentCulture = originalCulture;
        }
    }

    public string? GetFieldNameForProperty(PropertyInfo propertyInfo)
    {
        throw new NotImplementedException();
    }

    public HttpContent ToHttpContent<T>(T item)
    {
        return new StringContent(string.Empty);
    }
}