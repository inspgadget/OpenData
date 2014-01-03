using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

using Globetrotter.DomainLayer;

namespace Globetrotter.PersistenceLayer
{
	public class ContinentCountryXmlLoader : IContinentCountryLoader
	{
		public const string XmlFile = "continents_countries";
		
		public const string ContinentTagName = "continent";
		public const string IsoAlphaThreeCodeAttributeName = "isoalphathreecode";
		public const string NameAttributeName = "name";
		
		public const string ContinentsXPath = "/continents/continent";
		public const string CountriesXPath = "/continents/continent/country";
		public const string CountriesForContinentXPath = "/continents/continent[@name='?']/country";
		
		public ContinentCountryXmlLoader()
		{
		}

		public Country loadCountry(string isoAlphaThreeCode)
		{
			return null;
		}
		
		public IList<Country> LoadCountries()
		{
			List<Country> countries = new List<Country>();
			
			//load the xml
			TextAsset xmlAsset = (TextAsset)Resources.Load(ContinentCountryXmlLoader.XmlFile);
			XmlDocument document = new XmlDocument();
			document.LoadXml(xmlAsset.text);
			
			//select the countries
			foreach(XmlNode nodeContinent in document.SelectNodes(ContinentCountryXmlLoader.CountriesXPath))
			{
				Country country = new Country();
				countries.Add(country);
				
				//attributes
				foreach(XmlAttribute attributeCountry in nodeContinent.Attributes)
				{
					switch(attributeCountry.Name)
					{
						case ContinentCountryXmlLoader.NameAttributeName:
							country.Name = attributeCountry.Value;
							break;

						case ContinentCountryXmlLoader.IsoAlphaThreeCodeAttributeName:
							country.IsoAlphaThreeCode = attributeCountry.Value;
							break;
						
						default:
							break;
					}
				}
			}
			
			return countries;
		}
		
		/*public List<Country> LoadCountries()
		{
			return new List<Country>();
		}*/
		
		/*private List<Country> LoadCountriesForContinent(XmlDocument document, string continent)
		{
			List<Country> countries = new List<Country>();
			
			foreach(XmlNode nodeCountry in document.SelectNodes(ContinentCountryXmlLoader.CountriesForContinentXPath.Replace("?",continent)))
			{
				Country country = new Country();
				countries.Add(country);
				
				//attributes
				foreach(XmlAttribute attributeCountry in nodeCountry.Attributes)
				{
					switch(attributeCountry.Name)
					{
						case ContinentCountryXmlLoader.IsoAlphaThreeCodeAttributeName:
							country.IsoAlphaThreeCode = attributeCountry.Value;
							break;
						
						case ContinentCountryXmlLoader.NameAttributeName:
							country.Name = attributeCountry.Value;
							break;
						
						case ContinentCountryXmlLoader.PlaneTextureAttributeName:
							country.PlaneTexture = (Texture2D)Resources.Load(attributeCountry.Value);
							break;
						
						case ContinentCountryXmlLoader.RotationXAttributeName:
							country.RotationX = float.Parse(attributeCountry.Value,
									System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
							break;
						
						case ContinentCountryXmlLoader.RotationYAttributeName:
							country.RotationY = float.Parse(attributeCountry.Value,
									System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
							break;
						
						case ContinentCountryXmlLoader.RotationZAttributeName:
							country.RotationZ = float.Parse(attributeCountry.Value,
									System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
							break;
						
						case ContinentCountryXmlLoader.XAttributeName:
							country.X = float.Parse(attributeCountry.Value);
							break;
						
						case ContinentCountryXmlLoader.ZAttributeName:
							country.Z = float.Parse(attributeCountry.Value);
							break;
						
						default:
							break;
					}
				}
			}
			
			return countries;
		}*/
	}
}
