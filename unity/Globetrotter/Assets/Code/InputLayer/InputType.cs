using System;

namespace Globetrotter.InputLayer
{
	public enum InputType
	{
		ClickDouble = 1,
		ClickLong = 2,
		FocusNext = 4,
		FocusPrevious = 8,
		RotateDown = 16,
		RotateLeft = 32,
		RotateRight = 64,
		RotateUp = 128,
		ScrollDown = 256,
		ScrollLeft = 512,
		ScrollRight = 1024,
		ScrollUp = 2048,
		WipeDown = 4096,
		WipeLeft = 8192,
		WipeRight = 16384,
		WipeUp = 32768,
		ZoomIn = 65536,
		ZoomOut = 131072,
		None = -1
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
