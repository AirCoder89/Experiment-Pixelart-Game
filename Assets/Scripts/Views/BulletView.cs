using System;
using AirCoder.Controllers.GamePhysics.Components;
using AirCoder.Controllers.Shooting;
using AirCoder.Controllers.Shooting.Components;
using AirCoder.Helper;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Views
{
    public class BulletView : SpriteView
    {
        private HitCircle _circle;
        protected HitCircle hitCircle
        {
            get
            {
                if (_circle == null) GetComponent<HitCircle>();
                return _circle;
            }
        }
        
        
        private GameBullet _bullet;
        protected GameBullet bullet
        {
            get
            {
                if (_bullet == null) GetComponent<GameBullet>();
                return _bullet;
            }
        }

        private BulletData _bulletData;
        private GameObject _owner;
        private Vector2 _direction;
        
        public BulletView(Application inApp, string inName, string inLayerName, BulletData inData) : base(inApp, inName, inLayerName)
        {
            _bulletData = inData;
            
            // --------------------------------------------------------------------------------
            // Setup Bullet Collider
            // --------------------------------------------------------------------------------
            _circle = AddComponent(new HitCircle(this, _bulletData.radius, 1f, true, true));
            hitCircle.AddEventListener(ColliderEvent.OnHitEnter, OnHitObject);
            
            // --------------------------------------------------------------------------------
            // Assign Bullet Sprite
            // --------------------------------------------------------------------------------
            SetSprite(_bulletData.sprite);
            
            // --------------------------------------------------------------------------------
            // Setup Bullet Component
            // --------------------------------------------------------------------------------
            _bullet = AddComponent(new GameBullet(this));
        }

        private void OnHitObject(GameObject inObject)
        {
            if(inObject == _owner) return;
            
            var objectView = ObjectMap.GetGameView(inObject);
            var destructible = objectView as IDestructible;
            if (destructible != null && destructible.IsAlive)
            {
                //- Hit Destructible
                SystemFacade.VFX.PlayVfx(_bulletData.vfx, inObject.transform.localPosition + new Vector3(0,20,0));
                destructible.TakeDamage(_bulletData.damage, _direction);
            }
            else
            {
                //- Hit Indestructible
                SystemFacade.VFX.PlayVfx(_bulletData.vfx, inObject.transform.localPosition );
            }
            bullet.Pause();
            SystemFacade.Shooting.Dispose(this);
        }

        public void Launch(GameObject inOwner, Vector2 inPosition, Vector2 inDirection)
        {
            if(inOwner == null) throw new NullReferenceException($"Bullet owner must be not null !");
            _direction = inDirection;
            _owner = inOwner;
            bullet.Launch(inPosition, inDirection, _bulletData.speed);

        }
    }
}