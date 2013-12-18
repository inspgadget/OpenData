using System;
using System.Collections.Generic;

namespace Globetrotter.DataLayer
{
	public interface IWorldBankDataParser
	{
		IList<WorldBankDataItem> parseXml(string xml);
	}
}
