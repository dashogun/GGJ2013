using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace BloodyPipeDream
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		private GraphicsDeviceManager Graphics;
		private SpriteBatch SpriteBatch;
		private BloodyGrid Grid;

		enum GameMode { Menu, Game };
		enum GameDifficulty { Easy, Medium, Hard };
		Input Input;
		GameMode Mode;
		GameDifficulty Diff;
		Menu Menu;

		public static int ScreenWidth, ScreenHeight;
		public static SpriteFont Font;
		public static SpriteFont TitleFont;

		public Game1()
		{
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			Input = new Input();
			Mode = GameMode.Menu;
			Menu = new Menu(new String[]{"Start Easy Game", "Start Medium Game", "Start Hard Game", "Exit"});
			ScreenWidth = 1024;
			ScreenHeight = 768;
			Graphics.PreferredBackBufferWidth = ScreenWidth;
			Graphics.PreferredBackBufferHeight = ScreenHeight;
			Graphics.ApplyChanges();
			Font = Content.Load<SpriteFont>("font/SpriteFont2");
			TitleFont = Content.Load<SpriteFont>("font/SpriteFont1");

			base.Initialize();

            BloodyGrid g = new BloodyGrid(3, 3);
            g.setStart(new BloodyStartTile(2), 0, 0);
            g.insert(new BloodyStraightTile(1), 1, 0);
            bool canInsert;
            canInsert = g.canInsert(new BloodyStraightTile(1), 1, 0);
            canInsert = g.canInsert(new BloodyStraightTile(1), 2, 0);
            canInsert = g.canInsert(new BloodyStraightTile(0), 2, 0);
            canInsert = g.canInsert(new BloodyCurvedTile(0), 2, 0);
            canInsert = g.canInsert(new BloodyCurvedTile(1), 2, 0);
            canInsert = g.canInsert(new BloodyCurvedTile(2), 2, 0);
            canInsert = g.canInsert(new BloodyCurvedTile(3), 2, 0);
            canInsert = g.canInsert(new BloodyCurvedTile(3), 2, 0);
			Grid = g;
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			SpriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// TODO: Add your update logic here
			HandleInput();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			SpriteBatch.Begin();
			// TODO: Add your drawing code here

			if (Mode == GameMode.Menu)
			{
				Menu.Draw(SpriteBatch);
			}
			else
			{

			}

			SpriteBatch.End();
			base.Draw(gameTime);
		}

		protected void HandleInput()
		{
			Input.Update();

			if (Mode == GameMode.Menu)
			{
				if (Input.Back) { this.Exit(); }
				if (Input.Start && Menu.Position == 3) { this.Exit(); }
				if (Menu.Position == 3 && (Input.AnyButton || Input.Start)) { this.Exit(); }
				if (Menu.Position == 0 && (Input.AnyButton || Input.Start)) { Diff = GameDifficulty.Easy; Mode = GameMode.Game; }
				if (Menu.Position == 1 && (Input.AnyButton || Input.Start)) { Diff = GameDifficulty.Medium;  Mode = GameMode.Game; }
				if (Menu.Position == 2 && (Input.AnyButton || Input.Start)) { Diff = GameDifficulty.Hard; Mode = GameMode.Game; }
				if (Input.Up && !Input.WasUp) { Menu.MoveUp(); }
				if (Input.Down && !Input.WasDown) { Menu.MoveDown(); }
			}
			else if (Mode == GameMode.Game)
			{
				if (Input.Back) { Mode = GameMode.Menu; }
				if (Input.Up)
				{
					// move the cursor up
				}
				else if (Input.Left)
				{
					// move the cursor left
				}
				else if (Input.Right)
				{
					// move the cursor right
				}
				else if (Input.Down)
				{
					// move the cursor down
				}
				else if (Input.AnyButton)
				{
					// place the pipe at the current cursor position
				}
			}
		}
	}
}
