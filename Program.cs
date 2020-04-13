using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XmlParse
{
    class Program
    {
        static void Main(string[] args)
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
            <values>
                <xmlSample>
                    <date>2020-04-09T00:00:00</date>
                    <valueOrNull>1</valueOrNull>
                    <countInt></countInt>
                    <aString></aString>
                </xmlSample>
               <xmlSample>
                    <date>2020-04-09T00:00:00</date>
                    <valueOrNull></valueOrNull>
                    <countInt>3</countInt>
                    <aString>asdf</aString>
                </xmlSample>
                <xmlSample>
                    <date>2020-04-09T00:00:00</date>
                    <valueOrNull>1.3</valueOrNull>
                    <countInt>3</countInt>
                    <aString>asdf</aString>
                </xmlSample>
                <xmlSample>
                    <date>2020-04-09T00:00:00</date>
                    <valueOrNull>3,3</valueOrNull>
                    <countInt>3</countInt>
                    <aString>asdf</aString>
                </xmlSample>
            </values>";

            var orderData = XElement.Parse(xml);
            var data = orderData.Descendants("xmlSample").Select(d =>
            {
                return new XmlSample
                {
                Date = d.Element("date").As<DateTime>(),
                ValueOrNull = d.Element("valueOrNull").As<double?>(),
                AString = d.Element("aString").As<string>(),
                CountInt = d.Element("countInt").As<int>()
                };
            });
            Console.WriteLine(JValue.Parse(JsonConvert.SerializeObject(data)).ToString(Formatting.Indented));
            var x = Console.ReadKey();
        }
    }
}
//Adapted from: https: //stackoverflow.com/questions/3531318/convert-changetype-fails-on-nullable-types
public static class XElementExtensions
{
    public static T As<T>(this XElement e)
    {
        if (string.IsNullOrEmpty(e?.Value))
        {
            return default;
        }
        var t = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
        var val = Convert.ChangeType(e.Value?.Replace(",", "."), t, CultureInfo.InvariantCulture);
        return (T) val;
    }
}
public class XmlSample
{
    public DateTime Date { get; set; }
    public double? ValueOrNull { get; set; }
    public int CountInt { get; set; }
    public string AString { get; set; }

}