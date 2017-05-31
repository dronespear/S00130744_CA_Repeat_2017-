using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebGames
{
    public class Sprites
    {
        protected Texture2D texture;    //the texture to pull images from
        protected Vector2 position;     //the positon on screen
        protected float rotation = 0;   //the angle in radians of the sprite
        protected float scale = 1;      //the size of the sprite
        protected Rectangle sourceRectangle;    //rectangel to diplay a certain part of the texture
        protected float layer;  //the layer depth for drawing
        protected Vector2 origin;   //the point of rotation, scale and positioning of the sprite

        //Pass in teh texture, position and source Rectangle
        public Sprites(Texture2D texture, Vector2 position, Rectangle sourceRectangle)
        {
            this.texture = texture;
            this.position = position;
            this.sourceRectangle = sourceRectangle;

            //set the origin to default the center of the source Rectangle
            this.origin = new Vector2(sourceRectangle.Width, sourceRectangle.Height) * 0.5f;
        }

        //As above with rotation scale and draw layer added in
        public Sprites(Texture2D texture, Vector2 position, Rectangle sourceRectangle, float rotation, float scale, float layer)
        {
            this.texture = texture;
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.rotation = rotation;
            this.scale = scale;
            this.layer = layer;
            this.origin = new Vector2(sourceRectangle.Width, sourceRectangle.Height) * scale * 0.5f;
        }

        //just a template for other sprites
        public virtual void Update()
        {

        }

        //takes in the spritebatch ad utilizes one of the overridden draws
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White, rotation, origin, scale, SpriteEffects.None, layer);
        }

        //A read write for the Position variable
        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }
    }
}

