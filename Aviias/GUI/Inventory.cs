﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace Aviias
{
    [Serializable]
    public class Inventory
    {
        int _actualCell;
        Player _player;
        public _cell[]  _cellArray;
        private Text text;
        public Craft _craft;
        
        public Inventory(Player player)
        {
            _player = player;
            _cellArray = new _cell[40];
            _craft = new Craft();
            _actualCell = 0;
            
            for (int i = 0; i < 40; i++)
            {
                _cellArray[i] = new _cell();
                _cellArray[i]._name = "";
                _cellArray[i]._ressource = new Ressource("air");
            }
        }

        public void UpdatePosition(int i, Camera2D camera)
        {
            int _difX = 77;
            float _difY = 82;
            if (i == 0)
            {
                _cellArray[i].Position = new Vector2(camera.Position.X + 650, camera.Position.Y + 590);
            }
            else
            {
                if (i > 0 && i < 10)
                {
                    _cellArray[i].Position = new Vector2(_cellArray[i - 1].Position.X + _difX, _cellArray[0].Position.Y);
                }
                else
                {
                    if ((i % 10) == 0)
                    {
                        _cellArray[i].Position = new Vector2(_cellArray[0].Position.X, _cellArray[0].Position.Y + _difY);
                        _difY = _difY + _difY;
                    }
                    else
                    {
                        _cellArray[i].Position = new Vector2(_cellArray[i - 1].Position.X + _difX, _cellArray[0].Position.Y + _difY);
                    }
                }
            }
        }

        public _cell PositionToolBar(int i, Camera2D camera)

        {
            int _difX = 77;
            if (i == 0)
            {
                _cellArray[i].Position = new Vector2(camera.Position.X + 584, camera.Position.Y + 1022);
                return _cellArray[i];
            }
            else if (i > 0 && i < 10)
            {
                _cellArray[i].Position = new Vector2(_cellArray[i - 1].Position.X + _difX, _cellArray[0].Position.Y);
                return _cellArray[i];
            }
            return _cellArray[40];
        }

        public bool IsFull(int i)
        {
            if(_cellArray[i]._quantity == 0)
            {
                _cellArray[i].IsFull = false;
            }
            return _cellArray[i].IsFull;
        }

        public Vector2 PositionCellToolBar(Camera2D camera)
        {
            float DifX = 77;
            if (_actualCell == 0)
            {
                return new Vector2(camera.Position.X + 575, camera.Position.Y + 1012);
            }
            else
            {
                return new Vector2(camera.Position.X + DifX * _actualCell, camera.Position.Y + 474);
            }
        }

        public _cell[] Array
        {
            get { return _cellArray; }
        }

        public int ActualCell
        {
            get { return _actualCell; } 
            set { _actualCell = value; }           
        }

        public string GetNameBloc(int x)
        {
            return _cellArray[x]._name;
        }

        [Serializable]
        public struct _cell
        {
            [field: NonSerialized]
            public Vector2 Position;
            public bool IsFull { get; set; }
            public string _name { get; set; }
            public int _quantity { get; set; }
            public Ressource _ressource { get; set; }
        }

        public Vector2 Position { get { return Position; } set { Position = value; } }

        public void ReinitCell(int i)
        {
            _cellArray[i] = new _cell();
            _cellArray[i]._name = "";
            _cellArray[i]._quantity = 0;
            _cellArray[i].IsFull = false;
            _cellArray[i]._ressource = new Ressource("air");
        }

        public void AddInventory(int quantity, string name)
        {
            //foreach ( entry in _cellArray)
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if (_cellArray[i]._name == name && _cellArray[i]._quantity == 0)
                {
                    _cellArray[i].IsFull = true;
                    _cellArray[i]._quantity += quantity; return;
                }
                else if (_cellArray[i]._name == name)
                {
                    _cellArray[i]._quantity += quantity; return;
                }
            }

            for (int i = 0; i < _cellArray.Length; i++)
            {
                if (!_cellArray[i].IsFull && _cellArray[i]._name != name) { _cellArray[i]._name = name; _cellArray[i]._quantity = quantity; _cellArray[i].IsFull = true; _cellArray[i]._ressource = new Ressource(name); break; }
            }
        }

        public bool IsOnInventory(string name)
        {
            for (int i = 0; i < _cellArray.Length; i++)
            {
            if (_cellArray[i]._name == name && IsFull(i))
                {
                    return true;
                }
            }
            return false;         
        }

        public int Quantity(string name)
        {
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if (_cellArray[i]._name == name && IsFull(i))
                {
                    return _cellArray[i]._quantity;
                }
            }
            return -1;
        }

        public string GetName(Vector2 pos)
        {
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if (_cellArray[i].Position.X >= pos.X && _cellArray[i].Position.X < pos.X + 70 && _cellArray[i].Position.Y >= pos.Y && _cellArray[i].Position.Y < pos.Y + 69)
                {
                    return _cellArray[i]._name;
                }
            }

            return null;
        }

        public bool PutableBloc(List<string> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == name)
                {
                    return false;
                }
            }

            return true;
        }
        /*
        public void TriInventory(List<string> CraftNotPutable)
        {
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if(i < 10 && !PutableBloc(CraftNotPutable, _cellArray[i]._name))
                {
                    if(FirstEmptyCell() != -1)
                    {
                        _cellArray[FirstEmptyCell()] = _cellArray[i];
                        ReinitCell(i);
                    }
                } 
            }
        }

        public int FirstEmptyCell()
        {
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if(_cellArray[i]._name == "")
                {
                    return i;
                }
            }
            return -1;
        }
        */
        public void DecreaseInventory(int quantity, string name)
        {
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if(_cellArray[i]._name == name && _cellArray[i]._quantity == 1)
                {
                    _cellArray[i].IsFull = false;
                    _cellArray[i]._quantity -= quantity;
                    return;
                }
                else if (_cellArray[i]._name == name)
                {
                    _cellArray[i]._quantity -= quantity;
                    return;
                }
               
            }
        }

        public void PostionCraft(int i, Camera2D camera)
        {
            if (i == 0)
            {
                _craft._cellCraft[i]._position = new Vector2(_cellArray[0].Position.X + 825, _cellArray[0].Position.Y -450);
            }
            //else if (_craft._cellCraft[i].IsCraftable == true)
            //{
            //    _craft._cellCraft[i]._position = new Vector2(_craft._cellCraft[i - 1]._position.X + 500, _craft._cellCraft[i - 1]._position.Y - 250);
            //}
        }

        internal void Draw(SpriteBatch spriteBatch, ContentManager content, Camera2D camera)
        {
            text = new Text(content);
            spriteBatch.Draw(content.Load<Texture2D>("Inventaire"), new Vector2(camera.Position.X + 576, camera.Position.Y + 140), null, Color.White, 0f, Vector2.Zero, 1f,
                SpriteEffects.None, 0f);
            spriteBatch.Draw(content.Load<Texture2D>("babyplayer"), new Vector2(camera.Position.X + 965, camera.Position.Y + 190), null, Color.White, 0f, Vector2.Zero, 3.4f,
                SpriteEffects.None, 0f);
            for (int i=0; i<40; i++)
            {
                UpdatePosition(i, camera);
                if (_cellArray[i]._name != "" && IsFull(i))
                {
                    spriteBatch.Draw(content.Load<Texture2D>(_cellArray[i]._name), _cellArray[i].Position, null, Color.White, 0f, Vector2.Zero, 0.8f,
                        SpriteEffects.None, 0f);
                    text.DisplayText("" + _cellArray[i]._quantity, new Vector2(_cellArray[i].Position.X, _cellArray[i].Position.Y + 100), spriteBatch, Color.Black);
                }
            }
            _craft.IsCraftable(_cellArray);
            for (int i = 0; i < _craft._cellCraft.Length; i++)
            {
                PostionCraft(i, camera);
                if (_craft._cellCraft[i].IsCraftable == true)
                {
                        spriteBatch.Draw(content.Load<Texture2D>("craft"), _craft._cellCraft[i]._position, null, Color.White, 0f, Vector2.Zero, 1.1f,
                            SpriteEffects.None, 0f);
                        spriteBatch.Draw(content.Load<Texture2D>(_craft._cellCraft[i]._name), new Vector2(_craft._cellCraft[i]._position.X + 12, _craft._cellCraft[i]._position.Y +12), null, Color.White, 0f, Vector2.Zero, 0.8f,
                            SpriteEffects.None, 0f);
                }
            }
        }
    }

}
