using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public static class DebugTool
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object message, Object context = null)
    {
        if (context != null)
            Debug.Log(message, context);
        else
            Debug.Log(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message, Object context = null)
    {
        if (context != null)
            Debug.LogWarning(message, context);
        else
            Debug.LogWarning(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(object message, Object context = null)
    {
        if (context != null)
            Debug.LogError(message, context);
        else
            Debug.LogError(message);
    }
}