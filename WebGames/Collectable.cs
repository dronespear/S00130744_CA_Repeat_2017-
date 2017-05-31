using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WebGames
{
    public class Collectable : SimpleSprite
    {
        int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        Color tint;

        public Collectable(Texture2D cx, Vector2 position)
            : base(cx, position)
        {
            Random r = new Random();
            _value = r.Next(2, 6);
            if (_value > 4)
                tint = Color.Aqua;
            else tint = Color.White;
        }

        public Collectable(Texture2D cx, Vector2 position, int _value) : this(cx, position)
        {
            this._value = _value;
            tint = Color.White;
        }

        public void draw(SpriteBatch cp)
        {
            if (Visible)
                cp.Draw(Image, Position, tint);
        }
    }
}