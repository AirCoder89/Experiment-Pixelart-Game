using System;
using AirCoder.Controllers.AI.Components.FSM.EnemyAI.Actions;
using AirCoder.Helper;
using UnityEngine;

namespace AirCoder.Controllers.AI.Components.FSM.EnemyAI.Decisions
{
    public class IdleToPatrolDecision : Decision
    {
        private IdleAction _idleAction;

        public IdleToPatrolDecision(IdleAction inIdle)
        {
            _idleAction = inIdle ?? throw new Exception($"IdleAction must be not null");
        }
        
        public override bool Decide()
        {
            return _idleAction != null && _idleAction.IdleCompleted;
        }
    }
}