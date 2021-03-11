using System;
using System.Collections.Generic;
using AirCoder.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AirCoder.Core
{
    public class GameView
    {
        public Vector3 position;
        public Vector3 scale;
        public string name;

        private bool _visibility;
        public bool Visibility
        {
            get  {return _visibility;}
            set
            {
                _visibility = value;
                gameObject.SetActive(_visibility);
            }
        }
        
        public Dictionary<Type, GameComponent> Components{ get; private set; }
        private List<string> _attachedComponents;
        public HashSet<GameView> Children { get; private set; }
        public int index;
        
        public GameObject gameObject { get; private set; }

        public Transform transform => gameObject.transform;

        protected Application _application { get; private set; }
        

        public virtual void Destroy()
        {
            //- Release Game view references
            _application = null;
            Object.Destroy(this.gameObject);
            gameObject = null;
            foreach (var component in Components)
                component.Value.Destroy();
        }
        
        public void SetGameObject(GameObject inGameObject)
        {
            name = inGameObject.name;
            gameObject = inGameObject;
            gameObject.transform.SetParent(null);
            //gameObject.transform.localScale = SystemFacade.Renderer.ResetVector;
            gameObject.transform.localScale = Vector3.one;
        }

        public GameView(Application inApp, GameObject inGameObject)
        {
            _application = inApp;
            SetGameObject(inGameObject);
            
            Children = new HashSet<GameView>();
            _attachedComponents = new List<string>();
            Components = new Dictionary<Type, GameComponent>();
        }
        
        public GameView(Application inApp, string inName)
        {
            _application = inApp;
            
            if(_application == null) throw new Exception("Application is null");
            if (string.IsNullOrEmpty(inName))
            {
                //- generate random name
                inName = $"GV-{Extensions.StringExtensions.GenerateString(8, true)}";
            }
            SetGameObject(new GameObject(inName));
            
            Children = new HashSet<GameView>();
            _attachedComponents = new List<string>();
            Components = new Dictionary<Type, GameComponent>();
        }

        public bool AddChild(GameView inChild)
        {
            inChild.SetParent(this.transform);
            return Children.Contains(inChild);
        }
        
        public GameView CreateChild(string inName)
        {
            var child = new GameView(this._application, inName);
            child.SetParent(this.transform);
            child.SetScale(Vector2.one);
            child.SetPosition(Vector2.zero);
            Children.Add(child);
            return child;
        }

        public bool HasComponent<T>() where T : GameComponent
            => Components.ContainsKey(typeof(T));
        
        public T AddComponent<T>(T inComponent) where T : GameComponent
        {
            if (HasComponent<T>()) return Components[typeof(T)] as T;
            _attachedComponents.Add(typeof(T).Name);
            Components.Add(typeof(T), inComponent);
            return inComponent;
        }

        public T GetComponent<T>() where T : GameComponent
        {
            if (!HasComponent<T>()) return null;
            var component = Components[typeof(T)] as T;
            return component;
        }
        
        public bool RemoveComponent<T>() where T : GameComponent
        {
            if (!HasComponent<T>()) return false;
            var component = Components[typeof(T)];
            _attachedComponents.Remove(typeof(T).Name);
            var res =  Components.Remove(typeof(T));
            component.Destroy();
            return res;
        }

        public void RemoveAllComponents()
        {
            _attachedComponents.Clear();
            foreach (var component in Components)
                component.Value.Destroy();
        }
        
        public virtual void Tick()
        {
            
        }
        
        public virtual void ResumeView()
        {
            
        }
        
        public virtual void PauseView()
        {
            
        }
       
        public virtual void ResetView()
        {
            
        }

        public void FadeIn()
        {
            //Todo
        }

        public void FadeOut()
        {
            //Todo
        }
        
        public void SetParent(Transform inParent) => gameObject.transform.parent = inParent;

        public void SetScale(Vector2 inScale) => gameObject.transform.localScale = scale = inScale;

        public void SetScaleX(float inScaleX)
        {
            scale = gameObject.transform.localScale;
            scale.x = inScaleX;
            gameObject.transform.localScale = scale;
        }
        
        public void SetPosition(Vector2 inPosition) => gameObject.transform.localPosition = inPosition;

        public void SetPositionX(float inPosX)
        {
            position = gameObject.transform.localPosition;
            position.x = inPosX;
            gameObject.transform.localPosition = position;
        }
        
        public void SetPositionY(float inPosY)
        {
            position = gameObject.transform.localPosition;
            position.y = inPosY;
            gameObject.transform.localPosition = position;
        }

        public void SetDirectionX(int inDirection)
        {
            scale = gameObject.transform.localScale;
            scale.x = Mathf.Abs(scale.x)*inDirection;
            gameObject.transform.localScale = scale;
        }

        protected void Log(string inText)
        {
            var prefix = $"V-{GetType().Name}";
            Log(inText, DebugColor.None, prefix, _application.parameters.viewsColor);
        }
        
      
        protected virtual void Log(string text, DebugColor color = DebugColor.None, bool clear = false) 
            => Debugger.Print(text, color, "",DebugColor.black, clear,GetType());

        
        protected virtual void Log(string text ,DebugColor color = DebugColor.None ,string prefix = "",DebugColor colorPrefix = DebugColor.None ,bool clear = false) 
            => Debugger.Print(text, color, prefix, colorPrefix,clear,GetType());

        protected void ClearLog() => Debugger.Clear();

        protected virtual void Log<T, TD>(Dictionary<T, TD> dic, string title = "" ,string prefix = "", bool clear = false)
        {
            if (typeof(TD).IsAssignableFrom(typeof(ILog)))
                Debugger.PrintILogDictionary(dic, title,clear, GetType());
            else 
                Debugger.PrintDictionary(dic, title, clear, GetType());
        }
        
        protected virtual void Log<T>(List<T> list,string title = "" ,string prefix = "",bool clear = false)
        {
            if (typeof(T).IsAssignableFrom(typeof(ILog)))
                Debugger.PrintILogList(list, title, clear, GetType());
            else 
                Debugger.PrintList(list, title, clear, GetType());
        }
        
    }
}
