using Silk.NET.Maths;
using Silk.NET.Windowing;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.GameObjects;
using System.Security.Cryptography.X509Certificates;
namespace GameEngine.Render
{
    public class Renderer
    {
        private static GRBackendRenderTarget? _renderTarget;
        private static GRContext _grContext = null!;
        private static SKSurface _surface = null!;
        private static readonly SKPaint _paint = new SKPaint { IsAntialias = true };
        public static void Init(IWindow window)
        {
            var gl = GRGlInterface.Create(name =>
            window.GLContext!.TryGetProcAddress(name, out var addr) ? addr : IntPtr.Zero);
            _grContext = GRContext.CreateGl(gl);
            
            //creating surface
            CreateSurface(window);
            window.FramebufferResize += _ => CreateSurface(window);
        }
        public static void CreateSurface(IWindow window)
        {
            _surface?.Dispose();
            _renderTarget?.Dispose();
            
            // 0 = default OpenGL framebuffer 0x8058 = GL_RGBA8
            _renderTarget = new GRBackendRenderTarget(
                window.FramebufferSize.X,
                window.FramebufferSize.Y,
                0, // sample count
                8, // stencil bits
                new GRGlFramebufferInfo(0, 0x8058)
            );
            _surface = SKSurface.Create(_grContext, _renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
        }
        public static void RenderLoop()
        {
            //clear window
            var canvas = _surface.Canvas;
            canvas.Clear(SKColors.Black);
            
            //start camera
            canvas.Save();
            
            //move 0,0 to center of the screen
            float halfwidth = _renderTarget!.Width / 2f;
            float halfheight = _renderTarget!.Height / 2f;
            canvas.Translate(halfwidth, halfheight);
            
            //apply camera zoom
            canvas.Scale(Camera.Zoom, Camera.Zoom);
            //shift the world oppiste the camera (camera moves forwards // world moves backwards)
            canvas.Translate(-Camera.Position.X, -Camera.Position.Y);

            //draw objects
            foreach (var obj in GameObject.AllObjects.ToList())
            {
                if (obj.Type == "Rectangle")
                {
                    //draw square
                    DrawRect(obj.Position.X, obj.Position.Y, obj.Size.X, obj.Size.Y, obj.Colour);

                    
                }
                else if (string.IsNullOrWhiteSpace(obj.Type))
                {
                    //if obj doesnt have a type then remove it from the render
                    GameObject.AllObjects.Remove(obj);
                }
            }

            //Draws everything then if the user is playing it runs everything else like scripts etc
            //this is so the user can only see the game at its start. So when the user press plays everything starts
            //the user should just be able to see every gameobject etc until they type play/press play.
            if (EngineState.IsPlaying)
            {
                PlayLoop();
            }

            //end camera
            canvas.Restore();

            //present stage
            _surface.Canvas.Flush();
            _grContext.Flush();
        }
        public static void DrawRect(float x, float y, float width, float height, SKColor colour)
        {
            _paint.Color = colour;
            _surface.Canvas.DrawRect(x, y, width, height, _paint);
        }
        public static void PlayLoop()
        {
            foreach (var obj in GameObject.AllObjects)
            {
                if (obj.Attachted_rb != null)
                {
                    obj.Attachted_rb.RigidBodyLoop();
                }
                foreach(var script in obj.AttachtedScripts)
                {
                    script.Update();
                }
            }
        }
    }
}
