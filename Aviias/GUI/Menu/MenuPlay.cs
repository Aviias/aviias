﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    class MenuPlay
    {
        Button _new;
        Button _load;
        Button _back;
        Timer ButtonNew = new Timer(1.5f);
        Timer ButtonLoad = new Timer(1.5f);
        bool IsTrue;

        public MenuPlay()
        {
            _new = new Button(new Vector2(305, 655), 600, 100);
            _load = new Button(new Vector2(1044, 655), 600, 100);
            _back = new Button(new Vector2(21, -1),100, 50);
        }

        internal void Update(GameTime gameTime, ContentManager Content)
        {
            MouseState mouseState = Mouse.GetState();
            ButtonNew.Decrem(gameTime);
            ButtonLoad.Decrem(gameTime);
            if (Menu.Swap == true && _new.IsPressed(mouseState) && mouseState.Position.X >= _new._position.X && mouseState.Position.Y >= _new._position.Y && mouseState.Position.X <= _new._position.X + _new._width && mouseState.Position.Y <= _new._position.Y + _new._height && ButtonNew.IsDown())
            {
                _new._texture = ".\\Menu\\Button\\nouvelle_rouge";
                IsTrue = true;
                ButtonNew.ReInit();
            }
            else
            {
                _new._texture = ".\\Menu\\Button\\nouvelle_gris";
                IsTrue = false;

                if (_load.IsPressed(mouseState) && mouseState.Position.X >= _load._position.X && mouseState.Position.Y >= _load._position.Y && mouseState.Position.X <= _load._position.X + _load._width && mouseState.Position.Y <= _load._position.Y + _load._height && ButtonLoad.IsDown())
                {
                    _load._texture = ".\\Menu\\Button\\reprendre_rouge";
                    IsTrue = true;
                    ButtonLoad.ReInit();
                }
                else
                {
                    _load._texture = ".\\Menu\\Button\\reprendre_gris";
                    IsTrue = false;
                }
            }
            if (_back.IsPressed(mouseState) && mouseState.Position.X >= _back._position.X && mouseState.Position.Y >= _back._position.Y && mouseState.Position.X <= _back._position.X + _back._width && mouseState.Position.Y <= _back._position.Y + _back._height)
            {
                _back._texture = ".\\Menu\\Button\\retour_rouge";
                Menu.Swap = false;             
            }
            else
            {
                _back._texture = ".\\Menu\\Button\\retour_gris";
            }
        }

        public bool IsTrueF
        {
           get { return IsTrue; }
           set { IsTrue = value; }
        }
       
        internal void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            spriteBatch.Draw(content.Load<Texture2D>(".\\Menu\\Background\\menu_partie"), new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);
            _new.Draw(spriteBatch, content, new Vector2(_new._position.X, _new._position.Y + 20));
            _load.Draw(spriteBatch, content, new Vector2(_load._position.X, _load._position.Y + 20));
            _back.Draw(spriteBatch, content, new Vector2(_back._position.X, _back._position.Y + 20));
        }

    }
}
