using System;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AirCoder.Controllers.GamePhysics.Components
{
  
    public enum ColliderEvent
    {
        OnHitEnter, OnHitStay, OnHitExit
    }
    
    public abstract class GameCollider : GameComponent
    {
        private event Action<GameObject> _onHitEnter; 
        private event Action<GameObject> _onHitStay; 
        private event Action<GameObject> _onHitExit; 
        public GameObject TargetGo { get; private set; }
        public Rigidbody2D RigidBody { get; private set; }
        public Collider2D Collider { get; protected set; }

        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if(Collider == null) return;
                var box = Collider as BoxCollider2D;
                if (box != null) box.enabled = _enabled = value;
            }
        }
        
        protected bool isRun;

        protected bool isCollide;
        protected GameObject lastCollision;
        protected Collider2D[] colliderBuffer;
        
        public GameCollider(GameView inTarget, float inMass, bool inIsKinematic) : base(inTarget)
        {
            TargetGo = inTarget.gameObject;

            // --------------------------------------------------------------------------------
            // Add & Setup RigidBody2d
            // --------------------------------------------------------------------------------
            RigidBody = TargetGo.AddComponent<Rigidbody2D>();
            RigidBody.isKinematic = inIsKinematic;
            RigidBody.mass = inMass;
            RigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            RigidBody.gravityScale = SystemFacade.Parameters.gravityScale;
            RigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            SystemFacade.Physics.AddComponent(this); //subscribe the collider
            
            // --------------------------------------------------------------------------------
            // Initialize private variables
            // --------------------------------------------------------------------------------
            lastCollision = null;
            isCollide = false;
            colliderBuffer = new Collider2D[5];
            isRun = true;
        }
        
        public override void Destroy()
        {
            Object.Destroy(RigidBody);
            Object.Destroy(Collider);
            SystemFacade.Physics.RemoveComponent(this); //unsubscribe the collider
            
            //- release references
            base.Destroy();
            TargetGo = null;
            RigidBody = null;
            Collider = null;
            lastCollision = null;
            colliderBuffer = null;
        }
        
        public void AddEventListener(ColliderEvent inEventType, Action<GameObject> inCallback)
        {
            switch (inEventType)
            {
                case ColliderEvent.OnHitEnter : _onHitEnter = inCallback; break;
                case ColliderEvent.OnHitStay : _onHitStay = inCallback; break;
                case ColliderEvent.OnHitExit : _onHitExit = inCallback; break;
            }
        }

        public void RemoveEventListener(ColliderEvent inEventType)
        {
            switch (inEventType)
            {
                case ColliderEvent.OnHitEnter : _onHitEnter = null; break;
                case ColliderEvent.OnHitStay : _onHitStay = null; break;
                case ColliderEvent.OnHitExit : _onHitExit = null; break;
            }
        }
        
        public void RemoveAllEventsListener()
        {
            _onHitEnter = null;
            _onHitStay = null;
            _onHitExit = null;
        }

        public override void Pause()
        {
            isRun = false;
        }

        public override void Resume()
        {
            isRun = true;
        }

        protected void FireHitEnter(GameObject t) => _onHitEnter?.Invoke(t);
        protected void FireHitStay(GameObject t) => _onHitStay?.Invoke(t);
        protected void FireHitExit(GameObject t) => _onHitExit?.Invoke(t);
    }
}