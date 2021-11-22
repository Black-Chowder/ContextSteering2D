using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Spooky_Stealth.Traits
{
    //Top-down movement
    public class TDMovement : Trait
    {
        //Speed at which the entity moves (set by constructor)
        public float speed { get; set; }
        private const float defaultSpeed = 5f;

        //Stores if is controlled by player directly
        public bool directControl { get; set; }

        //Input key variables
        public Keys upKey { get; set; } = Keys.W;
        public Keys leftKey { get; set; } = Keys.A;
        public Keys downKey { get; set; } = Keys.S;
        public Keys rightKey { get; set; } = Keys.D;

        private const string traitName = "topDownMovement";
        public TDMovement(Entity parent, float speed = defaultSpeed, bool directControl = false) : base(traitName, parent)
        {
            this.speed = speed;
            this.directControl = directControl;
        }

        //Wrapper for default move function
        public void Move(GameTime gameTime, Vector2 target)
        {
            //Plug in angle to other move function
            Move(gameTime, MathF.Atan2(target.Y, target.X));
        }

        //Takes angle and determination to change parent delta position
        public void Move(GameTime gameTime, float angle, float determination = 1)
        {
            parent.dy += MathF.Sin(angle) * speed * determination * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
            parent.dx += MathF.Cos(angle) * speed * determination * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
        }

        public override void Update(GameTime gameTime)
        {
            if (directControl) DirectControl(gameTime);
        }

        //Takes direct control from the keyboard and moves player accordingly
        private void DirectControl(GameTime gameTime)
        {
            //Get keyboard state
            KeyboardState keys = Keyboard.GetState();

            //Temp variables that track raw input directions
            Vector2 target = new Vector2();

            //Get key inputs
            bool upKeyPressed = keys.IsKeyDown(upKey);
            bool leftKeyPressed = keys.IsKeyDown(leftKey);
            bool downKeyPressed = keys.IsKeyDown(downKey);
            bool rightKeyPressed = keys.IsKeyDown(rightKey);

            //If no key is pressed, return without influencing player delta position
            if (!upKeyPressed && !downKeyPressed && !leftKeyPressed && !rightKeyPressed) return;

            //Input to target positions
            if (upKeyPressed) //Up
            {
                target.Y -= 1;
            }
            if (leftKeyPressed) //Left
            {
                target.X -= 1;
            }
            if (downKeyPressed) //Down
            {
                target.Y += 1;
            }
            if (rightKeyPressed) //Right
            {
                target.X += 1;
            }

            Move(gameTime, target);
        }
    }
}
