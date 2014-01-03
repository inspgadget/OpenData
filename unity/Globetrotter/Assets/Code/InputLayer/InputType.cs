using System;

namespace Globetrotter.InputLayer
{
	public enum InputType
	{
		DoubleClick = 1,
		LongClock = 2,
		RotateDown = 4,
		RotateLeft = 8,
		RotateRight = 16,
		RotateUp = 32,
		WipeDown = 64,
		WipeLeft = 128,
		WipeRight = 256,
		WipeUp = 512,
		ZoomIn = 1024,
		ZoomOut = 2048
	}

	public static class InputTypeExtensions
	{
		public static InputType And(this InputType a, InputType b)
		{
			return a & b;
		}

		public static InputType Or(this InputType a, InputType b)
		{
			return a | b;
		}

		public static InputType Xor(this InputType a, InputType b)
		{
			return a ^ b;
		}
	}
}
