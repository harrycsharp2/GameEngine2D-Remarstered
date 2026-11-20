using GameEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components.RigidBody
{
    public class RigidBody
    {
        public GameObject? AttachtedObject;
        public Vector2? Position;
        public Vector2? Size;
        public float GravityFallSpeed = 10f;
        public bool EnableGravity = false;
        //create/attach rigidbody
        public void AttachRigidBody(GameObject gameobject)
        {
            AttachtedObject = gameobject;
            gameobject.Attachted_rb = this;
            gameobject.AllAttachtedComponents.Add("Rectangle");
            Position = gameobject.Position;
            Size = gameobject.Size; 
        }
        public void RigidBodyMove(Vector2 moveposition, float speed)
        {
            if (AttachtedObject != null)
            {
                AttachtedObject.Position.X = AttachtedObject.Position.X + moveposition.X * speed;
                AttachtedObject.Position.Y = AttachtedObject.Position.Y + moveposition.Y * speed;
            }
        }
        public void GravityLogic()
        {

        }
        //render rigidbody loop
        public void RigidBodyLoop()
        {
            if (EnableGravity)
            {
                GravityLogic();
            }
        }
    }
}
