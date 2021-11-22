using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Spooky_Stealth.Utils
{
    public static class ClickHandler
    {
        //Keyboard Button Clicked Variables
        //Stores the info for a button that can be clicked
        //TODO: Look into if this should be a struct or class
        private class Clicker
        {
            public bool Clicked { get; set; } = false;
            public bool Can { get; set; } = false;
            public Keys Key { get; private set; }

            public Clicker(Keys _key)
            {
                Key = _key;
            }
        }

        //Stores list of established clickers
        //TODO: Probably change to dictionary
        private static List<Clicker> clickers = new List<Clicker>();

        //Mouse Clicked Variables
        public static bool leftClicked { get; private set; } = false;
        private static bool canClickLeft = true;

        public static bool rightClicked { get; private set; } = false;
        private static bool canClickRight = true;

        private static KeyboardState keys;

        //Needs to be run before any functions that take in input
        public static void Update()
        {
            //Handling Keyboard Button Clicks
            keys = Keyboard.GetState();

            //Loop through clickers
            for (int i = 0; i < clickers.Count; i++)
            {
                Clicker clicker = clickers[i];
                bool isKeyDown = keys.IsKeyDown(clicker.Key);

                //If key's not pressed, can click = true
                if (!isKeyDown)
                {
                    clicker.Can = true;
                }
                
                //If key's pressed and can click = true, set clicked to true
                else if (clicker.Can)
                {
                    clicker.Clicked = true;
                    clicker.Can = false;
                    continue;
                }

                //set clicked to false if key pressed = false and can click = false
                clicker.Clicked = false;
            }

            //How the above code operates:
            //If key is pressed and canKey == true, keyClicked = true and canKey = false
            //Else, keyClicked = false
            //When key is lifted, canKey = true


            //Handling Mouse Button Clicks
            MouseState mouse = Mouse.GetState();

            //Left Click Handler
            leftClicked = false;
            if (mouse.LeftButton == ButtonState.Pressed && canClickLeft)
            {
                leftClicked = true;
                canClickLeft = false;
            }
            else if (mouse.LeftButton == ButtonState.Released)
                canClickLeft = true;

            //Right Click Handler
            rightClicked = false;
            if (mouse.RightButton == ButtonState.Pressed && canClickRight)
            {
                rightClicked = true;
                canClickRight = false;
            }
            else if (mouse.RightButton == ButtonState.Released)
                canClickRight = true;
        }

        //Returns bool representing if the key can be consitered clicked or not
        public static bool IsClicked(Keys key)
        {
            //Loop through stored clickers and return value of clicked if found
            for (int i = 0; i < clickers.Count; i++)
            {
                Clicker clicker = clickers[i];

                if (clicker.Key == key) return clicker.Clicked;
            }

            //If key is not set, create new key and return status of key
            clickers.Add(new Clicker(key));
            return keys.IsKeyDown(key);
        }

        public static void InitKey(Keys key)
        {
            clickers.Add(new Clicker(key));
        }
    }
}
