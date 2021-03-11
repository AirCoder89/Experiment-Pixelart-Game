using System;
using System.ComponentModel;
using AirCoder.Controllers.GamePhysics.Components;
using AirCoder.Controllers.Sounds;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Animations.Components
{
    /// <summary>
    /// Component can be attached to any GameView entity.
    /// We can create a custom animation from a sprite sheet or create animated VFX,
    /// with handy parameters such as playing sfx at some frame or update the collider size.
    /// </summary>
    
    public class GameAnimator : GameComponent
    {
        public event Action<AnimationData> OnPlayEvent;
        public event Action<AnimationData> OnStopEvent;
        public event Action<AnimationData> OnCompleteEvent; 
        
        public SpriteRenderer TargetRenderer { get; private set; }

        private AnimationData _animationData;

        private int _currentFrame;
        private float _frameDuration;
        private float _timeCounter;
        
        private HitBox _box;
        private HitBox HitBox => _box ?? (_box = gameView.GetComponent<HitBox>());
        
        public GameAnimator(GameView inTarget, SpriteRenderer inRenderer) : base(inTarget)
        {
            TargetRenderer = inRenderer;
            SystemFacade.Animations.AddComponent(this);
        }

        public void SetAnimationData(AnimationData inData)
        {
            _animationData = inData;
        }
        
        public void Play()
        {
            if (_animationData == null)
            {
                throw new WarningException($"Animation Data is null.");
            }

            PLay(_animationData);
        }
        
        public void PLay(AnimationData inData)
        {
            if (_animationData == inData)
            {
                throw new Exception($"Animation Data must be not null !");
            }
            
                _animationData = inData;
                _currentFrame = _animationData.GetRange().x;
                _frameDuration = _animationData.duration / _animationData.spriteSource.Length;
                _timeCounter = 0f;
    
                DrawFrame();
                
                // --------------------------------------------------------------------------------
                // Play sfx (per loop)
                // --------------------------------------------------------------------------------
                if (!_animationData.perFrame && _animationData.concreteFrame == 0 && _animationData.sfx != SoundList.None)
                    SystemFacade.Sounds.PlaySfx(_animationData.sfx);
                
                _isRun = true;
                OnPlayEvent?.Invoke(this._animationData);
        }

        private void DrawFrame()
        {
            // --------------------------------------------------------------------------------
            // Draw the current frame
            // --------------------------------------------------------------------------------
            if(_currentFrame < 0 || _currentFrame >= _animationData.spriteSource.Length)
                throw new Exception($"animation {_animationData.name} (startFrameIndex [{_animationData.GetRange().x}]/lastFrameIndex [{_animationData.GetRange().y}]) out of range  [{_animationData.spriteSource.Length}]!");
            
            TargetRenderer.sprite = _animationData.spriteSource[_currentFrame];

            if (_animationData.updateColliderSize) UpdateCollider();
            
            // --------------------------------------------------------------------------------
            // Play sfx (concrete frame)
            // --------------------------------------------------------------------------------
            if (!_animationData.perFrame && _animationData.concreteFrame != 0 && _animationData.concreteFrame == _currentFrame && _animationData.sfx != SoundList.None)
                SystemFacade.Sounds.PlaySfx(_animationData.sfx);
            
            // --------------------------------------------------------------------------------
            // Play sfx (per frame)
            // --------------------------------------------------------------------------------
            if (_animationData.perFrame && _animationData.sfx != SoundList.None)
                SystemFacade.Sounds.PlaySfx(_animationData.sfx);
        }

        private void UpdateCollider()
        {
            // --------------------------------------------------------------------------------
            // Update Collider size & Offset
            // --------------------------------------------------------------------------------
            if(HitBox == null) throw new Exception($"update collider size feature only works with HitBox Component!");
            HitBox.size = new Vector2(TargetRenderer.sprite.bounds.size.x, TargetRenderer.sprite.bounds.size.y);
            switch (_animationData.updateOffset)
            {
                case OffsetType.None: return;
                case OffsetType.ResetOffset : 
                    if(_animationData.updateOffsetAtFrame == -1) HitBox.offset = Vector2.zero;
                    else if(_currentFrame == _animationData.updateOffsetAtFrame) HitBox.offset = Vector2.zero;
                    return;
                case OffsetType.SetOffsetX : 
                    if(_animationData.updateOffsetAtFrame == -1) HitBox.offset = new Vector2(TargetRenderer.sprite.bounds.size.x, 0);
                    else if(_currentFrame == _animationData.updateOffsetAtFrame) HitBox.offset =new Vector2(TargetRenderer.sprite.bounds.size.x, 0);
                    return;
                case OffsetType.SetOffsetY : 
                    if(_animationData.updateOffsetAtFrame == -1) HitBox.offset = new Vector2(0, TargetRenderer.sprite.bounds.size.y);
                    else if(_currentFrame == _animationData.updateOffsetAtFrame) HitBox.offset = new Vector2(0, TargetRenderer.sprite.bounds.size.y);
                    return;
                case OffsetType.SetOffsetXY : 
                    if(_animationData.updateOffsetAtFrame == -1) HitBox.offset = new Vector2(TargetRenderer.sprite.bounds.size.x, TargetRenderer.sprite.bounds.size.y);
                    else if(_currentFrame == _animationData.updateOffsetAtFrame) HitBox.offset = new Vector2(TargetRenderer.sprite.bounds.size.x, TargetRenderer.sprite.bounds.size.y);
                    return;
            } 
        }

        public override void Destroy()
        {
            // --------------------------------------------------------------------------------
            // Unsubscribe from system
            // --------------------------------------------------------------------------------
            SystemFacade.Animations.RemoveComponent(this);
            
            // --------------------------------------------------------------------------------
            // Release references
            // --------------------------------------------------------------------------------
            base.Destroy();
            RemoveEventsListener();
            TargetRenderer = null;
            _animationData = null;
        }

        public void RemoveEventsListener()
        {
            OnPlayEvent = null;
            OnStopEvent = null;
            OnCompleteEvent = null;
        }
        
        public override void Pause()
        {
            _isRun = false;
        }

        public override void Resume()
        {
            _isRun = true;
        }

        public void Stop()
        {
            _isRun = false;
            _timeCounter = 0f;
            _currentFrame = _animationData.GetRange().x;
            
            OnStopEvent?.Invoke(this._animationData);
        }

        public override void Tick()
        {
            if(!_isRun || TargetRenderer == null) return;

            _timeCounter += Application.DeltaTime;
            if (!(_timeCounter >= _frameDuration)) return;
                _timeCounter = 0f;
                _currentFrame++;
                if (_currentFrame > _animationData.GetRange().y)
                {
                    // --------------------------------------------------------------------------------
                    // Animation Complete - Loop ?
                    // --------------------------------------------------------------------------------
                    OnCompleteEvent?.Invoke(this._animationData);
                    if (_animationData.loop)
                    {
                        _isRun = false;
                        _timeCounter = 0f;
                        _timeCounter = 0f;
                        _currentFrame = _animationData.GetRange().x;
                        var tmpData = _animationData;
                        _animationData = null;
                        PLay(tmpData);
                    }
                    else Stop();
                }
                else DrawFrame();
        }

    }
}