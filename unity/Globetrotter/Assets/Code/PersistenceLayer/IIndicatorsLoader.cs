using System;
using System.Collections.Generic;

using Globetrotter.DataLayer;

namespace Globetrotter.PersistenceLayer
{
	public interface IIndicatorsLoader
	{
		List<WorldBankIndicator> loadIndicators();
	}
}
