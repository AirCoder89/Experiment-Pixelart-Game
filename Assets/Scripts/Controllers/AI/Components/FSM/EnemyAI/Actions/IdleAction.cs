using AirCoder.Controllers.NPC.Components;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.AI.Components.FSM.EnemyAI.Actions
{
    public class IdleAction : Action
    {
        public bool IdleCompleted { get; private set; }
    
        private float _idleDuration;
        private float _counter;
        private readonly Vector2 _idleRangeTime;
        public IdleAction(NpcController inNpcController, Vector2 inIdleDuration) : base(inNpcController)
        {
            IdleCompleted = false;
            _idleRangeTime = inIdleDuration;
        }
        
        public override void OnEnter()
        {
            _idleDuration = Random.Range(_idleRangeTime.x, _idleRangeTime.y); //pick random duration
            IdleCompleted = false;
            _counter = 0f;
        }

        public override void OnTick()
        {
            npcController.DoIdle(); //set animation
            _counter += Application.DeltaTime;
            if (_counter >= _idleDuration)
            {
                IdleCompleted = true;
            }
        }

        public override void OnExit()
        {
            IdleCompleted = false;
            _counter = 0f;
        }

    }
}