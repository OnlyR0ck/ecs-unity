using System;
using UnityEngine.EventSystems;

namespace VS.Runtime.Utilities
{
    public interface IInputHandlerService
    {
        event Action<PointerEventData> OnUp;
        event Action<PointerEventData> OnDown;
    }
}