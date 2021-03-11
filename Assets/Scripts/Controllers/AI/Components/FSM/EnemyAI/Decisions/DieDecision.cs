using System;
using AirCoder.Controllers.Shooting;

namespace AirCoder.Controllers.AI.Components.FSM.EnemyAI.Decisions
{
    public class DieDecision : Decision
    {
        private readonly IDestructible _destructible;
        
        public DieDecision(IDestructible inDestructible)
        {
            _destructible = inDestructible ?? throw new Exception($"IDestructible must be not null");
        }
        
        public override bool Decide()
        {
            return !_destructible.IsAlive;
        }
    }
}