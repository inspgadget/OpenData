using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

using GlobeTest.DataLayer;

namespace GlobeTest.PersistenceLayer
{
	public class IndicatorsXmlLoader : IIndicatorsLoader
	{
		public const string XmlFile = "indicators";
		
		public const string IndicatorTagName = "indicator";
		public const string CodeAttributeName = "code";
		public const string NameAttributeName = "name";
		public const string TopicAttributeName = "topic";
		
		public const string IndicatorsXPath = "/indicators/indicator";
		
		public IndicatorsXmlLoader()
		{
		}
		
		public List<WorldBankIndicator> loadIndicators()
		{
			List<WorldBankIndicator> indicators = new List<WorldBankIndicator>();
			
			//load the xml
			TextAsset xmlAsset = (TextAsset)Resources.Load(IndicatorsXmlLoader.XmlFile);
			XmlDocument document = new XmlDocument();
			document.LoadXml(xmlAsset.text);
			
			//select the continents
			foreach(XmlNode nodeContinent in document.SelectNodes(IndicatorsXmlLoader.IndicatorsXPath))
			{
				WorldBankIndicator indicator = new WorldBankIndicator();
				indicators.Add(indicator);
				
				//attributes
				foreach(XmlAttribute attributeIndicator in nodeContinent.Attributes)
				{
					switch(attributeIndicator.Name)
					{
						case IndicatorsXmlLoader.NameAttributeName:
							indicator.Name = attributeIndicator.Value;
							break;
						
						case IndicatorsXmlLoader.CodeAttributeName:
							indicator.Code = attributeIndicator.Value;
							break;
						
						case IndicatorsXmlLoader.TopicAttributeName:
							indicator.Topic = attributeIndicator.Value;
							break;
						
						default:
							break;
					}
				}
			}
			
			return indicators;
		}
	}
}
