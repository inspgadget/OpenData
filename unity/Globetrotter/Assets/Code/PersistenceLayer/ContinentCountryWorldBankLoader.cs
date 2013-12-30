using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Xml;

using Globetrotter.DomainLayer;
using Globetrotter.NetworkLayer;
using Globetrotter.PersistenceLayer;

namespace Globetrotter.PersistenceLayer
{
    public class ContinentCountryWorldBankLoader : IContinentCountryLoader
    {
        private const string DataXPath = "/countries";

		private const string CountryTag = "country";
		private const string CountryTagAttribute = "id";
		private const string NameTag = "name";
		private const string RegionTag = "region";
		private const string CapitalCityTag = "capitalCity";
		private const string LongitudeTag = "longitude";
		private const string LatitudeTag = "latitude";


        public IList<Country> LoadCountries()
        {
            return loadData();
        }

        private IList<Country> loadData()
        {
            IList<Country> listOfCountries = new List<Country>();
            int port = 80;
            string url = "http://api.worldbank.org/countries?per_page=512";

			HttpResponse response = new HttpRequestController().GetResponse(url, port);
			string xml = Encoding.UTF8.GetString(response.Data);
			string data = xml.Replace("wb:", "");
            
			using(MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
			{
				ms.Flush();
				ms.Position = 0;

				XmlDocument document = new XmlDocument();
	            document.Load(ms);
	            
				foreach (XmlNode nodeContinent in document.SelectSingleNode(ContinentCountryWorldBankLoader.DataXPath))
	            {
	                Country item = new Country();
	                listOfCountries.Add(item);

	                foreach (XmlAttribute attributeData in nodeContinent.Attributes)
	                {
						if(attributeData.Name==ContinentCountryWorldBankLoader.CountryTagAttribute && !string.IsNullOrEmpty(attributeData.Value)) 
	                    {
	                        item.IsoAlphaThreeCode = attributeData.Value;
	                    }
	                }

	                foreach (XmlElement element in nodeContinent.ChildNodes)
	                {
	                    switch (element.Name)
	                    {
						case ContinentCountryWorldBankLoader.NameTag:
	                            if(!string.IsNullOrEmpty(element.InnerText)) 
	                                item.Name = element.InnerText;
	                            break;

						case ContinentCountryWorldBankLoader.RegionTag:
	                            if (!string.IsNullOrEmpty(element.InnerText))
	                                item.Continent = element.InnerText;
	                            break;

						case ContinentCountryWorldBankLoader.CapitalCityTag:
	                            if (!string.IsNullOrEmpty(element.InnerText))
	                                item.CapitalCity = element.InnerText;
	                            break;

						case ContinentCountryWorldBankLoader.LongitudeTag:
	                            if (!string.IsNullOrEmpty(element.InnerText))
	                                item.Longitude = double.Parse(element.InnerText, CultureInfo.InvariantCulture);
	                            break;

						case ContinentCountryWorldBankLoader.LatitudeTag:
	                            if (!string.IsNullOrEmpty(element.InnerText))
	                                item.Latitude = double.Parse(element.InnerText, CultureInfo.InvariantCulture);
	                            break;

	                        default:
	                            break;
	                    }
	                }
	            }
			}

            return listOfCountries;
        }

        public Country loadCountry(string isoAlphaThreeCode)
        {
			return null;
        }
    }

}