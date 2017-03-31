using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;
        GraphicsDeviceManager mGraphics;
        SpriteBatch mSpriteBatch;
        static public SpriteBatch sSpriteBatch; // Drawing support
        static public ContentManager sContent; // Loading textures
        static public GraphicsDeviceManager sGraphics; // Current display size
        static public Random sRan;
        // Prefer window size
        const int kWindowWidth = 800;
        const int kWindowHeight = 600;
        const int kNumObjects = 4;
        // Work with the TexturedPrimitive class
        TexturedPrimitive.TexturedPrimitive[] mGraphicsObjects; // An array of objects
        int mCurrentIndex = 0;

        TexturedPrimitive.TexturedPrimitive mUWBLogo;
        SoccerBall mBall;
        Vector2 mSoccerPosition = new Vector2(50, 50);
        float mSoccerBallRadius = 3f;
        MyGame2 mTheGame;

        public Game1()
        {
            //mGgraphics = new GraphicsDeviceManager(this);
            //Content.RootDirectory = "Content";
            mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Set preferred window size
            mGraphics.PreferredBackBufferWidth = kWindowWidth;
            mGraphics.PreferredBackBufferHeight = kWindowHeight;
            Game1.sRan = new Random();
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Create a new SpriteBatch, which can be used to draw textures.
            Game1.sSpriteBatch = new SpriteBatch(GraphicsDevice);
            // Create the primitives.
            mGraphicsObjects = new TexturedPrimitive.TexturedPrimitive[kNumObjects];
            mGraphicsObjects[0] = new TexturedPrimitive.TexturedPrimitive(
                    "UWB-JPG", // Image file name
                    new Vector2(10, 10), // Position to draw
                    new Vector2(30, 30)); // Size to draw
            mGraphicsObjects[1] = new TexturedPrimitive.TexturedPrimitive(
                    "UWB-JPG",
                    new Vector2(200, 200),
                    new Vector2(100, 100));
            mGraphicsObjects[2] = new TexturedPrimitive.TexturedPrimitive(
                    "UWB-PNG",
                    new Vector2(50, 10),
                    new Vector2(30, 30));
            mGraphicsObjects[3] = new TexturedPrimitive.TexturedPrimitive(
                    "UWB-PNG",
                    new Vector2(50, 200),
                    new Vector2(100, 100));

            // Define camera window bounds
            Camera.SetCameraWindow(new Vector2(10f, 20f), 100f);

            // Create the primitives
            mUWBLogo = new TexturedPrimitive.TexturedPrimitive("UWB-PNG", new Vector2(30, 30), new Vector2(20, 20));
            mBall = new SoccerBall(mSoccerPosition, mSoccerBallRadius * 2f);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // Allows the game to exit
            if (InputWrapper.InputWrapper.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // "A" to toggle to full-screen mode
            if (InputWrapper.InputWrapper.Buttons.A == ButtonState.Pressed)
            {
                if (!mGraphics.IsFullScreen)
                {
                    mGraphics.IsFullScreen = true;
                    mGraphics.ApplyChanges();
                }
            }

            // "B" toggles back to windowed mode
            if (InputWrapper.InputWrapper.Buttons.B == ButtonState.Pressed)
            {
                if (mGraphics.IsFullScreen)
                {
                    mGraphics.IsFullScreen = false;
                    mGraphics.ApplyChanges();
                }
            }

            mUWBLogo.Update(InputWrapper.InputWrapper.ThumbSticks.Left, Vector2.Zero);
            mBall.Update();
            mBall.Update(Vector2.Zero, InputWrapper.InputWrapper.ThumbSticks.Right);

            if (InputWrapper.InputWrapper.Buttons.A == ButtonState.Pressed)
                mBall = new SoccerBall(mSoccerPosition, mSoccerBallRadius * 2f);


            base.Update(gameTime);

            mTheGame.UpdateGame(gameTime);
            if (InputWrapper.InputWrapper.Buttons.A == ButtonState.Pressed)
                mTheGame = new MyGame2();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // Clear to background color
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Game1.sSpriteBatch.Begin(); // Initialize drawing support
                                        // Loop over and draw each primitive
                                        // Print out text message to echo status
            FontSupport.PrintStatus("Selected object is:" + mCurrentIndex + " Location=" + mGraphicsObjects[mCurrentIndex].mPosition, null);
            FontSupport.PrintStatusAt(mGraphicsObjects[mCurrentIndex].mPosition, "Selected", Color.Red);
            foreach (TexturedPrimitive.TexturedPrimitive p in mGraphicsObjects)
            {
                p.Draw();
            }
            Game1.sSpriteBatch.End(); // Inform graphics system we are done drawing

            // Clear to background color
            mUWBLogo.Draw();
            mBall.Draw();
            // Print out text message to echo status
            FontSupport.PrintStatus("Ball Position:" + mBall.Position, null);
            FontSupport.PrintStatusAt(mUWBLogo.mPosition,
            mUWBLogo.mPosition.ToString(), Color.White);
            FontSupport.PrintStatusAt(mBall.Position, "Radius" + mBall.Radius, Color.Red);

            mTheGame.DrawGame();

            base.Draw(gameTime);        
        }
    }
}
