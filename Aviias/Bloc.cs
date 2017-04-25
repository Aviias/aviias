﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    public class Bloc
    {
        Vector2 _position;
        public Texture2D _texture;
        float _scale;
        string _type;
        bool _isBreakable;
        bool _isAir;

        public Bloc(Vector2 position, float scale, string type, ContentManager content)
        {
            _position = position;
            _texture = content.Load<Texture2D>(type);
            _scale = scale;
            _type = type;
            if (type != "air")
            {
                _isBreakable = true;
                _isAir = false;
            }
            else
            {
                _isBreakable = false;
                _isAir = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, _scale / 64, SpriteEffects.None, 0f);
        }

        public void BreakBloc(Bloc bloc, Vector2 position, ContentManager content)
        {
            if (bloc._isBreakable)
            {
                bloc = new Bloc(position, 16, "air", content);
            }
        }

        public void setBloc(Bloc bloc, Vector2 position, ContentManager content)
        {
            
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
