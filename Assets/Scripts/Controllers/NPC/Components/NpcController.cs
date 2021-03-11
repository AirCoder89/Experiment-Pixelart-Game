using System;
using AirCoder.Controllers.AI.Components.FSM;
using AirCoder.Controllers.Animations.Components;
using AirCoder.Controllers.GamePhysics.Components;
using AirCoder.Controllers.Player.Components;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Action = System.Action;
using Application = AirCoder.Core.Application;
using Vector2Int = AirCoder.Core.Vector2Int;

namespace AirCoder.Controllers.NPC.Components
{
    public class NpcController : GameComponent
    {
        private readonly GameAnimator _animator;
        private readonly GameCollider _collider;
        private readonly EnemyView _view;
        private readonly RaySensor _sightRay;

        public EnemyView enemyView => _view;
        public bool hasEnemy { get; private set; }
        private ActorActions _lastAction;
        private ActorActions _currentAction;
        private bool _jumpRequest;
        private float _lastVelocity;
        private bool _NpcInTheAir;
        private ActorFacing _facing;
        private int _directionX;
        private bool _isDelay;
        private float _delayCounter;
        private float _delayAmount;
        private Action _delayCallback;
        
        public NpcController(GameView inGameView, ActorFacing inFacing) : base(inGameView)
        {
            // --------------------------------------------------------------------------------
            // Initializing
            // --------------------------------------------------------------------------------
            _animator = inGameView.GetComponent<GameAnimator>() ??
                        throw new Exception($"GameView [{inGameView.name}] must attached GameAnimator component.");
            
            _collider = inGameView.GetComponent<HitBox>() ??
                        throw new Exception($"GameView [{inGameView.name}] must attached GameCollider component.");
            
            _view = inGameView as EnemyView ?? throw new Exception($"NpcController can only attached to EnemyView");
            
            _lastAction = ActorActions.None;
            _currentAction = ActorActions.None;
            _facing = inFacing;
            _directionX = GetIntDirection();
            _view.SetDirectionX(_directionX);
            
            // --------------------------------------------------------------------------------
            // Add Ray Sensor to detect player
            // --------------------------------------------------------------------------------
            var rayLength = (_view.GetSize().x/2) + _view.Profile.sightDistance;
            _sightRay = gameView.AddComponent(new RaySensor(gameView, GetDirection(), rayLength, true));
            _sightRay.onHitEvent += inO => { hasEnemy = true; };
            _sightRay.onHitLostEvent += () => { hasEnemy = false;};
            _sightRay.SetOffset(new Vector2(0, (_view.GetSize().y - 7) )); //set the origin ray as middle center
            
            SystemFacade.NPC.AddComponent(this);
        }

        public override void Destroy()
        {
            SystemFacade.NPC.RemoveComponent(this);
            base.Destroy();
        }

        public override void Tick()
        {
            if(!_isDelay) return;
            _delayCounter += Application.DeltaTime;
            if (!(_delayCounter >= _delayAmount)) return;
                _isDelay = false;
                _delayCallback?.Invoke();
        }

        public void PlayAnimation(ActorActions inAction, Action inCallback = null, float inDelay = 0f)
        {
            // --------------------------------------------------------------------------------
            // Play the given animation and prevent over calling
            // --------------------------------------------------------------------------------
            if (inAction == _currentAction)
            {
                Log("return " + inAction);
                return;
            }
            _lastAction = _currentAction;
            _currentAction = inAction;
            _animator.RemoveEventsListener();
            if (inCallback != null)
            {
                _animator.OnCompleteEvent += inData =>
                {
                    if (inDelay > 0f)
                    {
                        _delayAmount = inDelay;
                        _delayCallback = inCallback;
                        _delayCounter = 0f;
                        _isDelay = true;
                    }
                    else inCallback?.Invoke();
                };
            }
            _animator.PLay(_view.Profile.GetAnimationData(inAction));
        }
        
        public void DoIdle()
        {
            PlayAnimation(ActorActions.Idle);
        }

        public void DoShoot()
        {
            PlayAnimation(ActorActions.Shoot, DoIdle);
            var bulletPos = gameView.transform.position + new Vector3(_view.Profile.bulletSlot.x * _directionX, _view.Profile.bulletSlot.y, gameView.transform.position.z);
            SystemFacade.Shooting.Shoot(_view.Profile.bullet, gameView.gameObject, bulletPos, GetDirection());
        }

        public void DoMoveTo(Vector2Int inPatrolRange)
        {
            PlayAnimation(ActorActions.Move);
            
            gameView.transform.Translate(new Vector3(_directionX * (Time.fixedDeltaTime + _view.Profile.speed), 0, 0));
            
            if (_facing == ActorFacing.Right && gameView.transform.localPosition.x > inPatrolRange.y ||
                _facing == ActorFacing.Left && gameView.transform.localPosition.x < inPatrolRange.x)
            {
                _view.SetDirectionX(InvertDirection());
            }
            _sightRay.SetDirection(GetDirection());
        }

        public void DoDie(ActorActions inDieSide)
        {
            PlayAnimation(inDieSide, () => { SystemFacade.NPC.RemoveEnemy(_view); }, 3f);
        }

        private int InvertDirection()
        {
            _directionX *= -1;
            _facing = (ActorFacing)_directionX;
            return _directionX;
        }
        
        public int GetIntDirection()
            => _facing == ActorFacing.Left ? -1 : 1;
        public Vector2 GetDirection()
            => _facing == ActorFacing.Left ? Vector2.left : Vector2.right;
    }
}