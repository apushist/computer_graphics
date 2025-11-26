using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
	public class ZBuffer
	{
		private float[,] buffer;
		public int Width { get; private set; }
		public int Height { get; private set; }

		public ZBuffer(int width, int height)
		{
			Width = width;
			Height = height;
			buffer = new float[width, height];
			Clear();
		}

		public void Clear()
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					buffer[x, y] = float.MaxValue;
				}
			}
		}

		public bool TestAndSet(int x, int y, float depth)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height)
				return false;

			if (depth < buffer[x, y])
			{
				buffer[x, y] = depth;
				return true;
			}
			return false;
		}
	}
}
