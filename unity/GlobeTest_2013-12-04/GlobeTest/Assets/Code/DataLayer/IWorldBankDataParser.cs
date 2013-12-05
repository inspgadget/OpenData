using System;
using System.Collections.Generic;

namespace GlobeTest.DataLayer
{
	public interface IWorldBankDataParser
	{
		IList<WorldBankDataItem> parseXml(string xml);
	}
}
