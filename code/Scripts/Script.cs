using GameEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Scripts
{
    public class Script
    {
        public GameObject GameObjects { get; set; } = null!;
        public virtual void Start() { }
        public virtual void Update() { }
    }
}
