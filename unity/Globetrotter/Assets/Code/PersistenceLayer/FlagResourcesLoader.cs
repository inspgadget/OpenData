using UnityEngine;
using System;

namespace Globetrotter.PersistenceLayer
{
	public class FlagResourcesLoader : IFlagLoader
	{
		public FlagResourcesLoader()
		{
		}

		public Texture LoadFlag(string isoAlphaThreeCode)
		{
			if(string.IsNullOrEmpty(isoAlphaThreeCode) == false)
			{
				string filename = isoAlphaThreeCode.ToUpper();

				return Resources.Load<Texture>(filename);
			}
			else
			{
				return null;
			}
		}
	}
}
