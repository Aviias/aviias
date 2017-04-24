﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace Aviias
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Monster monster;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        float playerMoveSpeed;
        // Texture2D texture;
        Map map = new Map(28, 50);
        Random random = new Random();
        int prob = 3;
        BoxingViewportAdapter _viewportAdapter;
        const int WindowWidth = 900;
        const int WindowHeight = 600;
        Camera2D _camera;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.PreferredBackBufferWidth = WindowWidth;

            Window.AllowUserResizing = true;
        }

        public Camera2D Camera
        {
            get
            {
                return _camera;
            }
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

            player = new Player();
            playerMoveSpeed = 8.0f;
            monster = new Monster(100, 1.0f, 0.05, 10, 5 );
            
            
            base.Initialize();
            map.GenerateMap(Content);

            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, WindowWidth, WindowHeight);
            _camera = new Camera2D(_viewportAdapter);
            _camera.LookAt(new Vector2(player.Position.X + 10, player.Position.Y + 15));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
           
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("test"), playerPosition);

            Vector2 monsterPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            monster.Initialize(Content.Load<Texture2D>("mob"), monsterPosition);

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
            currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.M))
            {
                map = new Map(28, 50);
                map.GenerateMap(Content);
            }
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
                
            UpdatePlayer(gameTime);
            monster.Update(player);
            UpdateMonster(gameTime);
            base.Update(gameTime);
        }

        private void UpdateMonster(GameTime gameTime)
        {
            monster._pos.X = MathHelper.Clamp(monster._pos.X, 0, GraphicsDevice.Viewport.Width - monster.Width);
            monster._pos.Y = MathHelper.Clamp(monster._pos.Y, 0, GraphicsDevice.Viewport.Height - monster.Height);
        }

        private void UpdatePlayer(GameTime gameTime)
        {

            player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                Camera.Move(new Vector2(-playerMoveSpeed, 0));
                player.Position.X -= playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                Camera.Move(new Vector2(+playerMoveSpeed, 0));
                player.Position.X += playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                Camera.Move(new Vector2(0, -playerMoveSpeed));
                player.Position.Y -= playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                Camera.Move(new Vector2(0, +playerMoveSpeed));
                player.Position.Y += playerMoveSpeed;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
            map.Draw(spriteBatch);
            monster.Draw(spriteBatch);
            player.Draw(spriteBatch);
           
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
