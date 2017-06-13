﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

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
        // Texture2D texture;
        Map map = new Map(200, 200);
        Random random = new Random();
        BoxingViewportAdapter _viewportAdapter;
        const int WindowWidth = 1920;
        const int WindowHeight = 1088;
        Camera2D _camera;
        public List<NPC> _npc;
        SpriteFont font;
        List<Monster> monsters = new List<Monster>();
        StreamWriter log; // Debug file
        Random rnd = new Random();
        Menu _menu;

        float _spawnTimer = 10f;
        List<int> list = new List<int>(16);
        //Ressource _testRessource = new Ressource();
        const float _spawnTIMER = 10f;

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

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player = new Player();
            player.PlayerMoveSpeed = 8.0f;
            Vector2 monsterPosition;
            _menu = new Menu();
            _menu.Initialize();

            int monsterneed = 5 - monsters.Count;
            if (monsterneed != 0)
            {
                for (int i = 0; i < monsterneed; i++)
                {
                    int posX = rnd.Next(0, map.WorldWidth * 10);
                    int posY = rnd.Next(0, map.WorldHeight * 10);
                    monsterPosition = new Vector2(posX, posY);
                    monster = new Monster(100, 1.0f, 0.05, 1, 5, Content, Content.Load<Texture2D>("alienmonster"), monsterPosition);
                    monsters.Add(monster);
                }
            }
            /*
            if (!File.Exists("logfile.txt"))
            {
                log = new StreamWriter("logfile.txt");
            }
            else
            {
                log = File.AppendText("logfile.txt");
            }
            */
            // Add a new monster in the list            
            base.Initialize();
            map.GenerateMap(Content);
            IsMouseVisible = true;

            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, WindowWidth, WindowHeight);
            _camera = new Camera2D(_viewportAdapter);
            _camera.LookAt(new Vector2(player.Position.X + 10, player.Position.Y + 15));
            _npc = new List<NPC>(8);
            _npc.Add(new NPC(Content, "pnj", spriteBatch, new Vector2(500, 250), 5));
            _npc.Add(new NPC(Content, "pnj", spriteBatch, new Vector2(1400, 300), 3));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");

            Vector2 playerPosition = new Vector2(1500, 345 + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("babyplayer"), playerPosition, Content);

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
            if (_menu.Jouer() == false)
            {
                _menu.Update(gameTime, Content);
            }
            else
            {


                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                Camera.Position = new Vector2(player.Position.X - WindowWidth / 2, player.Position.Y - WindowHeight / 2);
                // TODO: Add your update logic here
                currentKeyboardState = Keyboard.GetState();
                if (currentKeyboardState.IsKeyDown(Keys.M))
                {
                    map = new Map(200, 200);
                    map.GenerateMap(Content);
                }

                previousKeyboardState = currentKeyboardState;
                currentKeyboardState = Keyboard.GetState();

                player.Update(map);
                player.UpdatePlayerCollision(gameTime, player, monsters);
                Camera.Position = new Vector2(player.Position.X - WindowWidth / 2, player.Position.Y - WindowHeight / 2);
                for (int i = 0; i < monsters.Count; i++)
                {
                    if (monsters[i].IsDie == false && monsters[i] != null)
                    {
                        monsters[i].Update(player, gameTime);
                    }

                    list = player.GetCollisionSide(player.GetBlocsAround(map));

                    Camera.Move(new Vector2(-player.PlayerMoveSpeed, 0));
                    // player.Position.X -= player.PlayerMoveSpeed;
                    //  if (player.GetCollisionSide(player.GetBlocsAround(map)) != 2) player.Position.X -= playerMoveSpeed;
                    /*       if (!list.Contains(2)) player.Position.X -= player.PlayerMoveSpeed;
                           if (!list.Contains(1)) player.Position.X += player.PlayerMoveSpeed;*/
                }

                //     if (player.GetCollisionSide(player.GetBlocsAround(map)) != 3) player.Position.Y += playerMoveSpeed;
                //  if (!list.Contains(3)) player.Position.Y += player.PlayerMoveSpeed;

                player.Update(player, Camera, _npc, gameTime, Content, log, map, monsters);
                player.UpdatePlayerCollision(gameTime, player, monsters);
                base.Update(gameTime);


                float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                _spawnTimer -= elapsed;

                if (_spawnTimer < 1)

                {
                    int posX = rnd.Next(0, map.WorldWidth * 10);
                    int posY = rnd.Next(0, map.WorldHeight * 10);
                    Vector2 monsterPosition = new Vector2(posX, posY);
                    monster = new Monster(100, 1.0f, 0.05, 1, 5, Content, Content.Load<Texture2D>("alienmonster"), monsterPosition);
                    /*   foreach (NPC npc in _npc)
                       {
                           if (map.GetDistance(player.PlayerPosition, npc.Position) < 400) npc.Interact(player);
                       }*/
                    monsters.Add(monster);
                    _spawnTimer = _spawnTIMER;
                }

            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (_menu.Jouer() == false)
            {
                spriteBatch.Begin();
                _menu.Draw(spriteBatch, Content);
                spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
                map.Draw(spriteBatch, (int)player.Position.X, (int)player.Position.Y);
                for (int i = 0; i < monsters.Count; i++)
                {
                    monsters[i].Draw(spriteBatch);
                }
                if (player.IsDie == false)
                {
                    player.Draw(spriteBatch, Content);
                }

                //   foreach (NPC npc in _npc) if (npc._isTalking) npc.Talk(new Quest(), spriteBatch);
                foreach (NPC npc in _npc)
                {
                    npc.Draw(spriteBatch);
                    npc.Update();
                }
                spriteBatch.End();
                base.Draw(gameTime);
                // TODO: Add your drawing code here
            }
        }
    }
}
