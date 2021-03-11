using AirCoder.Controllers.AI.Components.FSM.EnemyAI.Actions;

namespace AirCoder.Controllers.AI.Components.FSM.EnemyAI.Decisions
{
    public class IdleDecision : Decision
    {
        private readonly PatrolAction _targetAction;
        public IdleDecision(PatrolAction inPatrolAction)
        {
            _targetAction = inPatrolAction;
        }
        public override bool Decide()
        {
            return _targetAction != null && _targetAction.PatrolCompleted;
        }
    }
}