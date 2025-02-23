using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VS.Runtime.Services.Input
{
    public class InputHandlerService : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IInputHandlerService
    {
        public event Action<PointerEventData> OnUp;
        public event Action<PointerEventData> OnMove;
        public event Action<PointerEventData> OnDown;

        public void OnPointerDown(PointerEventData eventData) =>
            OnDown?.Invoke(eventData);

        public void OnPointerUp(PointerEventData eventData) => 
            OnUp?.Invoke(eventData);

        public void OnPointerMove(PointerEventData eventData) => 
            OnMove?.Invoke(eventData);
    }
}