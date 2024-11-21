using UnityEngine;

namespace VS.Runtime.Core.Infrastructure
{
    public class CoreGameSceneRefs : MonoBehaviour
    {
        [field: SerializeField] public Transform ScreensRoot { get; private set; }
        [field: SerializeField] public Transform PopupsRoot { get; private set; }
        [field: SerializeField] public Transform MessagesRoot { get; private set; }
        [field: SerializeField] public Canvas GameCanvas { get; private set; }
        [field: SerializeField] public Transform LevelRoot { get; private set; }
        [field: SerializeField] public Camera GameCamera { get; private set; }
    }
}