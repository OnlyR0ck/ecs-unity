using System;
using UnityEngine.EventSystems;

namespace VS.Runtime.Services.Input
{
    public interface IInputHandlerService
    {
        event Action<PointerEventData> OnUp;
        event Action<PointerEventData> OnDown;
    }
}