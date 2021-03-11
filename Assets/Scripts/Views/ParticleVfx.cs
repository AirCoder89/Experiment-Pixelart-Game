using System;
using AirCoder.Controllers.VisualEffects;
using AirCoder.Core;
using AirCoder.Helper;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Views
{
    public class ParticleVfx : GameView, IVfx
    {
        public static event Action<IVfx> OnComplete; 
        
        private ParticleSystem _ps;

        public ParticleSystem particleSystem
        {
            get
            {
                if(_ps == null) _ps = gameObject.GetComponent<ParticleSystem>();
                return _ps;
            }
        }

        public ParticleVfx(Application inApp, GameObject inGameObject) : base(inApp, inGameObject)
        {
        }

        public void Play()
        {
            particleSystem.Play();
        }
        
        public override void Tick()
        {
            if (particleSystem == null || particleSystem.IsAlive()) return;
                OnComplete?.Invoke(this);
        }

        public void Remove()
        {
            particleSystem.Stop();
            SystemFacade.Pool.Despawn(this.gameObject.transform);
            SystemFacade.Pool.StoreGameView(this);
        }

        public Transform GetTransform()
        {
            return gameObject.transform;
        }
    }
}