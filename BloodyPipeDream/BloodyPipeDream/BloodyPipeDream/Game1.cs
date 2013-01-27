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
    class Globals
    {
        public static int GRID_SIZE = 10;
		public static int PRESSURE_INCREASE = 1;
		public static int PRESSURE_DECREASE = 10;
		public static int MSEC_PER_PRESSURE = 100;
    }



	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		private GraphicsDeviceManager Graphics;
		private SpriteBatch SpriteBatch;
		private BloodyGrid _grid;

		enum GameMode { Menu, Game };
		enum GameDifficulty { Easy, Medium, Hard };
		Input Input;
		GameMode Mode;
		GameDifficulty Diff;
		Menu Menu;
		TileQueue TileLookahead;
		BloodPressure BP;

		public static int ScreenWidth, ScreenHeight;
		public static SpriteFont Font;
		public static SpriteFont TitleFont;
		public static SpriteFont SmallFont;

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
			ScreenWidth = 800;
			ScreenHeight = 800;
			Graphics.PreferredBackBufferWidth = ScreenWidth;
			Graphics.PreferredBackBufferHeight = ScreenHeight;
			Graphics.ApplyChanges();

			base.Initialize();

			Rectangle gridArea = new Rectangle(
				ScreenWidth / 10,
				ScreenHeight / 10,
				ScreenWidth - 2 * (ScreenWidth / 10),
				ScreenHeight - 2 * (ScreenHeight / 10));
			_grid = new BloodyGrid(10, 10, gridArea);

			int lookaheadXPos = ScreenWidth - (ScreenWidth / 10) + (ScreenWidth / 100);
			int lookaheadWidth = (ScreenWidth / 12);
			TileLookahead = new TileQueue(lookaheadXPos, lookaheadWidth);

			int BPxPos = (ScreenWidth / 100);
			int BPWidth = (ScreenWidth / 12);
			BP = new BloodPressure(BPxPos, BPWidth);
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			Font = Content.Load<SpriteFont>("font/SpriteFont2");
			TitleFont = Content.Load<SpriteFont>("font/SpriteFont1");
			SmallFont = Content.Load<SpriteFont>("font/SpriteFont3");

            BloodyStartTile.loadContent(this);
            BloodyStraightTile.loadContent(this);
            BloodyNullTile.loadContent(this);
            BloodyCurvedTile.loadContent(this);
            BloodyEndTile.loadContent(this);
			TileQueue.loadContent(this);
			BloodPressure.loadContent(this);

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
			if (Mode == GameMode.Game)
			{
				BP.Update(gameTime);
			}

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

			// draw the title
			Rectangle title = new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight / 4);
			string titleStr = "BLOODY PIPE DREAM";
			Vector2 titleSize = Game1.TitleFont.MeasureString(titleStr);
			Vector2 titlePos = new Vector2((Game1.ScreenWidth / 2) - (titleSize.X / 2), 0);
			SpriteBatch.DrawString(Game1.TitleFont, titleStr, titlePos, Color.DarkRed, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);


			if (Mode == GameMode.Menu)
			{
				Menu.Draw(SpriteBatch);
			}
			else
			{
                _grid.drawTiles(SpriteBatch);
                _grid.drawCursor(SpriteBatch);
				TileLookahead.Draw(SpriteBatch);
				BP.Draw(SpriteBatch);
			}

			SpriteBatch.End();
			base.Draw(gameTime);
            
		}

		protected void HandleInput()
		{
			Input.Update();

			if (Mode == GameMode.Menu)
			{
				if (Input.Back && !Input.WasBack) { this.Exit(); }
				if (Input.Start && !Input.WasStart && Menu.Position == 3) { this.Exit(); }
				if (Menu.Position == 3 && (Input.AnyButton || Input.Start)) { this.Exit(); }
				if (Menu.Position == 0 && (Input.AnyButton || Input.Start)) { Diff = GameDifficulty.Easy; StartGame(); }
				if (Menu.Position == 1 && (Input.AnyButton || Input.Start)) { Diff = GameDifficulty.Medium; StartGame(); }
				if (Menu.Position == 2 && (Input.AnyButton || Input.Start)) { Diff = GameDifficulty.Hard; StartGame(); }
				if (Input.Up && !Input.WasUp) { Menu.MoveUp(); }
				if (Input.Down && !Input.WasDown) { Menu.MoveDown(); }
	        }
			else if (Mode == GameMode.Game)
			{
				if (Input.Back) { Mode = GameMode.Menu; }
				if (Input.Up && !Input.WasUp) { _grid.moveCursor(0, -1); }
                if (Input.Left && !Input.WasLeft) { _grid.moveCursor(-1, 0); }
                if (Input.Right && !Input.WasRight) { _grid.moveCursor(1, 0); }
                if (Input.Down && !Input.WasDown) { _grid.moveCursor(0, 1); }
				
                if (Input.Button1 && !Input.WasButton1)
				{
					// place the pipe at the current cursor position
					_grid.attemptInsertAtCursor(ref TileLookahead);
				}

				if (Input.Button2 && !Input.WasButton2)
				{
					// pump the blood through the pipes
					
					// lower the blood pressure
					BP.decreasePressure(Globals.PRESSURE_DECREASE);
				}
			}
		}

		protected void StartGame()
		{
			_grid.clearGrid();
			TileLookahead.Clear();
			switch (Diff)
			{
				case GameDifficulty.Easy:
					_grid.initialize(5, 5);
					// add 5 lookaheads
					TileLookahead.Push(_grid.generateRandomTile());
					TileLookahead.Push(_grid.generateRandomTile());
					TileLookahead.Push(_grid.generateRandomTile());
					TileLookahead.Push(_grid.generateRandomTile());
					TileLookahead.Push(_grid.generateRandomTile());

					// set the pressure speeds
					Globals.PRESSURE_DECREASE = 10;
					Globals.PRESSURE_INCREASE = 1;
					break;
				case GameDifficulty.Medium:
					_grid.initialize(7, 7);
					// add 3 lookaheads
					TileLookahead.Push(_grid.generateRandomTile());
					TileLookahead.Push(_grid.generateRandomTile());
					TileLookahead.Push(_grid.generateRandomTile());

					// set the pressure speeds
					Globals.PRESSURE_DECREASE = 8;
					Globals.PRESSURE_INCREASE = 2;
					break;
				case GameDifficulty.Hard:
					_grid.initialize(10, 10);
					// add 1 lookahead
					TileLookahead.Push(_grid.generateRandomTile());

					// set the pressure speeds
					Globals.PRESSURE_DECREASE = 5;
					Globals.PRESSURE_INCREASE = 3;
					break;
			}


			// dummy initialization
// 			_grid.setStart(new BloodyStartTile(2), 0, 1);
// 			_grid.setEnd(new BloodyEndTile(), 11, 5);
// 			_grid.insert(new BloodyStraightTile(1), 1, 2);
// 			_grid.insert(new BloodyStraightTile(0), 1, 3);
// 			_grid.insert(new BloodyCurvedTile(0), 2, 1);
// 			_grid.insert(new BloodyCurvedTile(1), 2, 2);
// 			_grid.insert(new BloodyCurvedTile(2), 3, 1);
// 			_grid.insert(new BloodyCurvedTile(3), 3, 2);
// 			bool canInsert;
// 			canInsert = _grid.canInsert(new BloodyStraightTile(1), 1, 0);
// 			canInsert = _grid.canInsert(new BloodyStraightTile(1), 2, 0);
// 			canInsert = _grid.canInsert(new BloodyStraightTile(0), 2, 0);
// 			canInsert = _grid.canInsert(new BloodyCurvedTile(0), 2, 0);
// 			canInsert = _grid.canInsert(new BloodyCurvedTile(1), 2, 0);
// 			canInsert = _grid.canInsert(new BloodyCurvedTile(2), 2, 0);
// 			canInsert = _grid.canInsert(new BloodyCurvedTile(3), 2, 0);
// 			canInsert = _grid.canInsert(new BloodyCurvedTile(3), 2, 0);
			BP.resetPressure();
			Mode = GameMode.Game;
		}
	}
}
