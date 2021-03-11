using AirCoder.Controllers.NPC.Components;
using AirCoder.Helper;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.AI.Components.FSM.EnemyAI.Actions
{
    public class AttackAction : Action
    {
        private readonly float _fireRate;
        private float _counter;
        private bool _canShoot;
        
        public AttackAction(NpcController inNpcController, float inFireRate) : base(inNpcController)
        {
            _fireRate = inFireRate;
        }

        public override void OnEnter()
        {
            _counter = 0f;
            _canShoot = true;
        }

        public override void OnTick()
        {
            if (_canShoot)
            {
                _canShoot = false;
                npcController.DoShoot();
                return;
            }
            
            _counter += Application.DeltaTime;
            if (!(_counter >= _fireRate)) return;
            _counter = 0f;
            _canShoot = true;
        }

        public override void OnExit()
        {
            
        }
    }
}