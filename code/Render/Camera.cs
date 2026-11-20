using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Render
{
    public class Camera
    {
        public static Vector2 Position = new Vector2(400, 300);
        public static float ScreenWidth = 800;
        public static float ScreenHeight = 600;
        public static float Zoom = 1.0f;

        //converts screen pxiel cords to world cords
        //usefull for clicking objects
        public static Vector2 ScreenToWorld(Vector2 screenpoint, Vector2 viewportSize)
        {
            Vector2 centered = screenpoint - (viewportSize / 2f);
            Vector2 unscaled = centered / Zoom;
            return unscaled + Position;
        }
        //converts world cords into screen pixel cords
        public static Vector2 WorldToScreen(Vector2 worldPoint, Vector2 viewportSize)
        {
            Vector2 relative = worldPoint - Position;
            Vector2 scaled = relative * Zoom;
            return scaled + (viewportSize / 2f);
        }
    }
}
