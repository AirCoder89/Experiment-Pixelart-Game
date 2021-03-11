using AirCoder.Controllers.NPC.Components;
using UnityEngine;

namespace AirCoder.Controllers.AI.Components.FSM
{
    public abstract class Action
    {
        protected NpcController npcController;
        
        public Action(NpcController inNpcController)
        {
            npcController = inNpcController;
        }
        public abstract void OnEnter ();
        public abstract void OnTick ();
        public abstract void OnExit ();
    }
}