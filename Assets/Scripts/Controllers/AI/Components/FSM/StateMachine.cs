using System;
using AirCoder.Controllers.NPC.Components;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;

namespace AirCoder.Controllers.AI.Components.FSM
{
    public class StateMachine : GameComponent
    {
         private State _startState;
         private State _currentState;
         private State _previousState;
         private NpcController _npcController;
        
        public StateMachine(GameView inGameView, State inStartState) : base(inGameView)
        {
            _npcController = inGameView.GetComponent<NpcController>();
            if(_npcController == null) throw new Exception($"NpcController Component must be attached in the target gameView");
            
            _isRun = false;
            _startState = inStartState;
            _currentState = null;
            _previousState = null;
        }

        public void Start()
        {
            if(_startState == null) throw new Exception($"Start State is null");
            SystemFacade.AI.AddComponent(this);
            
            _isRun = true;
             SetState(_startState);
        }

        public override void Pause()
        {
            _isRun = false;
        }

        public override void Resume()
        {
            _isRun = true;
        }

        public override void Tick()
        {
            if(!_isRun || _currentState == null) return;
            _currentState.Tick();
        }

        public void SetState(State inState)
        {
            if(inState == null) throw new Exception("Cannot make transition to a null state !");
            _previousState = _currentState;
            _currentState = inState;
            _previousState?.Exit();
            _currentState.Initialize(this);
            _currentState.Enter();
        }

        public override void Destroy()
        {
            SystemFacade.AI.RemoveComponent(this);
            base.Destroy();
        }
    }
}