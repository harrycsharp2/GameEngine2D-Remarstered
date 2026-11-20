using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.GameObjects;
using System.ComponentModel.Design;
using Silk.NET.Core;
using GameEngine.Components.RigidBody;

namespace GameEngine.HandleConsole
{
    public class HandleConsole
    {

        public static GameObject? ClickedGameObject;
        public static void Start()
        {
            //starts console loop in background thread
            Task.Run(ConsoleLoop);
        }
        public static void ConsoleLoop()
        {
            while (true)
            {
                CommandMenu();
            }
        }
        public static void CommandMenu()
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return;

            //spawn object
            if (input.Equals("spawn gameobject", StringComparison.OrdinalIgnoreCase))
            {

                GameObject obj = new GameObject();
                obj.CreateObject();
            }
            //play
            else if (input.Equals("play", StringComparison.OrdinalIgnoreCase))
            {
                EngineState.PlayEngine();
            }
            //stop
            else if (input.Equals("stop", StringComparison.OrdinalIgnoreCase))
            {
                EngineState.IsPlaying = false;
            }
            //attach script 2 gameobject
            else if (input.StartsWith("+script", StringComparison.OrdinalIgnoreCase))
            {
                var parts = input.Split(' ');
                if (ClickedGameObject != null)
                {
                    string filepath = parts[1];
                    ClickedGameObject.AttachScript(filepath);
                }
            }
            //edit gameobject
            else if (input.StartsWith("set", StringComparison.OrdinalIgnoreCase))
            {
                if (ClickedGameObject == null)
                {
                    Console.WriteLine("no object selected - click a object");
                    return;
                }
                //set width 100
                //set positionx 200
                //splits string into parts
                var parts = input.Split(' ');
                if (parts.Length == 3)
                {
                    string prop = parts[1];
                    string val = parts[2];
                    
                    if (prop == "width" && float.TryParse(val, out float w)) ClickedGameObject.Size.X = w;
                    else if (prop == "height" && float.TryParse(val, out float h)) ClickedGameObject.Size.Y = h;
                    else if (prop == "x" && float.TryParse(val, out float x)) ClickedGameObject.Position.X = x;
                    else if (prop == "y" && float.TryParse(val, out float y)) ClickedGameObject.Position.Y = y;
                    else if (prop == "name") ClickedGameObject.Name = val;
                    else
                    {
                        Console.WriteLine("Format: set width 100, set x 100, set name billy");
                    }

                    Console.WriteLine($"Updated {prop} to {val} on {ClickedGameObject.Name}");
                }
            }
            //add component to gameobject
            if (input.StartsWith("attach", StringComparison.OrdinalIgnoreCase))
            {
                string[] allcomponents = { "rigidbody" };
                var parts = input.Split(' ');
                if (parts.Length == 2 && 
                    ClickedGameObject != null)
                {
                    string component = parts[1];
                    if (allcomponents.Contains(component))
                    {
                        RigidBody rb = new RigidBody();
                        rb.AttachRigidBody(ClickedGameObject);
                        Console.WriteLine($"added rigidbody too {ClickedGameObject.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("something went wrong - format: attach 'component'  // you must click the gameobject u choose first");
                }
            }
            //clear
            else if (input.Equals("clear", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
            }
            //exit
            else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Environment.Exit(0);
            }
            //view all objects
            else if (input.Equals("view all gameobjects", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var obj in GameObject.AllObjects)
                {
                    Console.WriteLine($"Object Name: {obj.Name}, Object X {obj.Position.X}, Object Y: {obj.Position.Y}, Object Width: {obj.Size.X}, Object Height: {obj.Size.Y}");
                }
            }
            //help
            else if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
                Console.WriteLine("FEATURES");
                Console.WriteLine("Click gameobject - view/edit details");
                Console.WriteLine("COMMANDS");
                Console.WriteLine("1. spawn gameobject");
                Console.WriteLine("2 'view all gameobjects");
                Console.WriteLine("3. set 'property' 'value' - edit gameobjects (must click gameobject first)");
                Console.WriteLine("4. +script 'filepath' - attach script to gameobject (must click gameobject first)");
                Console.WriteLine("5. play - start game");
                Console.WriteLine("6. stop - stop game");
                Console.WriteLine("'clear'");
                Console.WriteLine("'exit'");
                Console.WriteLine("'help'");
            }
            //unknown command
            else
            {
                Console.WriteLine("unkown command");
            }
        }
    }
}
