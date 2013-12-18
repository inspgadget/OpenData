using System.Collections.Generic;

using Globetrotter.DomainLayer;

namespace Globetrotter.PersistenceLayer
{
	public interface IContinentCountryLoader
	{
		Country loadCountry(string isoAlphaThreeCode);

		IList<Country> LoadCountries();
	}
}
