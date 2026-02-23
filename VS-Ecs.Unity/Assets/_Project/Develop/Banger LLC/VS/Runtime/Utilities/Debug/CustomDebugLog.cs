using System;
using System.Collections.Generic;
using System.Reflection;

namespace VS.Runtime.Utilities.Debug
{
    using Debug = UnityEngine.Debug;
    public class CustomDebugLog
    {
        private static MethodInfo _clearConsoleMethod;

        public static void Log(object message, DebugColor color = DebugColor.Gray)
        {
            if (Debug.isDebugBuild)
                Debug.Log($"<color=#{ColorDictionary.Colors[color]}>{message}</color>");
        }

        public static void LogWarning(string message, DebugColor color = DebugColor.Gray)
        {
            if (Debug.isDebugBuild)
                Debug.LogWarning($"<color=#{ColorDictionary.Colors[color]}>{message}</color>");
        }

        public static void LogError(string message, DebugColor color = DebugColor.Gray)
        {
            if (Debug.isDebugBuild)
                Debug.LogError($"<color=#{ColorDictionary.Colors[color]}>{message}</color>");
        }

        public static void LogException(System.Exception e)
        {
            if (Debug.isDebugBuild)
                Debug.LogException(e);
        }

        
        public static void ClearLogConsole()
        {
#if UNITY_EDITOR
            ClearConsoleMethod.Invoke(new object(), null);
#endif
        }

        private static MethodInfo ClearConsoleMethod {
            get {
                if (_clearConsoleMethod == null) {
#if UNITY_EDITOR
                    Assembly assembly = Assembly.GetAssembly (typeof(UnityEditor.SceneView));
                    Type logEntries = assembly.GetType ("UnityEditor.LogEntries");
                    _clearConsoleMethod = logEntries.GetMethod ("Clear");
#endif
                }
                return _clearConsoleMethod;
            }
        }
    }
    
    public enum DebugColor : byte
    {
        Black = 0,
        Gray = 1,
        Green = 2,
        Yellow = 3,
        Orange = 4,
        Blue = 5,
        Red = 6,
        Magenta = 7,
        White = 8
    }
    
    public sealed class ColorDictionary
    {
        private const string Black = "000000";
        private const string Gray = "adadad";
        private const string Green = "19e619";
        private const string Yellow = "f0f409";
        private const string Orange = "ff9900";
        private const string Blue = "00bfff";
        private const string Red = "e34234";
        private const string Magenta = "ce29ff";
        private const string White = "ffffff";

        public static readonly Dictionary<DebugColor, string> Colors = new()
        {
            { DebugColor.Black, Black },
            { DebugColor.Gray, Gray },
            { DebugColor.Green, Green },
            { DebugColor.Yellow, Yellow },
            { DebugColor.Orange, Orange },
            { DebugColor.Blue, Blue },
            { DebugColor.Red, Red },
            { DebugColor.Magenta, Magenta },
            { DebugColor.White, White },
        };
    }
}