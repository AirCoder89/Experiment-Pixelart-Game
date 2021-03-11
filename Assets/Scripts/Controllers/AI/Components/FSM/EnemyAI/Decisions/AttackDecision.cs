using AirCoder.Controllers.NPC.Components;

namespace AirCoder.Controllers.AI.Components.FSM.EnemyAI.Decisions
{
    public class AttackDecision : Decision
    {
        private readonly NpcController _npcController;
        public AttackDecision(NpcController inNpcController)
        {
            _npcController = inNpcController;
        }
        
        public override bool Decide()
        {
            return _npcController.hasEnemy;
        }
    }
}