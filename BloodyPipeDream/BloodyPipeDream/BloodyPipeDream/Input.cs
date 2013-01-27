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


		public bool WasBack;
		public bool WasStart;
		public bool WasUp;
		public bool WasDown;
		public bool WasLeft;
		public bool WasRight;
		public bool WasButton1;
		public bool WasButton2;
		public bool WasButton3;
		public bool WasButton4;

		public bool WasAnyKey;
		public bool WasAnyButton;

		private GamePadState OldPad;
		private KeyboardState OldKB;

		public void Update()
		{
			GamePadState pad = GamePad.GetState(PlayerIndex.One);
			KeyboardState kb = Keyboard.GetState();

			Back = (pad.Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape));
			Start = (pad.Buttons.Start == ButtonState.Pressed || kb.IsKeyDown(Keys.Enter));
			Button1 = (pad.Buttons.A == ButtonState.Pressed || kb.IsKeyDown(Keys.D1) || kb.IsKeyDown(Keys.Space));
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


			WasBack = (OldPad.Buttons.Back == ButtonState.Pressed || OldKB.IsKeyDown(Keys.Escape));
			WasStart = (OldPad.Buttons.Start == ButtonState.Pressed || OldKB.IsKeyDown(Keys.Enter));
			WasButton1 = (OldPad.Buttons.A == ButtonState.Pressed || OldKB.IsKeyDown(Keys.D1) || OldKB.IsKeyDown(Keys.Space));
			WasButton2 = (OldPad.Buttons.B == ButtonState.Pressed || OldKB.IsKeyDown(Keys.D2));
			WasButton3 = (OldPad.Buttons.X == ButtonState.Pressed || OldKB.IsKeyDown(Keys.D3));
			WasButton4 = (OldPad.Buttons.Y == ButtonState.Pressed || OldKB.IsKeyDown(Keys.D4));
			WasUp = (OldPad.DPad.Up == ButtonState.Pressed || OldKB.IsKeyDown(Keys.Up));
			WasDown = (OldPad.DPad.Down == ButtonState.Pressed || OldKB.IsKeyDown(Keys.Down));
			WasLeft = (OldPad.DPad.Left == ButtonState.Pressed || OldKB.IsKeyDown(Keys.Left));
			WasRight = (OldPad.DPad.Right == ButtonState.Pressed || OldKB.IsKeyDown(Keys.Right));
			WasAnyKey = (OldKB.GetPressedKeys().Length > 0 ||
				WasBack || WasStart ||
				WasButton1 || WasButton2 || WasButton3 || WasButton4 ||
				WasUp || WasDown || WasLeft || WasRight);
			WasAnyButton = (WasButton1 || WasButton2 || WasButton3 || WasButton4);

			OldPad = pad;
			OldKB = kb;
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
