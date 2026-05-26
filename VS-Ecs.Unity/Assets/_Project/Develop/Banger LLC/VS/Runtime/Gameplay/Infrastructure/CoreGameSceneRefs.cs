using UnityEngine;

namespace VS.Runtime.Core.Infrastructure
{
    public interface ICoreGameSceneRefs : IUISceneReferences
    {
        Transform LevelRoot { get; }
        Camera GameCamera { get; }
    }

    public interface IUISceneReferences
    {
        Canvas Canvas { get; }
        Transform ScreensRoot { get; }
        Transform PopupsRoot { get; }
        Transform MessagesRoot { get; }   
    }

    public class CoreGameSceneRefs : MonoBehaviour, ICoreGameSceneRefs
    {
        [field: SerializeField] public Transform ScreensRoot { get; private set; }
        [field: SerializeField] public Transform PopupsRoot { get; private set; }
        [field: SerializeField] public Transform MessagesRoot { get; private set; }
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public Transform LevelRoot { get; private set; }
        [field: SerializeField] public Camera GameCamera { get; private set; }
    }
}