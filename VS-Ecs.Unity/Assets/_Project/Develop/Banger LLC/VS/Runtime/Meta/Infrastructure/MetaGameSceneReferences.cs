using UnityEngine;

namespace VS.Runtime.Meta.Infrastructure
{
    public class MetaGameSceneReferences : MonoBehaviour, IMetaGameSceneReferences 
    {
        [field: SerializeField] public Transform ScreensRoot { get; private set; }
        [field: SerializeField] public Transform PopupsRoot { get; private set; }
        [field: SerializeField] public Transform MessagesRoot { get; private set; }
        [field: SerializeField] public Canvas Canvas { get; private set; }
    }
}