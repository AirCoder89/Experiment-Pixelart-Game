using AirCoder.Controllers.NPC.Components;
using UnityEngine;
using Application = AirCoder.Core.Application;
using Vector2Int = AirCoder.Core.Vector2Int;

namespace AirCoder.Controllers.AI.Components.FSM.EnemyAI.Actions
{
    public class PatrolAction : Action
    {
        public bool PatrolCompleted { get; private set; }

        private readonly Vector2Int _range;
        private float _idleDuration;
        private float _counter;
        private Vector2 _patrolRangeTime;
        public PatrolAction(NpcController inNpcController, Vector2Int inPatrolRange, Vector2 inPatrolDuration) : base(inNpcController)
        {
            _patrolRangeTime = inPatrolDuration;
            _range = inPatrolRange;
            PatrolCompleted = false;
        }

        public override void OnEnter()
        {
            _idleDuration = Random.Range(_patrolRangeTime.x, _patrolRangeTime.y); //pick random duration
            _counter = 0f;
            PatrolCompleted = false;
        }

        public override void OnTick()
        {
            npcController.DoMoveTo(_range); //translate position & set animation
            _counter += Application.DeltaTime;
            if (_counter >= _idleDuration)
            {
                PatrolCompleted = true;
            }
        }

        public override void OnExit()
        {
            PatrolCompleted = false;
            _counter = 0f;
        }
    }
}