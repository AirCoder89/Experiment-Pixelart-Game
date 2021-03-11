using System.Linq;
using AirCoder.Controllers.AI.Components.FSM;
using AirCoder.Controllers.AI.Components.FSM.EnemyAI.Actions;
using AirCoder.Controllers.AI.Components.FSM.EnemyAI.Decisions;
using AirCoder.Controllers.NPC;
using AirCoder.Controllers.NPC.Components;
using AirCoder.Controllers.Shooting;
using AirCoder.Core;
using AirCoder.Helper;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.AI
{
    public class AISystem : GameSystem
    {
        public AISystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
        }
        
        public State CreateEnemyAi(NpcController inNpcController, NpcDataHolder inNpcData)
        {
            // --------------------------------------------------------------------------------
            // Setup States
            // --------------------------------------------------------------------------------
            var idleState = new State("Npc Idle"); //- entry state
            var patrolState = new State("Npc Patrol");
            var attackState = new State("Npc Attack");
            var dieState = new State("Npc Die");
            
            // --------------------------------------------------------------------------------
            // Build States
            // --------------------------------------------------------------------------------

            BuildIdleState(idleState, inNpcController, inNpcData, patrolState, attackState, dieState);
            BuildPatrolState(patrolState, inNpcController, inNpcData, idleState, attackState, dieState);
            BuildAttackState(attackState, inNpcController, inNpcData, patrolState,dieState);
            BuildDieState(dieState, inNpcController, inNpcData);
            // --------------------------------------------------------------------------------
            // return Entry States
            // --------------------------------------------------------------------------------
            return idleState;
        }

        private void BuildDieState(State inTargetState, NpcController inNpcController, NpcDataHolder inNpcData)
        {
            // --------------------------------------------------------------------------------
            // 1-  Setup Actions
            // --------------------------------------------------------------------------------
            var dieAction = new DieAction(inNpcController);
            
            // --------------------------------------------------------------------------------
            // 2-  Define State
            // --------------------------------------------------------------------------------
            inTargetState.Define(
                Color.green,new Action[]{dieAction},
                null
            );
        }
        private void BuildAttackState(State inTargetState, NpcController inNpcController, NpcDataHolder inNpcData, State inPatrolState,State inDieState)
        {
            // --------------------------------------------------------------------------------
            // 1-  Setup Actions
            // --------------------------------------------------------------------------------
            var attackAction = new AttackAction( inNpcController, inNpcData.profile.fireRate);
            // --------------------------------------------------------------------------------
            // 2-  Setup decisions
            // --------------------------------------------------------------------------------
            var attackToPatrolDecision = new AttackToPatrolDecision(inNpcController);
            var dieDecision = new DieDecision(inNpcController.gameView as IDestructible);
            // --------------------------------------------------------------------------------
            // 3-  Setup Transitions
            // --------------------------------------------------------------------------------
           
            var transitionToPatrol = new Transition()
            {
                hasFalseState = false,
                decision = attackToPatrolDecision,
                trueState = inPatrolState
            };
            var transitionToDie = new Transition()
            {
                hasFalseState = false,
                decision = dieDecision,
                trueState = inDieState
            };
            
            // --------------------------------------------------------------------------------
            // 4-  Define State
            // --------------------------------------------------------------------------------
            inTargetState.Define(
                Color.red,new Action[]{attackAction},
                new []{ transitionToDie, transitionToPatrol}
            );
        }
        
        private void BuildPatrolState(State inTargetState, NpcController inNpcController, NpcDataHolder inNpcData, State inIdleState, State inAttackState, State inDieState)
        {
            // --------------------------------------------------------------------------------
            // 1-  Setup Actions
            // --------------------------------------------------------------------------------
            var patrolAction = new PatrolAction(inNpcController, inNpcData.patrolRange, inNpcData.patrolDuration);
            // --------------------------------------------------------------------------------
            // 2-  Setup decisions
            // --------------------------------------------------------------------------------
            var attackDecision = new AttackDecision(inNpcController);
            var idleDecision = new IdleDecision(patrolAction);
            var dieDecision = new DieDecision(inNpcController.gameView as IDestructible);
            // --------------------------------------------------------------------------------
            // 3-  Setup Transitions
            // --------------------------------------------------------------------------------
            var transitionToAttack = new Transition()
            {
                hasFalseState = false,
                decision = attackDecision,
                trueState = inAttackState
            };
          
            var transitionToIdle = new Transition()
            {
                hasFalseState = false,
                decision = idleDecision,
                trueState = inIdleState
            };
            var transitionToDie = new Transition()
            {
                hasFalseState = false,
                decision = dieDecision,
                trueState = inDieState
            };
            
            // --------------------------------------------------------------------------------
            // 4-  Define State
            // --------------------------------------------------------------------------------
            inTargetState.Define(
                Color.cyan,new Action[]{patrolAction},
                new []{ transitionToDie, transitionToAttack, transitionToIdle}
            );
        }
        private void BuildIdleState(State inTargetState, NpcController inNpcController, NpcDataHolder inNpcData, State inPatrolState,State inAttackState, State inDieState)
        {
            // --------------------------------------------------------------------------------
            // 1-  Setup Actions
            // --------------------------------------------------------------------------------
            var idleAction = new IdleAction( inNpcController, inNpcData.idleDuration );
            // --------------------------------------------------------------------------------
            // 2-  Setup decisions
            // --------------------------------------------------------------------------------
            var attackDecision = new AttackDecision(inNpcController);
            var idleToPatrolDecision = new IdleToPatrolDecision(idleAction);
            var dieDecision = new DieDecision(inNpcController.gameView as IDestructible);
            // --------------------------------------------------------------------------------
            // 3-  Setup Transitions
            // --------------------------------------------------------------------------------
            var transitionToAttack = new Transition()
            {
                hasFalseState = false,
                decision = attackDecision,
                trueState = inAttackState
            };
            var transitionToPatrol = new Transition()
            {
                hasFalseState = false,
                decision = idleToPatrolDecision,
                trueState = inPatrolState
            };
            var transitionToDie = new Transition()
            {
                hasFalseState = false,
                decision = dieDecision,
                trueState = inDieState
            };
            
            // --------------------------------------------------------------------------------
            // 4-  Define State
            // --------------------------------------------------------------------------------
            inTargetState.Define(
                Color.green,new Action[]{idleAction},
                new []{ transitionToAttack, transitionToDie, transitionToPatrol}
                );
        }
        
        public override void Tick()
        {
            if((!IsRun)) return;
            foreach (var gameCollider in _components.ToList())
                gameCollider.Tick();
        }

        public override void PauseSystem()
        {
            base.PauseSystem();
            foreach (var gameCollider in _components.ToList())
                gameCollider.Pause();
        }

        public override void ResumeSystem()
        {
            base.ResumeSystem();
            foreach (var gameCollider in _components.ToList())
                gameCollider.Resume();
        }
    }
}