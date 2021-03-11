using AirCoder.Controllers.NPC.Components;

namespace AirCoder.Controllers.AI.Components.FSM.EnemyAI.Decisions
{
    public class AttackToPatrolDecision : Decision
    {
        private readonly NpcController _npcController;
        public AttackToPatrolDecision(NpcController inNpcController)
        {
            _npcController = inNpcController;
        }
        
        public override bool Decide()
        {
            return !_npcController.hasEnemy;
        }
    }
}