using System;

namespace GlobeTest.InputLayer
{
	public enum InputDirection : byte
	{
		Backwards = 1,
		Down = 2,
		Forwards = 4,
		Left = 8,
		Right = 16,
		Up = 32
	}
}
