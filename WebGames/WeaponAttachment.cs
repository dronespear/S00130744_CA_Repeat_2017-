using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WebGames
{
    public class WeaponAttachment : Sprites
    {

        protected Vector2 localPosition;    //a local position is this objects position in relation to a parenting position.
        protected Player parentBody;   //A vehicle body to know that this is our parent 

        //Here we pass in teh required items for Sprite
        public WeaponAttachment(Texture2D texture, Vector2 localPosition, Rectangle sourceRectangle)
            : base(texture, localPosition, sourceRectangle)
        {
            this.localPosition = localPosition; //set the localposition
        }

        //same as comments above
        public WeaponAttachment(Texture2D texture, Vector2 localPosition, Rectangle sourceRectangle, float rotation, float scale)
            : base(texture, localPosition, sourceRectangle, rotation, scale, 1)
        {
            this.localPosition = localPosition;
        }

        public override void Update()
        {
            //update our positon to be equalt to the partens position plus our own local position.
            position = parentBody.position + localPosition;
            base.Update();
        }

        //reads and Writes the parentBody varable
        public Player ParentBody
        {
            get { return parentBody; }
            set { parentBody = value; }
        }

    }
}
