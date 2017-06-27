﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MonoGame.Extended;
using Aviias.GUI;
using Aviias.IA;

namespace Aviias
{
    [Serializable]
    public class Player
    {
        [field: NonSerialized]
        Texture2D PlayerTexture;
        [field: NonSerialized]
        public Vector2 Position;
        int _health;
       // [field: NonSerialized]
        Text text;
        public bool _displayPos;
        string _str;
       // [field: NonSerialized]
        public List<Quest> _activeQuest;
        int _resistance;
        int _damage;
        bool _isDie;
        string _texture;
        float x;
        float y;
        bool _collisions;
        bool Active;
        float _moveSpeed;
        bool _stopDamage;
        List<int> list = new List<int>(16);
      //  [field: NonSerialized]
        Save save;
        [field: NonSerialized]
        KeyboardState currentKeyboardState;
        [field: NonSerialized]
        KeyboardState previousKeyboardState;
        [field: NonSerialized]
        List<Monster> monsters = new List<Monster>();
        [field: NonSerialized]
        MouseState mouseState = Mouse.GetState();
        [field: NonSerialized]
        MouseState currentMouseState = Mouse.GetState();
        int previousMouseState;
        public bool isInAir;
        public float _yVelocity;
        public double _gravity;
        public float _jumpHeight;
        int _nbBlocs;
        public bool flyMod;
        public bool IsInventoryOpen;
        Map _map;
        Animation PMoveLeft;
        Animation PMoveRight;
        Animation CurrentAnim;
      //  [field: NonSerialized]
        Timer playerTimer = new Timer(1.2f);
      //  [field: NonSerialized]
        Timer invenTimer = new Timer(1.3f);
       // [field: NonSerialized]
        Timer craftTimer = new Timer(1.5f);
       // [field: NonSerialized]
        Timer blocBreakTimer = new Timer(1.5f);
        //[field: NonSerialized]
        Timer blockDurationTimer = new Timer(1.5f);
        //[field: NonSerialized]
        Timer setBlocTimer = new Timer(1.2f);
        //[field: NonSerialized]
        Timer scrollToolBarTimer = new Timer(1.1f);
        Timer stopDamageTimer = new Timer(12f);
        Timer stopDamageCDTimer = new Timer(5f);

        List<string> CraftNotPutable = new List<string>();
        

        internal Dictionary<Ressource, int> _inventory;
        //   MonoGame.Extended.Camera2D Camera;
        float _playerMoveSpeed;
        internal Inventory _inv;
        List<Soul> _souls = new List<Soul>(32);

        public int Width
        {
            get { return PlayerTexture.Width; }
        }

        public int Height
        {
            get { return PlayerTexture.Height; }
        }

        public float X
        {
            get { return Position.X; }
           
        }

        public float Y
        {
            get { return Position.Y; }
        }

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public int Resistance
        {
            get { return _resistance; }
            set { _resistance = value; }
        }

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public void LoadContent(ContentManager content)
        {
            PMoveLeft = new Animation(content, "gauche", 50f,3,Position);
            PMoveRight = new Animation(content, "droite", 50f, 3, Position);
        }

        public void Initialize(Texture2D texture, Vector2 position, ContentManager content, Map map)
        {
            PlayerTexture = texture;
            Position = position;
            Active = true;
            _resistance = 0;
            _damage = 20;
            _health = 100;
            text = new Aviias.Text(content);
            _displayPos = true;
            _isDie = false;
            _gravity = 1;
            _jumpHeight = -12;
            _moveSpeed = 0.8f;
            _map = map;
            _activeQuest = new List<Quest>(8);
            _inv = new Inventory(this);
            save = new Save(map, this, Game1._npc);
            previousMouseState = currentMouseState.ScrollWheelValue;
            x = position.X;
            y = position.Y;
            _texture = texture.Name;
            _stopDamage = false;
            CraftNotPutable.Add("stick");
            CraftNotPutable.Add("wood_shovel");
            CraftNotPutable.Add("heal_potion");
            /*
            _inv.AddInventory(2, "oak_wood");
            _inv.AddInventory(4, "oak_plank");
            _inv.AddInventory(500, "dirt");
            _inv.AddInventory(70, "stone");
            _inv.AddInventory(12, "bedrock");
            _inv.AddInventory(45, "bookshelf");
            _inv.AddInventory(27, "coal_ore");
            _inv.AddInventory(117, "glass");
            _inv.AddInventory(80, "iron_ore");
            _inv.AddInventory(1000, "stonebrick");
            _inv.AddInventory(247, "oak_leaves");
            */
        }

        public Vector2 PlayerPosition
        {
            get { return Position; }
        }

        public void AddQuest(Quest quest)
        {
            _activeQuest.Add(quest);
        }

        public void RemoveQuest(Quest quest)
        {
            _activeQuest.Remove(quest);
        }

            
        public float PlayerMoveSpeed
        {
            get { return _playerMoveSpeed; }
            set { _playerMoveSpeed = value; }
        }

        public bool IsDie
        {
            get { return _isDie; }
            set { _isDie = value; }
        }

        public void GetDamage(int damage)
        {
            int newHealth = _health - (damage - _resistance);
            if (newHealth <= 0)
            {
                _isDie = true;
            }
            else
            {
                _health = newHealth;
            }
        }

        public void breakBloc(Bloc bloc, ContentManager content, Bloc[,] blocs, int i, int j, int scale, StreamWriter log)
        {
            Bloc bloc1;
            if (bloc != null)
            {
                //log.WriteLine("---- > breakBloc i=" + i + " j=" + j);
                if (bloc.IsBreakable)
                {
                    bloc1 = new Bloc(blocs[i,j].GetPosBlock ,scale, "air", content);
                    _inv.AddInventory(1, blocs[i, j].Type);
                    blocs[i, j] = bloc1;
                    
                    //log.WriteLine("---- > breakBloc block.X = " + blocs[i, j].GetPosBlock.X + " block.Y = " + blocs[i, j].GetPosBlock.Y);
                }
            }
            _map.ActualizeShadow((int)Position.X, (int)Position.Y);
        }

        public void setbloc(Bloc bloc, ContentManager content, Bloc[,] blocs, int i, int j, int scale, string name, Map map)
        {
            Bloc bloc1;
            if (bloc != null && blocs[i, j].Type == "air" && Vector2.Distance(PlayerPosition, bloc.GetPosBlock) <= 400) 
            {
                bloc1 = new Bloc(blocs[i, j].GetPosBlock, scale, name, content);
                _inv.DecreaseInventory(1, name);
                blocs[i, j] = bloc1;
                map.ActualizeShadow((int)PlayerPosition.X,(int)PlayerPosition.Y);
            }
        }

        bool IsOnLadder(Map map)
        {
            for (int a = (int)(Position.Y / 16); a < (Position.Y / 16) + 3; a++)
            {
                for (int b = (int)(Position.X / 16); b < (Position.X) / 16 + 1; b++)
                {
                    if (map._blocs[b, a] != null && map._blocs[b, a].Type == "ladder") return true;
                }
            }
            return false;
        }

        internal void Jump(Map map)
        {
            if (!IsInAir(map) && !IsOnLadder(map))
            {
                _yVelocity = _jumpHeight;
                Position.Y -= 10;
            }
        }

        internal bool IsStopDamage
        {
            get { return _stopDamage; }
            set { _stopDamage = value; }
        }

        internal void Update(Map map)
        {
            if (!flyMod && !IsOnLadder(map))
            {
                if (IsInAir(map))
                {
                    Position.Y += _yVelocity;
                    if (_yVelocity < 12) _yVelocity += (float)_gravity;
                }
                else
                {
                    _yVelocity = 0;
                }
            }
        }

        internal bool IsInAir(Map map)
        {
            List<int> list = new List<int>(16);
            list = GetCollisionSide(GetBlocsAround(map));
            if (list.Contains(3)) return false;
            return true;
        }

        public bool PutableBloc(List<string> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i] == name)
                {
                    return false;
                }
            }

            return true;
        }

        internal void Update(Player player, Camera2D Camera, List<NPC> _npc, GameTime gameTime, ContentManager Content, StreamWriter log, Map map, List<Monster> monsters)
        {
            currentKeyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            invenTimer.Decrem(gameTime);
            playerTimer.Decrem(gameTime);
            craftTimer.Decrem(gameTime);
            blocBreakTimer.Decrem(gameTime);
            setBlocTimer.Decrem(gameTime);
            scrollToolBarTimer.Decrem(gameTime);
            stopDamageCDTimer.Decrem(gameTime);
            stopDamageTimer.Decrem(gameTime);
            bool tmp = false;
            foreach (Soul soul in _souls)
            {
                soul.Update(gameTime);
                if (soul.IsDown())
                {
                    _souls.Remove(soul);
                    tmp = true;
                }
                if (tmp) break;
            }

            list = GetCollisionSide(GetBlocsAround(map));

            if(stopDamageTimer.IsDown())
            {
                IsStopDamage = false;
                stopDamageTimer.ReInit();
            }

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                if (!list.Contains(2))
                {
                    Position.X -= _playerMoveSpeed;
                    PMoveLeft.PlayAnim(gameTime);
                    CurrentAnim = PMoveLeft;
                }
                
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                if (!list.Contains(1))
                {
                    Position.X += _playerMoveSpeed;
                    PMoveRight.PlayAnim(gameTime);
                    CurrentAnim = PMoveRight;
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                Position.Y -= _playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
             //   Camera.Move(new Vector2(0, +_playerMoveSpeed));
                player.Position.Y += _playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.E) && invenTimer.IsDown())
            {
                IsInventoryOpen = !IsInventoryOpen;
                invenTimer.ReInit();
            }

            if (currentKeyboardState.IsKeyDown(Keys.C) && craftTimer.IsDown() && IsInventoryOpen)
            {
                for (int i = 0; i < _inv._craft._cellCraft.Length; i++)
                {
                    if (_inv._craft._cellCraft[i].IsCraftable == true)
                    {
                        _inv.AddInventory(_inv._craft._cellCraft[i]._quantity, _inv._craft._cellCraft[i]._name);

                        foreach (KeyValuePair<int, Ressource> element in _inv._craft._cellCraft[i]._ressource)
                        {
                            _inv.DecreaseInventory(element.Key, element.Value.Name);
                        }
                        break;
                    }
                }
                craftTimer.ReInit();

            }


            if (currentKeyboardState.IsKeyDown(Keys.Space) /*|| currentKeyboardState.IsKeyDown(Keys.Up)*/)
            {
                Jump(map);
            }

               if ((System.Windows.Forms.Control.MouseButtons & System.Windows.Forms.MouseButtons.Left) == System.Windows.Forms.MouseButtons.Left)
               {

                mouseState = Mouse.GetState();
                Vector2 position = new Vector2(mouseState.X, mouseState.Y);
                position = Camera.ScreenToWorld(position);

                for (int i = 0; i < monsters.Count; i++)
                {
                    if (position.X >= monsters[i].MonsterPosition.X && position.X <= monsters[i].MonsterPosition.X + monsters[i].Width && position.Y >= monsters[i].MonsterPosition.Y && position.Y <= monsters[i].MonsterPosition.Y + monsters[i].Height)
                    {
                        if (playerTimer.IsDown() && Vector2.Distance(player.PlayerPosition, position) <= 400)
                        {
                            monsters[i].GetDamage(player.Damage);
                            if (monsters[i].IsDie)
                            {
                                Soul soul = new Soul(monsters[i].MonsterPosition, Content, monsters[i].BaseDamage, monsters[i].BaseHealth);
                                _souls.Add(soul);
                                monsters.Remove(monsters[i]);
                            }
                            playerTimer.ReInit();
                        }
                            
                    }
                }
                
                if (mouseState.LeftButton == ButtonState.Pressed && blocBreakTimer.IsDown() && !IsInventoryOpen)
                {
                    blockDurationTimer.Decrem(gameTime);
                    if (blockDurationTimer.IsDown() && Vector2.Distance(player.PlayerPosition, position) <= 100)
                    {
                        map.FindBreakBlock(position, player, Content, log);
                        blocBreakTimer.ReInit();
                        blockDurationTimer.ReInit();
                    }
                    
                }
              
            }

            if (currentKeyboardState.IsKeyDown(Keys.NumPad1) && scrollToolBarTimer.IsDown())
            {
                _inv.ActualCell = 0;
                scrollToolBarTimer.ReInit();
                
            } else if (currentKeyboardState.IsKeyDown(Keys.NumPad2) && scrollToolBarTimer.IsDown())
            {
                _inv.ActualCell = 1;
                scrollToolBarTimer.ReInit();
            }
            else if (currentKeyboardState.IsKeyDown(Keys.NumPad3) && scrollToolBarTimer.IsDown())
            {
                _inv.ActualCell = 2;
                scrollToolBarTimer.ReInit();
            }
            else if (currentKeyboardState.IsKeyDown(Keys.NumPad4) && scrollToolBarTimer.IsDown())
            {
                _inv.ActualCell = 3;
                scrollToolBarTimer.ReInit();
            }
            else if (currentKeyboardState.IsKeyDown(Keys.NumPad5) && scrollToolBarTimer.IsDown())
            {
                _inv.ActualCell = 4;
                scrollToolBarTimer.ReInit();
            }
            else if (currentKeyboardState.IsKeyDown(Keys.NumPad6) && scrollToolBarTimer.IsDown())
            {
                _inv.ActualCell = 5;
                scrollToolBarTimer.ReInit();
            }
            else if (currentKeyboardState.IsKeyDown(Keys.NumPad7) && scrollToolBarTimer.IsDown())
            {
                _inv.ActualCell = 6;
                scrollToolBarTimer.ReInit();
            }
            else if (currentKeyboardState.IsKeyDown(Keys.NumPad8) && scrollToolBarTimer.IsDown())
            {
                _inv.ActualCell = 7;
                scrollToolBarTimer.ReInit();
            }
            else if (currentKeyboardState.IsKeyDown(Keys.NumPad9) && scrollToolBarTimer.IsDown())
            {
                _inv.ActualCell = 8;
                scrollToolBarTimer.ReInit();
            }
            else if (currentKeyboardState.IsKeyDown(Keys.NumPad0) && scrollToolBarTimer.IsDown())
            {
                _inv.ActualCell = 9;
                scrollToolBarTimer.ReInit();
            }

            previousMouseState = currentMouseState.ScrollWheelValue;

            if ((System.Windows.Forms.Control.MouseButtons & System.Windows.Forms.MouseButtons.Right) == System.Windows.Forms.MouseButtons.Right && setBlocTimer.IsDown())
            {
                mouseState = Mouse.GetState();
                Vector2 position = new Vector2(mouseState.X, mouseState.Y);
                position = Camera.ScreenToWorld(position);
                int cell = _inv.ActualCell;
                string name = _inv.GetNameBloc(cell);
                // Type is true when it is a craft
                if (_inv.IsOnInventory(name) && PutableBloc(CraftNotPutable, name))
                {
                    map.SetBloc(position, Content, player, name, map);
                    setBlocTimer.ReInit();
                }
            }

                if (currentKeyboardState.IsKeyDown(Keys.P))
            {
                if (player._displayPos) player._displayPos = false;
                else player._displayPos = true;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                player.AddStr("a");
            }
            if (currentKeyboardState.IsKeyDown(Keys.I))
            {
                foreach (NPC npc in _npc) if (map.GetDistance(player.PlayerPosition, npc.Position) < 400) npc.Interact(player);
            }

            if (currentKeyboardState.IsKeyDown(Keys.G))
            {
                flyMod = !flyMod;
            }

            if (currentKeyboardState.IsKeyDown(Keys.N))
            {
                for(int i = 0; i < _inv._cellArray.Length; i++)
                {
                    if(_inv._cellArray[i]._name == "heal_potion" && _inv._cellArray[i]._quantity >= 1)
                    {
                        RegenerateHealth(50);
                        _inv.DecreaseInventory(1, "heal_potion");
                    }
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                map.TimeForward();
                map.ActualizeShadow((int)Position.X, (int)Position.Y);
            }

            if (currentKeyboardState.IsKeyDown(Keys.F) && stopDamageCDTimer.IsDown())
            {
                IsStopDamage = true;
                stopDamageCDTimer.ReInit();
            }


            if (currentKeyboardState.IsKeyDown(Keys.X))
            {

                map.ActualizeShadow((int)Position.X, (int)Position.Y);
            }


            if (currentKeyboardState.IsKeyDown(Keys.M))
            {
                /*  map = new Map(200, 200);
                  map.GenerateMap(Content);*/

                Game1.map = save.DeserializeMap();
                Game1.map.Reload(Content);
                Game1.player = save.DeserializePlayer();
                Game1.player.ReloadPlayer(Content);
                Game1.player.text.Reload(Content);
                Game1._npc = save.DeserializeNpc();
                foreach (NPC npc in Game1._npc) npc.Reload(Content);
            }

            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                x = Position.X;
                y = Position.Y;
                save = new Save(map, this, Game1._npc);
                save.SerializeMap();
                save.SerializePlayer();
                save.SerializeNpc();
            }
        }

        internal void RegenerateHealth(int quantity)
        {
            Health += quantity;
            if (Health > 100) Health = 100;
        }

        internal void UpdatePlayerCollision(GameTime gameTime, Player player, List<Monster> monsters)
        {
            Rectangle playerRect;
            Rectangle monsterRect;
            

            playerRect = new Rectangle((int)player.X, (int)player.Y, player.Width, player.Height);

            for (int i = 0; i < monsters.Count; i++)
            {
                monsterRect = new Rectangle((int)monsters[i].posX, (int)monsters[i].posY, monsters[i].Width, monsters[i].Height);
                if (playerRect.Intersects(monsterRect))
                {
                    // Collision between player and monster
                    if (Math.Abs(playerRect.Center.X - monsterRect.Center.X) > Math.Abs(playerRect.Center.Y - monsterRect.Center.Y))
                    {
                        if (playerRect.Center.X < monsterRect.Center.X)
                        {
                            monsters[i].posX = playerRect.Right - monsters[i].moveSpeed;
                        }
                        if (playerRect.Center.X > monsterRect.Center.X)
                        {
                            monsters[i].posX = playerRect.Left - monsters[i].Width - monsters[i].moveSpeed;
                        }
                    }
                    else
                    {
                        if (playerRect.Center.Y < monsterRect.Center.Y)
                        {
                            monsters[i].posY = playerRect.Bottom - monsters[i].moveSpeed;
                        }
                        if (playerRect.Center.Y > monsterRect.Center.Y)
                        {
                            monsters[i].posY = playerRect.Top - monsters[i].Height - monsters[i].moveSpeed;
                        }
                    }
                }
            }

        }
            internal void UpdateCollision(Map map, Player player) {

            _nbBlocs = 0;

            List<Bloc> _blocs = new List<Bloc>(16);
            for (int a = (int)(Position.Y / 16); a < (Position.Y / 16) + 8; a++)
            {
                for (int b = (int)(Position.X / 16); b < (Position.X) / 16 + 8; b++)
                {
                    if (a >= 0 && b >= 0 && a < map._worldHeight && b < map._worldWidth && map._blocs[b, a] != null && map._blocs[b, a].Type != "air")
                    {
                        _blocs.Add(map._blocs[b, a]);
                        _nbBlocs++;
                    }
                }
            }
            
            List<int> list = new List<int>(16);
            list = GetCollisionSide(_blocs);
        }

        public List<int> GetCollisionSide(List<Bloc> _blocs)
        {
            List<int> result = new List<int>(16);
            Rectangle playerRect;
            Rectangle playerRect2;
            playerRect = new Rectangle((int)Position.X, (int)Position.Y, PlayerTexture.Width, PlayerTexture.Height);
            playerRect2 = new Rectangle((int)Position.X, (int)Position.Y + 1, PlayerTexture.Width, PlayerTexture.Height);

            Rectangle rectTest = new Rectangle((int)Position.X, (int)Position.Y - 10, PlayerTexture.Width, PlayerTexture.Height);

            for (int i = 0; i < _blocs.Count; i++)
            {
                if (_blocs[i] != null)
                {
                    Rectangle blocRect;
                    blocRect = new Rectangle((int)_blocs[i].posX, (int)_blocs[i].posY, _blocs[i].Width, _blocs[i].Height);
                    if (playerRect.Intersects(blocRect))
                    {
                        if (playerRect.Bottom > blocRect.Top && playerRect.Bottom < blocRect.Bottom)
                        {
                            result.Add(3);
                            if(!rectTest.Intersects(blocRect)) Position.Y -= 1;
                        }
                         if (playerRect.Top < blocRect.Bottom && playerRect.Top > blocRect.Top) result.Add(4);
                        //  if (playerRect.Left < blocRect.Right && playerRect.Left > blocRect.Left) result.Add(2);
                        //  if (playerRect.Right > blocRect.Left && playerRect.Right < blocRect.Right) result.Add(1);
                        if (rectTest.Left < blocRect.Right && rectTest.Left > blocRect.Left) result.Add(2);
                        if (rectTest.Right > blocRect.Left && rectTest.Right < blocRect.Right) result.Add(1);
                        _collisions = true;
                    }
                    if (playerRect2.Intersects(blocRect))
                    {
                        if (playerRect2.Bottom > blocRect.Top && playerRect2.Bottom < blocRect.Bottom)
                        {
                            _yVelocity = 0;
                            result.Add(3);
                        }
                    }
                }
            }
            return result;
        }
        
        
        
        internal List<Bloc> GetBlocsAround(Map map)
        {
            _nbBlocs = 0;

            List<Bloc> _blocs = new List<Bloc>(16);

            for (int a = (int)(Position.Y / 16); a < (Position.Y / 16) + 8; a++)
            {
                for (int b = (int)(Position.X / 16); b < (Position.X) / 16 + 8; b++)
                {
                    if (a >= 0 && b >= 0 && a < map._worldHeight && b < map._worldWidth && map._blocs[b, a] != null && map._blocs[b, a].Type != "air" && map._blocs[b, a].Type != "ladder")
                    {
                        _blocs.Add(map._blocs[b, a]);
                        _nbBlocs++;
                    }
                }
            }

            return _blocs;
        }

        public string ImageHealth(int health)
        {
            return (Math.Floor((double)health / 10)*10).ToString();
        }

        internal void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            if (CurrentAnim != null) CurrentAnim.Draw(spriteBatch, Position);
            else spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(content.Load<Texture2D>(ImageHealth(_health)), new Vector2(Position.X - 950, Position.Y - 500), null, Color.White, 0f, Vector2.Zero, 1.1f,
               SpriteEffects.None, 0f);
        //    if (_displayPos) text.DisplayText((Position.X  + " - " + Position.Y), new Vector2(Position.X, Position.Y - 30), spriteBatch, Color.Red);
         //   text.DisplayText(("" +_health + "/"  + "100"), new Vector2(Position.X - 785, Position.Y - 420), spriteBatch, Color.White);

            if(IsInventoryOpen)
            {
                _inv.Draw(spriteBatch, content);
            }else
            {
                spriteBatch.Draw(content.Load<Texture2D>("Barre d'inventaire"), new Vector2(Position.X - 400, Position.Y +474), null, Color.White, 0f, Vector2.Zero, 1f,
                    SpriteEffects.None, 0f);
                for(int i=0; i<10; i++)
                {
                    if (_inv.PositionToolBar(i).IsFull == true)
                    {
                        spriteBatch.Draw(content.Load<Texture2D>(_inv.PositionToolBar(i)._name), _inv.PositionToolBar(i).Position, null, Color.White, 0f, Vector2.Zero, 0.8f,
                            SpriteEffects.None, 0f);
                        text.DisplayText("" + _inv.PositionToolBar(i)._quantity, new Vector2(_inv.PositionToolBar(i).Position.X, _inv.PositionToolBar(i).Position.Y + 100), spriteBatch, Color.Black);
                    }
                }
                spriteBatch.Draw(content.Load<Texture2D>("Roullette"), _inv.PositionCellToolBar(), null, Color.White, 0f, Vector2.Zero, 1f,
                    SpriteEffects.None, 0f);

            }

            foreach(Soul soul in _souls)
            {
                soul.Draw(spriteBatch);
            }

            if (IsDie == true)
            {
                spriteBatch.Draw(content.Load<Texture2D>("gameover"), new Vector2(Position.X, Position.Y), null, Color.White, 0f, Vector2.Zero, 1f,
                     SpriteEffects.None, 0f);
            }
            
            

        }

        public void AddStr(string str)
        {
             _str += str;
        }

        void ReloadPlayer(ContentManager content)
        {
            Position = new Vector2(x, y);
            PlayerTexture = content.Load<Texture2D>(_texture);
        }

        public Dictionary<Ressource, int> Inventory => _inventory;
    }
}