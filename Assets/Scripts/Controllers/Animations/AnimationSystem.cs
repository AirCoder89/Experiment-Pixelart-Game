using System.Collections.Generic;
using System.Linq;
using AirCoder.Controllers.Animations.Components;
using AirCoder.Core;

namespace AirCoder.Controllers.Animations
{
    /// <summary>
    /// Custom animation system to manage & Tick GameAnimator component.
    /// </summary>
    public class AnimationSystem : GameSystem
    {
        public AnimationSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
        }
        
        public override void PauseSystem()
        {
            base.PauseSystem();
            foreach (var animator in _components.ToList())
                animator.Pause();
        }

        public override void ResumeSystem()
        {
            base.ResumeSystem();
            foreach (var animator in _components.ToList())
                animator.Resume();
        }

        public override void Tick()
        {
            base.Tick();
            foreach (var animator in _components.ToList())
                animator.Tick();
        }
    }
}