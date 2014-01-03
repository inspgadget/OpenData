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
		ScrollDown = 64,
		ScrollLeft = 128,
		ScrollRight = 256,
		ScrollUp = 512,
		WipeDown = 1024,
		WipeLeft = 2048,
		WipeRight = 4096,
		WipeUp = 8192,
		ZoomIn = 16384,
		ZoomOut = 32768
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
