using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BloodyPipeDream
{
	class Input
	{
		public bool Back;
		public bool Start;
		public bool Up;
		public bool Down;
		public bool Left;
		public bool Right;
		public bool Button1;
		public bool Button2;
		public bool Button3;
		public bool Button4;

		public bool AnyKey;
		public bool AnyButton;

		public void Update()
		{
			GamePadState pad = GamePad.GetState(PlayerIndex.One);
			KeyboardState kb = Keyboard.GetState();

			Back = (pad.Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape));
			Start = (pad.Buttons.Start == ButtonState.Pressed || kb.IsKeyDown(Keys.Enter));
			Button1 = (pad.Buttons.A == ButtonState.Pressed || kb.IsKeyDown(Keys.D1));
			Button2 = (pad.Buttons.B == ButtonState.Pressed || kb.IsKeyDown(Keys.D2));
			Button3 = (pad.Buttons.X == ButtonState.Pressed || kb.IsKeyDown(Keys.D3));
			Button4 = (pad.Buttons.Y == ButtonState.Pressed || kb.IsKeyDown(Keys.D4));
			Up = (pad.DPad.Up == ButtonState.Pressed || kb.IsKeyDown(Keys.Up));
			Down = (pad.DPad.Down == ButtonState.Pressed || kb.IsKeyDown(Keys.Down));
			Left = (pad.DPad.Left == ButtonState.Pressed || kb.IsKeyDown(Keys.Left));
			Right = (pad.DPad.Right == ButtonState.Pressed || kb.IsKeyDown(Keys.Right));
			AnyKey = (kb.GetPressedKeys().Length > 0 ||
				Back || Start ||
				Button1 || Button2 || Button3 || Button4 ||
				Up || Down || Left || Right);
			AnyButton = (Button1 || Button2 || Button3 || Button4);
		}

		public string getKeyString()
		{
			if (Up) return "Up";
			if (Down) return "Down";
			if (Left) return "Left";
			if (Right) return "Right";
			if (Button1) return "A";
			if (Button2) return "B";
			if (Button3) return "X";
			if (Button4) return "Y";
			if (Back) return "Back";
			if (Start) return "Start";
			return "Nothing";
		}
	}
}
