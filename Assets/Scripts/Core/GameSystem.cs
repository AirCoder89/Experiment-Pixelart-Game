using System.Collections.Generic;
using System.Linq;
using AirCoder.Extensions;
using NaughtyAttributes;

namespace AirCoder.Core
{
    public interface IFixedTick
    {
        void FixedTick();
    }
    
    public interface ILateTick
    {
        void LateTick();
    }
    
    /// <summary>
    /// Game System base class
    /// </summary>
    public abstract class GameSystem
    {
        public GameController Controller { get; protected set; }
        public Application application { get; protected set; }

        protected List<GameComponent> _components;
        
        protected bool IsRun;
        protected bool IsInitialized;

        protected virtual void ReleaseReferences()
        {
            Controller = null;
            application = null;
        }

        public GameSystem(GameController inController, Application inApp, SystemConfig inConfig = null)
        {
            this.Controller = inController;
            this.application = inApp;
            IsInitialized = true;
            ClearComponentList();
        }
    
        protected void ClearComponentList()
        {
            if (_components == null)
            {
                _components = new List<GameComponent>();
                return;
            }
            _components?.Clear();
        }
        
        public void RemoveComponent(GameComponent inComponent)
        {
            if(!_components.Contains(inComponent)) return;
            _components.Remove(inComponent);
        }
        
        public void AddComponent(GameComponent inComponent)
        {
            _components.Add(inComponent);
        }
        
        public T GetSystem<T>() where T : GameSystem
        {
            return application.GetSystem<T>();
        }

        public virtual void Tick()
        {
            if(!IsRun) return;
        }

        [Button("Start")]
        public virtual void StartSystem()
        {
            IsRun = true;
        }
    
        [Button("Pause")]
        public virtual void PauseSystem()
        {
            IsRun = false;
        }

        [Button("Resume")]
        public virtual void ResumeSystem()
        {
            IsRun = true;
        }
        
        [Button("Reset")]
        public virtual void ResetSystem()
        {
        
        }

        protected void Log(string inText)
        {
            var prefix = $"S-{GetType().Name}";
           Log(inText, DebugColor.None, prefix, application.parameters.systemsColor);
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