using System.Collections.Generic;
using UnityEngine;

namespace VS.Runtime.Utilities.Logging
{
    public class CustomDebugLog
    {
            private static readonly Dictionary<DebugColor, string> ColorDictionary = new Dictionary<DebugColor, string>
        {
            { DebugColor.White, "white" },
            { DebugColor.Yellow, "yellow" },
            { DebugColor.Orange, "orange" },
            { DebugColor.Red, "red" },
            { DebugColor.Green, "green" },
            { DebugColor.Blue, "blue" },
            { DebugColor.Cyan, "cyan" },
            { DebugColor.Magenta, "magenta" }
        };

        public static void Log(string message, DebugColor color = DebugColor.White)
        {
#if DEVELOPMENT_BUILD
        Debug.Log($"<color={ColorDictionary[color]}>{message}</color>");
#endif
        }

        public static void LogWarning(string message, DebugColor color = DebugColor.Yellow)
        {
#if DEVELOPMENT_BUILD
        Debug.LogWarning($"<color={ColorDictionary[color]}>{message}</color>");
#endif
        }

        public static void LogError(string message, DebugColor color = DebugColor.Orange)
        {
#if DEVELOPMENT_BUILD
        Debug.LogError($"<color={ColorDictionary[color]}>{message}</color>");
#endif
        }

        public static void LogException(System.Exception exception, DebugColor color = DebugColor.Red)
        {
#if DEVELOPMENT_BUILD
        Debug.LogError($"<color={ColorDictionary[color]}>{exception.Message}\n{exception.StackTrace}</color>");
#endif
        }
    }

    public enum DebugColor
    {
            White,
            Yellow,
            Orange,
            Red,
            Green,
            Blue,
            Cyan,
            Magenta
    }
}