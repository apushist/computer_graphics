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
		public int width;
		public int height;
		private bool enabled = false;

		public bool Enabled
		{
			get => enabled;
			set => enabled = value;
		}

		public int Width => width;
		public int Height => height;

		public ZBuffer(int width, int height)
		{
			this.width = width;
			this.height = height;
			buffer = new float[width, height];
			Clear();
		}

		public void Clear()
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					buffer[x, y] = float.MaxValue;
				}
			}
		}

		public bool TestAndSet(int x, int y, float depth)
		{
			if (!enabled) return true;

			if (x < 0 || x >= width || y < 0 || y >= height)
				return false;

			if (depth < buffer[x, y])
			{
				buffer[x, y] = depth;
				return true;
			}
			return false;
		}

		public void Resize(int newWidth, int newHeight)
		{
			if (newWidth != width || newHeight != height)
			{
				width = newWidth;
				height = newHeight;
				buffer = new float[width, height];
				Clear();
			}
		}
	}
}
