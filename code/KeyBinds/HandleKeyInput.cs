using GameEngine.GameObjects;
using GameEngine.Render;
using Silk.NET.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static GameEngine.KeyBinds.HandleKeyInput;
namespace GameEngine.KeyBinds
{
    public class HandleKeyInput
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey); //getkeystate - checking pyhsical hardware state
        private static HashSet<int> _downKeys = new HashSet<int>(); //list of keys that have been pressed in the previous frame
        public enum Key
        {
            Space = 0x20,  //VK codes for keys to be uses to check if they are pressed
            Escape = 0x1B,
            Enter = 0x0D,
            Left = 0x25,
            Up = 0x26,
            Right = 0x27,
            Down = 0x28,
            Shift = 0x10,
            Control = 0x11
        }
        public static bool IsKeyDown(char key)
        {
            int vkey = char.ToUpper(key);
            bool isdown = (GetAsyncKeyState(vkey) & 0x8000) != 0;
            return isdown;
        }
        public static bool IsKeyDown(Key key)
        {
            bool isdown = (GetAsyncKeyState((int)key) & 0x8000) != 0;
            return isdown;
        }
        //checks chars like w/e/f
        public static bool IsKeyPressed(char key)
        {
            int vKey = char.ToUpper(key); 
            bool isDown =  (GetAsyncKeyState(vKey) & 0x8000) != 0; // getkeystate returns 16 bit(short) 0x8000 tells u if key is pressed - returns true if != 0
            bool wasDown = _downKeys.Contains(vKey); // checks if key was already down
            if (isDown) 
            {
                if (!wasDown) // if wasnt already pressed return true
                {
                    _downKeys.Add(vKey);
                    return true;
                }
            }
            else // if key was already pressed return false
            {
                _downKeys.Remove(vKey);
            }
            return false;
        }
        // Check special keys
        public static bool IsKeyPressed(Key key)
        {
            bool isDown = (GetAsyncKeyState((int) key) & 0x8000) != 0; // getkeystate returns 16 bit(short) 0x8000 tells u if key is pressed - returns true if != 0
            bool wasDown = _downKeys.Contains((int)key); // checks if key was already down
            if (isDown)
            {
                if (!wasDown) // if wasnt already pressed return true
                {
                    _downKeys.Add((int)key);
                    return true;
                }
            }
            else // if key was already pressed return false
            {
                _downKeys.Remove((int)key);
            }
            return false;
        }
        public static void KeyInputLoop()
        {
            CameraMovementKeys(); // check camera movement keys
        }
        public static void CameraMovementKeys()
        {
            if (IsKeyDown('W'))
            {
                float speed = 10f;
                Vector2 movepos;
                movepos.Y = -1;
                Camera.Position.Y = Camera.Position.Y + movepos.Y * speed;
            }
            if (IsKeyDown('D'))
            {
                float speed = 10f;
                Vector2 movepos;
                movepos.X = 1;

                Camera.Position.X = Camera.Position.X + movepos.X * speed;
            }
            if (IsKeyDown('A'))
            {
                float speed = 10f;
                Vector2 movepos;
                movepos.X = -1;

                Camera.Position.X = Camera.Position.X + movepos.X * speed;
            }
            if (IsKeyDown('S'))
            {
                float speed = 10f;
                Vector2 movepos;
                movepos.Y = 1;

                Camera.Position.Y = Camera.Position.Y + movepos.Y * speed;
            }
        }
    }
}
