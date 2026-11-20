using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class EngineState
    {
        public static bool IsPlaying = false;
        public static void PlayEngine()
        {
            IsPlaying = true;
        }
    }
}
