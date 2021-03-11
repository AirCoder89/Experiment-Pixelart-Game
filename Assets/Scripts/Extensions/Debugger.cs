using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Extensions
{
    public interface IHelper{}
    public interface ILog { void Log(Type type = null);}
    public enum DebugColor
    {
        None,red,green,blue,yellow,black,Violet,SlateBlue,Tomato,Orange,DodgerBlue,MediumSeaGreen
    }
    
    public static class Debugger
    {
        public static void Print(string text,bool clear = false, Type type = null)
        {
            if(!Application.CanDebug(type)) return;
            if (clear) Clear();
            Debug.Log(text);
        }
        
        public static void Print(string text,DebugColor color = DebugColor.None, string prefix = "", DebugColor colorPrefix = DebugColor.None ,bool clear = false, Type type = null)
        {
            if(!Application.CanDebug(type)) return;
            if (clear) Clear();
            if (prefix.IsNullOrEmpty())
            {
                if (color == DebugColor.None) Print(text, clear, type);
                else Debug.Log($"<color={color}>{text}</color>");
            }
            else
            {
                var finalPrefix = colorPrefix == DebugColor.None ? prefix : $"<color={colorPrefix}>{prefix}</color>";
                var finalText = color == DebugColor.None  ? text : $"<color={color}>{text}</color>";
                Debug.Log($"[{finalPrefix}] > {finalText}");
            }
        }
    
        public static void Clear()
        {
			#if UNITY_EDITOR
			            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
			            var type = assembly.GetType("UnityEditor.LogEntries");
			            var method = type.GetMethod("Clear");
			            method.Invoke(new object(), null);
			#endif
        }
    
        public static void PrintDictionary<T,TD>(Dictionary<T, TD> dic,string title,bool clear = false, Type type = null)
        {
            if(!Application.CanDebug(type)) return;
            Print($"-------- Dictionary [{title}] -------",DebugColor.yellow,"", DebugColor.None, clear,type);
            foreach (var item in dic)
            {
                Print($"--> Object List value TypeOf{item.Value} - Key : {item.Key} [", DebugColor.None,"", DebugColor.None, clear, type);
                Print("]", DebugColor.red, "", DebugColor.None, clear, type);
            }
            Print("___________________________________________", DebugColor.None,"", DebugColor.None, clear, type);
        }
        
    
        public static void PrintILogDictionary<T,TD>(Dictionary<T, TD> dic,string title,bool clear = false, Type type = null)
        {
            if(!Application.CanDebug(type)) return;
            Print($"-------- Dictionary [{title}] -------",DebugColor.yellow,"", DebugColor.None, clear,type);
            foreach (var item in dic)
            {
                Print($"--> Object List value TypeOf{item.Value} - Key : {item.Key} [", DebugColor.None,"", DebugColor.None, clear, type);
                var logValue = item.Value as ILog;
                logValue?.Log(type);
                Print("]", DebugColor.red,"", DebugColor.None, clear, type);
            }
            Print("___________________________________________", DebugColor.None,"", DebugColor.None, clear, type);
        }
        
        public static void PrintList<T>(List<T> list,string title,bool clear = false, Type type = null)
        {
            if(!Application.CanDebug(type)) return;
            Print($"-------- List [{title}] -------",DebugColor.yellow,"", DebugColor.None, clear);
            for (var i = 0; i < list.Count; i++)
            {
                Print($"index [{i}] - Value [{list[i].ToString()}]",DebugColor.None,"", DebugColor.None, clear, type);
            }
            Print("___________________________________________", DebugColor.None,"", DebugColor.None, clear, type);
        }
    
        public static void PrintILogList<T>(List<T> list,string title,bool clear = false, Type type = null)
        {
            if(!Application.CanDebug(type)) return;
            Print($"-------- List [{title}] -------",DebugColor.yellow,"", DebugColor.None, clear);
            for (var i = 0; i < list.Count; i++)
            {
                Print($"--> Object List TypeOf{list[i]} - Index : {i} [", DebugColor.None,"", DebugColor.None, clear, type);
                var logValue = list[i] as ILog;
                logValue?.Log(type);
                Print("]", DebugColor.red,"", DebugColor.None, clear, type);
            }
            Print("___________________________________________", DebugColor.None,"", DebugColor.None, clear, type);
        }
    }
}
