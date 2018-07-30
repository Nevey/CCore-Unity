using UnityEngine;

namespace CCore.Utilities
{
    public static class Converter
    {
		public static string ColorToHex(Color color)
		{
			return string.Format(
				"#{0:X2}{1:X2}{2:X2}",
				FloatToByte(color.r),
				FloatToByte(color.g),
				FloatToByte(color.b)
			);
		}
	
		public static byte FloatToByte(float value)
		{
			value = Mathf.Clamp01(value);

			return (byte)(value * 255);
		}
    }
}