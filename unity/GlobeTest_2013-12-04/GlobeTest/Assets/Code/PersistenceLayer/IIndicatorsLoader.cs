using System;
using System.Collections.Generic;

using GlobeTest.DataLayer;

namespace GlobeTest.PersistenceLayer
{
	public interface IIndicatorsLoader
	{
		List<WorldBankIndicator> loadIndicators();
	}
}
