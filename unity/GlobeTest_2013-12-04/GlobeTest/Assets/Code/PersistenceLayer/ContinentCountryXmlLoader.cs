using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

using GlobeTest.DomainLayer;

namespace GlobeTest.PersistenceLayer
{
	public class ContinentCountryXmlLoader : IContinentCountryLoader
	{
		public const string XmlFile = "continents_countries";
		
		public const string ContinentTagName = "continent";
		public const string IsoAlphaThreeCodeAttributeName = "isoalphathreecode";
		public const string NameAttributeName = "name";
		public const string PlaneTextureAttributeName = "planetexture";
		public const string RotationXAttributeName = "rotationx";
		public const string RotationYAttributeName = "rotationy";
		public const string RotationZAttributeName = "rotationz";
		public const string XAttributeName = "x";
		public const string ZAttributeName = "z";
		
		public const string ContinentsXPath = "/continents/continent";
		public const string CountriesForContinentXPath = "/continents/continent[@name='?']/country";
		
		public ContinentCountryXmlLoader()
		{
		}
		
		public List<Continent> LoadContinents()
		{
			List<Continent> continents = new List<Continent>();
			
			//load the xml
			TextAsset xmlAsset = (TextAsset)Resources.Load(ContinentCountryXmlLoader.XmlFile);
			XmlDocument document = new XmlDocument();
			document.LoadXml(xmlAsset.text);
			
			//select the continents
			foreach(XmlNode nodeContinent in document.SelectNodes(ContinentCountryXmlLoader.ContinentsXPath))
			{
				Continent continent = new Continent();
				continents.Add(continent);
				
				//attributes
				foreach(XmlAttribute attributeContinent in nodeContinent.Attributes)
				{
					switch(attributeContinent.Name)
					{
						case ContinentCountryXmlLoader.NameAttributeName:
							continent.Name = attributeContinent.Value;
							break;
						
						case ContinentCountryXmlLoader.PlaneTextureAttributeName:
							continent.PlaneTexture = (Texture2D)Resources.Load(attributeContinent.Value);
							break;
						
						case ContinentCountryXmlLoader.RotationXAttributeName:
							continent.RotationX = float.Parse(attributeContinent.Value,
									System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
							break;
						
						case ContinentCountryXmlLoader.RotationYAttributeName:
							continent.RotationY = float.Parse(attributeContinent.Value,
									System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
							break;
						
						case ContinentCountryXmlLoader.RotationZAttributeName:
							continent.RotationZ = float.Parse(attributeContinent.Value,
									System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
							break;
						
						default:
							break;
					}
				}
				
				//countries
				continent.Countries = LoadCountriesForContinent(document, continent.Name);
			}
			
			return continents;
		}
		
		public List<Country> LoadCountriesForContinent(string continent)
		{
			return new List<Country>();
		}
		
		private List<Country> LoadCountriesForContinent(XmlDocument document, string continent)
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
		}
	}
}
