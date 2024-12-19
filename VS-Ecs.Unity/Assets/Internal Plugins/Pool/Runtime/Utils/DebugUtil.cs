using UnityEngine;

namespace VS.Pool.Utils
{
    internal static class DebugUtil
    {
        internal static void Log(object message, DebugColor color = DebugColor.Gray)
        {
            if (Debug.isDebugBuild)
                Debug.Log($"<color=#{ColorDictionary.Colors[color]}>{message}</color>");
        }

        internal static void LogWarning(string message, DebugColor color = DebugColor.Gray)
        {
            if (Debug.isDebugBuild)
                Debug.LogWarning($"<color=#{ColorDictionary.Colors[color]}>{message}</color>");
        }

        internal static void LogError(string message, DebugColor color = DebugColor.Gray)
        {
            if (Debug.isDebugBuild)
                Debug.LogError($"<color=#{ColorDictionary.Colors[color]}>{message}</color>");
        }

        internal static void LogException(System.Exception e)
        {
            if (Debug.isDebugBuild)
                Debug.LogException(e);
        }
    }
}