using System.Collections.Generic;

using GlobeTest.DomainLayer;

namespace GlobeTest.PersistenceLayer
{
	public interface IContinentCountryLoader
	{
		List<Continent> LoadContinents();
		
		List<Country> LoadCountriesForContinent(string continent);
	}
}
