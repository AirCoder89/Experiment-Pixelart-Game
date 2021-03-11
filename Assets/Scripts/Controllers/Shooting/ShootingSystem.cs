using System;
using System.Collections.Generic;
using System.Linq;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Shooting
{
    public class ShootingSystem : GameSystem
    {
        private readonly ShootingSystemConfig _config;
        
        public ShootingSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
            _config = inConfig as ShootingSystemConfig;
            if(_config == null) throw new Exception($"System config must be not null.");
        }

        public void Shoot(BulletsList inBulletLabel, GameObject inOwner, Vector2 inPosition, Vector2 inDirection)
        {
            // --------------------------------------------------------------------------------
            // Bring & Setup Bullet view
            // --------------------------------------------------------------------------------
            var bullet = GetBullet(inBulletLabel);
            bullet.Visibility = true;
            bullet.Launch(inOwner, inPosition, inDirection);
        }
        
        private BulletView GetBullet(BulletsList inBulletLabel)
        {
            var bulletData = _config.bulletsData?.FirstOrDefault(b => b.label == inBulletLabel);
            if(bulletData == null) throw new NullReferenceException($"bullet data {inBulletLabel} not found!");
            
            var bullet = SystemFacade.Pool.GetConcreteGameView<BulletView>();
            if (bullet != null)
            {
                bullet.Visibility = false;
                return bullet;
            }
            
            //- create new bullet
            bullet = new BulletView(application, "Bullet", LayerNames.Vfx, bulletData) { Visibility = false };
                return bullet;
        }

        public void Dispose(BulletView inBullet)
        {
            inBullet.Visibility = false;
            SystemFacade.Pool.StoreGameView(inBullet);
        }
        
        public override void Tick()
        {
            if(!IsRun) return;
            foreach (var component in _components.ToList())
                component ?.Tick();
        }
    }
}