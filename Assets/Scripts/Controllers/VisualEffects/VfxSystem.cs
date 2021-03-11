using System;
using System.Collections.Generic;
using System.Linq;
using AirCoder.Controllers.Sounds;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.VisualEffects
{
    public class VfxSystem : GameSystem
    {
        public List<IVfx> VfxList { get;}
        private readonly VfxSystemConfig _config;
        
        public VfxSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
            _config = inConfig as VfxSystemConfig;
            if(_config == null) throw new Exception($"System config must be not null.");
            
            VfxList = new List<IVfx>();
            ParticleVfx.OnComplete += OnVfxComplete;
            AnimationVfx.OnComplete += OnVfxComplete;
        }
        
        private void OnVfxComplete(IVfx inVfx)
        {
            //- vfx complete -> dispawn it
            VfxList.Remove(inVfx);
            inVfx.Remove();
        }

        public override void Tick()
        {
            base.Tick();
            foreach (var vfx in VfxList.ToList())
                vfx.Tick();
        }

        public void PlayVfx(VfxList inLabel, Vector2 inPosition)
        {
            var map = GetVfxMap(inLabel);
            if (map == null) throw new Exception($"Vfx {inLabel} not found!");

            IVfx gameVfx = null;
            switch (map.type)
            {
                case VfxType.Animation:
                    gameVfx = GetAnimationVfx(map);
                    break;
                case VfxType.ParticleSystem:
                    gameVfx = GetParticleVfx(map);
                    break;
            }
            
            if(gameVfx == null)  throw new Exception($"Vfx Is null !");
            
            VfxList.Add(gameVfx);
            
            //- add vfx to level view
            SystemFacade.Level.Level.AddVfx(gameVfx, new Vector2(inPosition.x, inPosition.y));

            //- play sound effect
            if (map.sfx != SoundList.None)
                SystemFacade.Sounds.PlaySfx(map.sfx);
            
            gameVfx?.Play();
        }

        private ParticleVfx GetParticleVfx(VfxMap inMap)
        {
            var particle = SystemFacade.Pool.GetConcreteGameView<ParticleVfx>();
            var go = SystemFacade.Pool.Spawn(inMap.vfxName); 
            
            if (particle == null)
            {
                //- create new particle
                particle = new ParticleVfx(this.application, go);
                SystemFacade.Pool.StoreGameView(particle);
            }
            else
            {
                //- use old one from the pool
                particle.SetGameObject(go);
            }
            return particle;
        }
        
        private AnimationVfx GetAnimationVfx(VfxMap inMap)
        {
            var anim = SystemFacade.Pool.GetConcreteGameView<AnimationVfx>();
            var go = SystemFacade.Pool.Spawn(inMap.vfxName);

            if (anim == null)
            {
                //- create new animation
                anim = new AnimationVfx(this.application, inMap.vfxName, LayerNames.Vfx);
                SystemFacade.Pool.AddItem(anim.gameObject.transform, inMap.vfxName, false, null);
                SystemFacade.Pool.StoreGameView(anim);
            }
            else
            {
                //- use old one from the pool
                anim.SetGameObject(go);
                anim.UpdateSpriteRendererInstance();
            }
            
            anim.Animator.SetAnimationData(inMap.animationData);
            return anim;
        }

        private VfxMap GetVfxMap(VfxList inLabel) => _config.vfxMap.FirstOrDefault(v => v.label == inLabel);
    }
}