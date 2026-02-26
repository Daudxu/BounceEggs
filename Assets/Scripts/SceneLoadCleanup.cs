using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 在场景加载时移除多余的 EventSystem 和 AudioListener。
/// 使用 [DefaultExecutionOrder(-1000)] 确保在 EventSystem.OnEnable 之前执行。
/// </summary>
[DefaultExecutionOrder(-1000)]
public class SceneLoadCleanup : MonoBehaviour
{
    void Awake()
    {
        RemoveDuplicateEventSystems();
        RemoveDuplicateAudioListeners();
    }

    private void RemoveDuplicateEventSystems()
    {
        var eventSystems = Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        for (int i = 1; i < eventSystems.Length; i++)
            Object.DestroyImmediate(eventSystems[i].gameObject);
    }

    private void RemoveDuplicateAudioListeners()
    {
        var listeners = Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        for (int i = 1; i < listeners.Length; i++)
            Object.DestroyImmediate(listeners[i].gameObject);
    }
}
