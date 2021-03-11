using AirCoder.Controllers.NPC.Components;
using AirCoder.Core;

namespace AirCoder.Controllers.AI.Components.FSM.EnemyAI.Actions
{
    public class DieAction : Action
    {
        public DieAction(NpcController inNpcController) : base(inNpcController)
        {
        }

        public override void OnEnter()
        {
           
        }

        public override void OnTick()
        {
            var dieSide = npcController.enemyView.LastHitDirection == npcController.GetDirection()
                ? ActorActions.DieFront
                : ActorActions.DieBack;
            npcController.DoDie(dieSide);
        }

        public override void OnExit()
        {
            
        }
    }
}