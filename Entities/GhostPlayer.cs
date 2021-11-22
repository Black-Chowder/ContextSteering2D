using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Black_Magic;
using Black_Magic.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Spooky_Stealth.Traits;

namespace Spooky_Stealth.Entities
{
    public class GhostPlayer : Entity
    {

        //Texture Variables
        private static Texture2D spriteSheet;

        private const string className = "ghostPlayer";
        public GhostPlayer(float x = 0, float y = 0) : base(className, x, y)
        {
        }

        public static void LoadContent(ContentManager Content)
        {
            spriteSheet = Content.Load<Texture2D>(@"GhostBoi");
        }

        public override void Update(GameTime gameTime)
        {
            //Debug.WriteLine("Position = (" + x + ", " + y + ", " + z);

            //Update Base Traits
            base.traitUpdate(gameTime);

        }
    }
}
