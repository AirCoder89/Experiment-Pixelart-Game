using AirCoder.Core;
using AirCoder.Views;
using UnityEngine;

namespace AirCoder.Controllers.GamePhysics.Components
{
    public class HitBox : GameCollider
    {
        private Vector2 _offset;
        public Vector2 offset
        {
            get { return _offset; }
            set
            {
                if(Collider == null) return;
                var box = Collider as BoxCollider2D;
                if (box != null) box.offset = _offset = value;
            }
        }
        
        private Vector2 _size;
        public Vector2 size
        {
            get { return _size; }
            set
            {
                if(Collider == null) return;
                var box = Collider as BoxCollider2D;
                if (box != null) box.size = _size = value;
            }
        }

        public HitBox(GameView inTarget, Vector2 inSize, float inMass,bool inIsKinematic = false, bool inIsTrigger = false) : base(inTarget, inMass, inIsKinematic)
        {
            var box = TargetGo.AddComponent<BoxCollider2D>();
            box.isTrigger = inIsTrigger;
            Collider = box;
            size = inSize;
        }

        public override void Tick() //Fixed Tick
        {
            base.Tick();
            if(!isRun) return;
            var nbCollider = Physics2D.OverlapBoxNonAlloc(TargetGo.transform.localPosition, size, 0f, colliderBuffer);
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
            else
            {
                lastCollision = null;
            }
        }

    }
}