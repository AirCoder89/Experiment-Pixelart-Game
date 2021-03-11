using System.Collections.Generic;
using System.Linq;
using AirCoder.Controllers.GamePhysics.Components;
using AirCoder.Core;

namespace AirCoder.Controllers.GamePhysics
{
    public class PhysicsSystem : GameSystem, IFixedTick
    {
        public PhysicsSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
        }
        
        public void FixedTick()
        {
            if((!IsRun)) return;
            foreach (var gameCollider in _components.ToList())
                gameCollider.Tick();
        }

        public override void PauseSystem()
        {
            base.PauseSystem();
            foreach (var gameCollider in _components.ToList())
                gameCollider.Pause();
        }

        public override void ResumeSystem()
        {
            base.ResumeSystem();
            foreach (var gameCollider in _components.ToList())
                gameCollider.Resume();
        }

    }
}