using UnityEngine;
using System;

namespace Globetrotter.PersistenceLayer
{
	public interface IFlagLoader
	{
		Texture LoadFlag(string isoAlphaThreeCode);
	}
}
