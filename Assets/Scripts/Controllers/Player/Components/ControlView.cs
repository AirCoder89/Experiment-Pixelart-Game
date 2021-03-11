using System;
using AirCoder.Controllers.Animations.Components;
using AirCoder.Controllers.GamePhysics.Components;
using AirCoder.Controllers.VirtualInput;
using AirCoder.Controllers.VisualEffects.Components;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using NaughtyAttributes;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Player.Components
{
    public class ControlView : GameComponent
    {
        public PlayerProfile profile { get; private set; }
        public float horizontalMove { get; private set;}
        public float shift { get;  set;}
        
        private ActorActions _lastAction;
        private ActorActions _currentAction;
        private bool _jumpRequest;
        private  bool _duckRequest;
        private float _lastVelocity;
        private bool _playerInTheAir;
        private bool _isBlocked;
        private bool _isQuickMove;
        private ActorFacing _facing;
        private int _directionX;
        
        private readonly GameCollider _collider;
        private readonly GameAnimator _animator;
        private readonly RaySensor _levelColliderSensor;
        private readonly PlayerView _view;
        private bool _isDelay;
        private float _delayCounter;
        private float _delayAmount;
        private Action _delayCallback;
        
        public ControlView(GameView inGameView, PlayerProfile inProfile) : base(inGameView)
        {
            // --------------------------------------------------------------------------------
            // Initializing
            // --------------------------------------------------------------------------------
            _collider = inGameView.GetComponent<HitBox>() ??
                        throw new NullReferenceException($"Missing Component HitBox");
            
            _animator = inGameView.GetComponent<GameAnimator>() ??
                        throw new NullReferenceException($"Missing Component GameAnimator");
            
            _view = inGameView as PlayerView ?? throw new Exception($"ControlView must be attached on PlayerView");
            
            _facing = ActorFacing.Right;
            _directionX = 1;
            profile = inProfile;
            _lastAction = ActorActions.None;
            _currentAction = ActorActions.None;
            _isQuickMove = false;
            _jumpRequest = false;
            
            // --------------------------------------------------------------------------------
            // Add Ray Sensor to detect is level collider
            // --------------------------------------------------------------------------------
            var rayChild = _view.CreateChild("CollisionRay");
            var rayLength = (_view.GetSize().x/2) + profile.colliderBlockDistance;
            _levelColliderSensor = rayChild.AddComponent(new RaySensor(rayChild, GetDirection(), rayLength, true));
            _levelColliderSensor.onHitEvent += inO => { _isBlocked = true; };
            _levelColliderSensor.onHitLostEvent += () => { _isBlocked = false;};
            _levelColliderSensor.SetOffset(new Vector2(0, (_view.GetSize().y / 2))); //set the origin ray as middle center
            // --------------------------------------------------------------------------------
            // Add Ray Sensor to detect is grounded
            // --------------------------------------------------------------------------------
            var rayGround = gameView.AddComponent(new RaySensor(gameView, Vector2.down, 5f, true));
            rayGround.onHitEvent += inO => { _playerInTheAir = false; };
            rayGround.onHitLostEvent += () => { _playerInTheAir = true;};
        }
        
        public override void Destroy()
        {
            // --------------------------------------------------------------------------------
            // Remove components & release references
            // --------------------------------------------------------------------------------
            gameView.RemoveComponent<RaySensor>();
            base.Destroy();
            profile = null;
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
            
            if (inAction == _currentAction) return;
            _lastAction = _currentAction;
            _currentAction = inAction;
            if(profile == null) throw new Exception($"Player profile must not null");
            if(_animator == null) throw new Exception($"Player needs GameAnimator Component to do this action !");
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
            _animator.PLay(profile.GetAnimationData(inAction));
        }
        
        public void QuickMove(float inHorizontal)
        {
            // --------------------------------------------------------------------------------
            // if Shift key ? activate the tail fx by adding Ghost effect component
            // --------------------------------------------------------------------------------
            if(profile.ghostEffect && !_isQuickMove) 
                gameView.AddComponent(new GhostEffect(gameView, profile.ghostDuration, profile.ghostColor, profile.ghostDensity));
            
            _isQuickMove = true;
            Move(inHorizontal, profile.quickMoveSpeed);
        }

        public void NormalMove(float inHorizontal)
        {
            // --------------------------------------------------------------------------------
            // shift key not pressed any more ? remove the tail fx
            // --------------------------------------------------------------------------------
            if (_isQuickMove && profile.ghostEffect)
                gameView.RemoveComponent<GhostEffect>();
            
            _isQuickMove = false;
            Move(inHorizontal, profile.normalMoveSpeed);
        }

        private void Move(float inHorizontal, float inSpeed)
        {
            if (_collider == null || _currentAction == ActorActions.Shoot) return;
            if ((int)inHorizontal != 0)
            {
                // --------------------------------------------------------------------------------
                // player is on move : play the desired animation
                // --------------------------------------------------------------------------------
                if(!_playerInTheAir) PlayAnimation(ActorActions.Move);
                _directionX = inHorizontal < 0 ? -1 : 1;
                _facing = inHorizontal < 0 ? ActorFacing.Left : ActorFacing.Right;
                _levelColliderSensor.SetDirection(GetDirection());
                gameView.SetDirectionX(_directionX);
                horizontalMove = inHorizontal;
            }
            else
            {
                // --------------------------------------------------------------------------------
                // Player is idle : play the desired animation
                // --------------------------------------------------------------------------------
                horizontalMove = 0;
                if(!_playerInTheAir && _currentAction != ActorActions.Shoot)
                    PlayAnimation(ActorActions.Idle);
            }
            
            // --------------------------------------------------------------------------------
            // Translate target view
            // --------------------------------------------------------------------------------
            if(_isBlocked) return;
            gameView.position = gameView.transform.position;
            gameView.transform.Translate(new Vector3(horizontalMove * (Time.fixedDeltaTime + inSpeed), 0, 0));
        }
        
        public bool IsOnMove()
        {
            // --------------------------------------------------------------------------------
            // Player is on move or blocked by something ?
            // --------------------------------------------------------------------------------
            return !_isBlocked;  
        }
       
        public void Shoot(ButtonState inBtnState)
        {
             if (inBtnState == ButtonState.ButtonDown && !_playerInTheAir) DoShoot();
        }
        
        public void Jump(ButtonState inBtnState)
            => _jumpRequest = (inBtnState == ButtonState.ButtonDown && !_playerInTheAir);
        
        public void Duck(ButtonState inBtnState)
        {
           _duckRequest = (inBtnState == ButtonState.ButtonDown  && !_playerInTheAir);
        }
        
        public void FixedTick()
        {
            if(_collider == null || !_view.IsAlive) return;
            if (_playerInTheAir)
            {
                // --------------------------------------------------------------------------------
                // Player is in the air : play the desired animation (jumping or falling)
                // --------------------------------------------------------------------------------
                _lastVelocity = _collider.RigidBody.velocity.y;
                if (_lastVelocity > 0) PlayAnimation(ActorActions.Jumping);
                else if (_lastVelocity < 0) PlayAnimation(ActorActions.Falling);
            }
            else if (_lastVelocity != 0)
            {
                _lastVelocity = 0;
                PlayAnimation(ActorActions.Landing);
            }
            
            // --------------------------------------------------------------------------------
            // Apply actions requests
            // --------------------------------------------------------------------------------
            if (_jumpRequest && !_playerInTheAir) DoJump();
            if (_duckRequest && !_playerInTheAir) DoDuck();
        }

        private void DoShoot()
        {
            PlayAnimation(ActorActions.Shoot, () => { PlayAnimation(ActorActions.Idle); });
            var bulletPos = gameView.transform.position + new Vector3(profile.bulletSlot.x * _directionX, profile.bulletSlot.y, gameView.transform.position.z);
            SystemFacade.Shooting.Shoot(profile.bullet, gameView.gameObject, bulletPos, GetDirection());
        }
        
        private void DoDuck()
        {
            Log("DoDuck" );
          //  PlayAnimation(InputBehaviour.Duck);
        }

        public void DoDie(ActorActions inDieSide)
        {
            PlayAnimation(inDieSide, () => { SystemFacade.Player.RemovePlayer(); }, 3f);
        }
        
        private void DoJump()
        {
            if(_lastAction != ActorActions.Jumping)
                PlayAnimation(ActorActions.Jumping);
            
            _jumpRequest = false;
            _collider.RigidBody.velocity = new Vector2(_collider.RigidBody.velocity.x, 0);
            _collider.RigidBody.AddForce(Vector2.up * profile.jumpForce, ForceMode2D.Impulse);
        }

        public int GetIntDirection()
            => _facing == ActorFacing.Left ? -1 : 1;
        public Vector2 GetDirection()
            => _facing == ActorFacing.Left ? Vector2.left : Vector2.right;
    }
}