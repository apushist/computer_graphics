using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
	public static class MathUtils
	{
		// Преобразование градусов в радианы
		public static double DegreesToRadians(double degrees)
		{
			return degrees * Math.PI / 180.0;
		}

		// Преобразование радианов в градусы
		public static double RadiansToDegrees(double radians)
		{
			return radians * 180.0 / Math.PI;
		}

		// Ограничение значения в диапазоне
		public static double Clamp(double value, double min, double max)
		{
			return value < min ? min : value > max ? max : value;
		}
	}
}
