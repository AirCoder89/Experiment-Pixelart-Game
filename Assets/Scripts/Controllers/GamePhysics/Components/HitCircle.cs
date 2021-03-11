using AirCoder.Core;
using AirCoder.Views;
using UnityEngine;

namespace AirCoder.Controllers.GamePhysics.Components
{
    public class HitCircle : GameCollider
    {
        private float _radius;
        public float Radius
        {
            get { return _radius; }
            set
            {
                if(Collider == null) return;
                var circle = Collider as CircleCollider2D;
                if (circle != null) circle.radius = _radius = value;
            }
        }

        public HitCircle(GameView inTarget, float inRadius, float inMass,bool inIsKinematic = false,  bool inIsTrigger = false) : base(inTarget, inMass,inIsKinematic)
        {
            var circle = TargetGo.AddComponent<CircleCollider2D>();
            circle.isTrigger = inIsTrigger;
            Collider = circle;
            Radius = inRadius;
        }

        public override void Tick() //Fixed Tick
        {
            base.Tick();
            if(!isRun) return;
            var nbCollider = Physics2D.OverlapCircleNonAlloc(TargetGo.transform.localPosition, Radius, colliderBuffer);
            isCollide = nbCollider > 1;
            
            if (isCollide)
            {
                if (lastCollision == null)
                {
                    //- Hit Enter
                    lastCollision = colliderBuffer[1].gameObject;
                    FireHitEnter(lastCollision);
                    return;
                }
                //- Hit Stay
                FireHitStay(colliderBuffer[1].gameObject);
            }
            else if (lastCollision != null)
            {
                //- Hit Exit
                FireHitExit(lastCollision);
                lastCollision = null;
            }
            else lastCollision = null;
        }

    }
}