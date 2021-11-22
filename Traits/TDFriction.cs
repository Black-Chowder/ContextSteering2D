using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;

namespace Spooky_Stealth.Traits
{
    public class TDFriction : Trait
    {
        private const float defaultCoefficient = 2;
        public float coefficient { get; set; }
        public bool isAbsolute { get; set; } = false;

        private const string traitName = "topDownFriction";
        public TDFriction(Entity parent, float coefficient = defaultCoefficient) : base(traitName, parent)
        {
            this.coefficient = coefficient;
            base.priority = 150;
        }

        public override void Update(GameTime gameTime)
        {
            if (isAbsolute)
            {
                parent.dx = 0;
                parent.dy = 0;
                return;
            }

            //If parent.delta pos is low enough, just set to 0
            if (MathF.Abs(parent.dx) <= 0.01f)
                parent.dx = 0;
            if (MathF.Abs(parent.dy) <= 0.01f)
                parent.dy = 0;

            //Calculate Friction Force
            Vector2 frictionForce;
            frictionForce.X = parent.dx != 0 ? parent.dx - parent.dx / coefficient : 0;
            frictionForce.Y = parent.dy != 0 ? parent.dy - parent.dy / coefficient : 0;

            //Apply Friction Force
            parent.dx -= frictionForce.X * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
            parent.dy -= frictionForce.Y * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
        }
    }
}
