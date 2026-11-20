//NOTES
//USING NUTPACKAGES:
//Silk.NET.OpenGL
//Silk.NET.Windowing 
//Silk.NET.input
//SkiaSharp
///Microsoft.CodeAnalysis.CSharp
using GameEngine.Render;
using GameEngine.GameObjects;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Input;
using System;
using System.IO;
using System.Numerics;
using GameEngine.KeyBinds;
using GameEngine.HandleConsole;
namespace Program
{
    public class Program
    {
        // WINDOW SETTINGS
        public static Vector2D<int> WindowDimensions = new Vector2D<int>(800, 600);
        public static string WindowTitle = "Game Engine Remarsterd";
        public static IMouse? Mouse;
        private static bool wasMouseDown = false;
        /// <summary>
        /// TO DO
        /// ABLE TO EDIT RIGIDBODY
        /// COLLIDER
        /// No Position Reset on stop - reset game
        /// IMAGES
        /// AUDIO - add method PlayAudio.MP3(filepath to mp3)
        /// </summary>
        public static void Main()
        {
            LoadProgram();
        }
        public static void LoadProgram()
        {
            //settings
            var settings = WindowOptions.Default;
            settings.Size = WindowDimensions;
            settings.Title = WindowTitle;
            
            //window instance
            IWindow window = Window.Create(settings);

            //load renderer 
            window.Load += () =>
            {
                Renderer.Init(window);

                //grab mouse
                IInputContext input = window.CreateInput();
                Mouse = input.Mice[0];
            };

            //loops
            window.Render += RenderLoop;
            window.Update += RunChecks;

            //start script loops
            HandleConsole.Start();

            //launch window
            window.Run();
        }
        private static void RenderLoop(double deltaTime)
        {
            Renderer.RenderLoop();
            HandleKeyInput.KeyInputLoop();
        }
        private static void RunChecks(double deltaTime)
        {
            bool isMouseDown = Mouse != null && Mouse.IsButtonPressed(MouseButton.Left);
            //check if mouse is clicking object
            if (isMouseDown&& !wasMouseDown)
            {
                foreach (var obj in GameObject.AllObjects)
                {
                    bool isclicked = obj.CheckClick();  //check if gameobject is clicked
                    if (isclicked == true) //if it is display info
                    {
                        Console.WriteLine($"\nObject clicked, \n" +
                            $"Name: {obj.Name}  :::  Type: {obj.Type}\n" +
                            $"Position: X:{obj.Position.X} Y:{obj.Position.Y}   :::  Colour: {obj.Colour}\n" +
                            $"Width: {obj.Size.X} Height: {obj.Size.Y}\n");
                        Console.WriteLine("Components");
                        foreach (var component in obj.AllAttachtedComponents)
                        {
                            Console.WriteLine(component);
                        }
                        HandleConsole.ClickedGameObject = obj;
                    }
                }
            }
            wasMouseDown = isMouseDown;
        }
    }
}