
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace WebGames
{
    class weapon : WeaponAttachment
    {

        private Vector2 direction;                          //the direction our turret is facing
        private List<weapon_bullet> bullets = new List<weapon_bullet>();  //the ammo for this turret
        private int bulletIndex;                            //an index to get bullets from the collection
        private float shotTimer;                            //a timer to increment each frame for shooting
        private float shotCooldown = 1;                     //how much time needs to pass before we can shoot again

        
      
        //Sprite parameters plus a collection of bullets
        public weapon(Texture2D texture, Vector2 position, Rectangle sourceRectangle, List<weapon_bullet> bullets)
            : base(texture, position, sourceRectangle)
        {
            origin = new Vector2(origin.X, origin.Y * 0.425f);  //a reset to the origin so the turret rotates around the correct point
            this.bullets = bullets; //set our bullet collection
        }

        public override void Update()
        {
            MouseState mouseState = Mouse.GetState();   //get the current state of the mouse
            shotTimer += 1.0f / 60.0f;  //increment by time passed per frame 1.0f = one Second, 60.0f = frames per second

            //if teh mouse button is down and the timer is above or equal to the cooldown then we Shoot.
            if (mouseState.LeftButton == ButtonState.Pressed && shotTimer >= shotCooldown)
            {
                shotTimer = 0;  //reset the timer
                Shoot();    //call shoot
            }

            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);    //get mouse position
            direction = mousePosition - position;   //find the direction from turret to mouse
            direction.Normalize();  //Normalize the direction to be of Unit length i.e. (20,20) -> (0.5f,0.5f)
            float angle = (float)Math.Atan2(direction.Y, direction.X);  //use Atan2 to get appropriate rotation
            rotation = angle + MathHelper.ToRadians(270);   //add the angle to an offset so our cannon is displayed correctly

            //update bullets
            foreach (weapon_bullet bullet in bullets)
                bullet.Update();

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw Bullets
            foreach (weapon_bullet bullet in bullets)
                bullet.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public void Shoot()
        {
            //If we have no bullets or have shot them all we leave this method
            if (bullets.Count == 0 || bulletIndex >= bullets.Count)
                return;
            weapon_bullet bullet = bullets[bulletIndex];   //get the bullet at the bullet index
            bullet.IsLive = true;                   //make the bullet live
            bullet.Position = position + (direction * sourceRectangle.Height);  //set teh correct starting position of the bullet
            bullet.Direction = direction;   //the bullets direction is equal to the cannons direction at this frame
            bulletIndex++;  //increase the bullet Index by 1
        }
        
    }
}