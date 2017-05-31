using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WebGames
{
    public class Enemy : SimpleSprite
    {
        int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        Color tint;

        public Enemy(Texture2D tx, Vector2 position)
            : base(tx, position)
        {
            Random r = new Random();
            _value = r.Next(15, 35);
            if (_value > 25)
                tint = Color.Aqua;
            else tint = Color.White;
        }

        public void draw(SpriteBatch sp)
        {
            if (Visible)
                sp.Draw(Image, Position, tint);
        }
    }
}