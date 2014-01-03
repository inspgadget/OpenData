using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Globetrotter.DataLayer
{
    public class WorldBankDataParser : IWorldBankDataParser
    {
        public const string IndicatorTagName = "indicator";
        public const string CodeAttributeName = "id";
        public const string CountryTagName = "country";
        public const string DateTagName = "date";
        public const string ValueTagName = "value";

        public const string DataXPath = "/data/data";
		
        public IList<WorldBankDataItem> parseXml(string xml)
        {
            return loadData(xml);
        }
        private IList<WorldBankDataItem> loadData(string xml)
        {
            IList<WorldBankDataItem> listOfworldBankData = new List<WorldBankDataItem>();

            //load the xml

            string data = xml.Replace("wb:", "");
            byte[] encodedString = Encoding.UTF8.GetBytes(data);

            // Put the byte array into a stream and rewind it to the beginning
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;

            XmlDocument document = new XmlDocument();
            document.Load(ms);

            foreach(XmlNode nodeContinent in document.SelectNodes(WorldBankDataParser.DataXPath))
            {
                WorldBankDataItem item = new WorldBankDataItem();
                listOfworldBankData.Add(item);

                foreach (XmlElement element in nodeContinent.ChildNodes)
                {
                    switch (element.Name)
                    {
                        case WorldBankDataParser.CountryTagName:
                            if(!string.IsNullOrEmpty(element.InnerText))
                                item.Country = element.InnerText;
                            break;

                        case WorldBankDataParser.DateTagName:
                            if (!string.IsNullOrEmpty(element.InnerText))
                                item.Year = int.Parse(element.InnerText);
                            break;

                        case WorldBankDataParser.ValueTagName:
                            if (!string.IsNullOrEmpty(element.InnerText))
                                item.Value = double.Parse(element.InnerText, CultureInfo.InvariantCulture);
                            break;

                        case WorldBankDataParser.IndicatorTagName:
                            if (!string.IsNullOrEmpty(element.InnerText))
                                item.IndicatorName = element.InnerText;
                            foreach (XmlAttribute attributeData in element.Attributes)
                            {
                                if (attributeData.Name == WorldBankDataParser.CodeAttributeName && !string.IsNullOrEmpty(attributeData.Value))
                                {
                                    item.IndicatorCode = attributeData.Value;
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            return listOfworldBankData;
        }
    }
}