using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SkiaSharp;
using Silk.NET.Input;
using Program;
using GameEngine.Render;
using GameEngine.Components.RigidBody;
using GameEngine.Scripts;
namespace GameEngine.GameObjects
{
    public class GameObject
    {
        public static List<GameObject> AllObjects = new List<GameObject>();
        public List<string> AllAttachtedComponents = new List<string>();
        public List<Script> AttachtedScripts = new List<Script>();
        public static string[] AllTypes = { "Rectangle" };
        public RigidBody? Attachted_rb;
        public SKColor Colour = SKColors.White;
        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Size = new Vector2(50, 50);
        public string Name = "Object";
        public string Type = "Rectangle";
        public void CreateObject()
        {
            AllObjects.Add(this);
            Console.WriteLine("Object Created");
        }
        public void AttachScript(string filepath)
        {
            Type? scriptype = Compiler.CompileScript(filepath);
            if (scriptype == null)
            {
                Console.WriteLine("failed");
                return;
            }
            Script script = (Script)Activator.CreateInstance(scriptype)!;
            script.GameObjects = this;
            AttachtedScripts.Add(script);
            
            script.Start();
            Console.WriteLine("attachted");
        }
        public bool CheckClick()
        {
            //get mouse 
            IMouse? mouse = Program.Program.Mouse;
            if (mouse == null) return false;
            if (!mouse.IsButtonPressed(MouseButton.Left)) return false;

            //Mouse to world
            Vector2 _WindowDimesions = new Vector2(800, 600);
            Vector2 mouseScreen = mouse.Position;
            Vector2 mouseWorld = Camera.ScreenToWorld(mouseScreen, _WindowDimesions);
            //check if mouse is ontop off gameoject return as bool
            return mouseWorld.X >= Position.X &&
                   mouseWorld.X <= Position.X + Size.X &&
                   mouseWorld.Y >= Position.Y &&
                   mouseWorld.Y <= Position.Y + Size.Y;
        }
    }
}
