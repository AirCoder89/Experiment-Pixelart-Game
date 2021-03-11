using System;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;

namespace AirCoder.Controllers.GamePhysics.Components
{
    public class RaySensor : GameComponent
    {
        public event Action<GameObject> onHitEvent;
        public event Action onHitLostEvent;
        
        private Vector2 _origin;
        private Vector2 _direction;
        private float _length;
        private bool _drawRay;
        private bool _filterByLayer;
        private LayerMask _layerMask;
        private Transform _target;
        protected RaycastHit2D[] _hitsBuffer;
        private Vector2 _offset = Vector2.zero;
        private GameObject _lastHit;
        
        
        
        //- overloading the constructor to fit our needs
        public RaySensor(GameView inGameView, Vector2 inOrigin, Vector2 inDirection, float inLength , LayerMask inLayerMask, bool inDraw = false) : base(inGameView)
        {
            _lastHit = null;
            _offset = Vector2.zero;
            _filterByLayer = true;
            _layerMask = inLayerMask;
            _origin = inOrigin;
            _direction = inDirection;
            _length = inLength;
            _drawRay = inDraw;
            _hitsBuffer = new RaycastHit2D[5];
            SystemFacade.Physics.AddComponent(this);
        }
        public RaySensor(GameView inGameView,Vector2 inDirection, float inLength,LayerMask inLayerMask, bool inDraw = false) : base(inGameView)
        {
            _lastHit = null;
            _offset = Vector2.zero;
            _filterByLayer = true;
            _layerMask = inLayerMask;
            _target = gameView.gameObject.transform;
            _origin = gameView.gameObject.transform.position;
            _direction = inDirection;
            _length = inLength;
            _drawRay = inDraw;
            _hitsBuffer = new RaycastHit2D[5];
            SystemFacade.Physics.AddComponent(this);
        }
        public RaySensor(GameView inGameView,Vector2 inDirection, float inLength, bool inDraw = false) : base(inGameView)
        {
            _lastHit = null;
            _offset = Vector2.zero;
            _filterByLayer = false;
            _target = gameView.gameObject.transform;
            _origin = gameView.gameObject.transform.position;
            _direction = inDirection;
            _length = inLength;
            _drawRay = inDraw;
            _hitsBuffer = new RaycastHit2D[5];
            SystemFacade.Physics.AddComponent(this);
        }
        public RaySensor(GameView inGameView, Vector2 inOrigin, Vector2 inDirection, float inLength, bool inDraw = false) : base(inGameView)
        {
            _lastHit = null;
            _offset = Vector2.zero;
            _filterByLayer = false;
            _origin = inOrigin;
            _direction = inDirection;
            _length = inLength;
            _drawRay = inDraw;
            _hitsBuffer = new RaycastHit2D[5];
            SystemFacade.Physics.AddComponent(this);
        }

        public override void Destroy()
        {
            onHitEvent = null;
            onHitLostEvent = null;
            SystemFacade.Physics.RemoveComponent(this);
            
            //- release references
            base.Destroy();
            _lastHit = null;
            _hitsBuffer = null;
            _target = null;
        }
        
        public override void Tick() //Fixed Tick
        {
            base.Tick();
            _origin = (_target == null ? _origin : (Vector2)_target.position) + _offset;
            var hit = _filterByLayer 
                ? Physics2D.RaycastNonAlloc(_origin, _direction, _hitsBuffer, _length, _layerMask)
                : Physics2D.RaycastNonAlloc(_origin, _direction, _hitsBuffer, _length);

            if (hit > 1)
            {
                _lastHit = _hitsBuffer[1].collider.gameObject;
                //- hit
                onHitEvent?.Invoke(_lastHit);
            }
            else if(_lastHit != null)
            {
                //- hit lost
                _lastHit = null;
                onHitLostEvent?.Invoke();
            }
            
            if(_drawRay) Debug.DrawRay(_origin, _direction * _length, Color.red);
        }

        //- Useful API
        public void SetOffset(Vector2 inOffset) => _offset = inOffset;
        public void SetDirection(Vector2 inDirection) => _direction = inDirection;
        public void SetOrigin(Vector2 inOrigin) =>  _origin = inOrigin;
        public void SetLayerMask(LayerMask inLayerMask)
        {
            _filterByLayer = true;
            _layerMask = inLayerMask;
        }
        public void RemoveLayerMask() => _filterByLayer = false;
        public bool DrawRay => _drawRay;
    }
}