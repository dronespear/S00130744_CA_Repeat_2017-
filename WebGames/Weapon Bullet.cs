using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WebGames
{
    class weapon_bullet : Sprites
    {
        
        private Vector2 direction;  //the direction this bullet moves in
        private float speed = 1;    //the speed at which it moves

        public bool IsLive { get; set; }    //a property to state if this bullet is live or not

        public Rectangle BoundingAxe;
        public bool InCollision = false;
        //the width and height of our texture
        int spriteWidth = 0;

        public int SpriteWidth
        {
            get { return spriteWidth; }
            set { spriteWidth = value; }
        }
        int spriteHeight = 0;

        public int SpriteHeight
        {
            get { return spriteHeight; }
            set { spriteHeight = value; }
        }

        //pass in a texture, a position and a speed for our bullet to move at
        public weapon_bullet(Texture2D texture, Vector2 position, float speed)
            : base(texture, position, new Rectangle(0, 0, texture.Width, texture.Height))  //Sprite construtor parameters
        {
            this.speed = speed; //set up the speed
            

        }
        public bool collisionDetect(weapon_bullet otherSprite)
        {

            BoundingAxe = new Rectangle((int)this.position.X, (int)this.position.Y, this.spriteWidth, this.spriteHeight);
            Rectangle otherBound = new Rectangle((int)otherSprite.position.X, (int)otherSprite.position.Y, otherSprite.spriteWidth, this.spriteHeight);
            if (BoundingAxe.Intersects(otherBound))
            {
                InCollision = true;
                return true;
            }
            else
            {
                InCollision = false;
                return false;
            }
        }
        public void Move(Vector2 delta)
        {
            position += delta;
            // update the new position of the Bounding Rect for an Animated sprite
            BoundingAxe = new Rectangle((int)this.position.X, (int)this.position.Y, this.spriteWidth, this.spriteHeight);

        }
        public override void Update()
        {
            if (!IsLive)    //if we aren't live then we don't update
                return;

            position += direction * speed;  //increase the position by the the direction times the speed
            base.Update();
        }
       
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsLive)  //if we are live then we draw this bullet
                base.Draw(spriteBatch);
           
        }

        //A read write property for the Direction
        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

    }
}

