using System;
using AirCoder.Controllers.NPC.Components;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.AI.Components.FSM
{
    public class State
    {
        public string name;
        public bool isDefined;
        
        private Color _color;
        private Action[] _actions;
        private Transition[] _transitions;

        private StateMachine _stateMachine;
        private bool _isInitialized;

        public State(string inName)
        {
            name = inName;
        }
        public void Define(Color inColor, Action[] inActions, Transition[] inTransitions)
        {
            isDefined = true;
            _isInitialized = false;
            _color = inColor;
            _actions = inActions;
            _transitions = inTransitions;
        }
        
        public void Initialize(StateMachine inMachine)
        {
            if(_isInitialized || !isDefined) return;
            _isInitialized = true;
            _stateMachine = inMachine;
        }
        
        public void Enter()
        {
            //_stateMachine.SetColor(color);
            if (_transitions == null || _transitions.Length == 0) Debug.LogWarning($"State ({name}) doesn't have Transitions");
            foreach (var action in _actions)
                action.OnEnter();
        }
        
        public void Tick()
        {
            CheckTransitions();
            DoActions();
        }

        private void CheckTransitions()
        {
            if (_transitions == null || _transitions.Length == 0) return;
            foreach (var transition in _transitions)
            {
                var decisionSucceeded = transition.decision.Decide();
                if (decisionSucceeded) 
                {
                    _stateMachine.SetState(transition.trueState);
                } 
                else if(transition.hasFalseState)
                {
                    if(transition.falseState == null)
                        throw new Exception("if hasFalseState = true, falseState must be not null !!");
                    _stateMachine.SetState(transition.falseState);
                }
            }
        }

        private void DoActions()
        {
            if (_actions == null || _actions.Length == 0)
            {
                Debug.LogWarning($"State ({name}) doesn't have actions");
                return;
            }
            foreach (var action in _actions)
                action.OnTick();
        }

        public void Exit()
        {
            foreach (var action in _actions)
                action.OnExit();
        }
    }
}