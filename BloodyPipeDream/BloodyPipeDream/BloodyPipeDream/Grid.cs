using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPipeDream
{
	class Grid
	{
		public Pipe[,] Pipes;
		public int Width;
		public int Height;

		public Grid(int width, int height)
		{
			Width = width;
			Height = height;
			Pipes = new Pipe[Width + 2, Height + 2];
		}

	}
}
