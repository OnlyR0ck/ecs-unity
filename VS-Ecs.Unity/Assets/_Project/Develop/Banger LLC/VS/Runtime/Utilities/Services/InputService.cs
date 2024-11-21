using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;
using VS.Runtime.Core.Infrastructure;

namespace VS.Runtime.Utilities
{
    public interface IInputService
    {
        public event Action<Vector2> OnDrag;
        public event Action<Vector2> OnEndDrag;
        public event Action<Vector2> OnStartDrag;
    }

    public class InputService : IInitializable, IDisposable, IInputService
    {
        private readonly InputHandlerService _inputHandlerService;
        private readonly Camera _mainCamera;
        private CancellationTokenSource _cts = new(); 
        
        public event Action<Vector2> OnDrag;
        public event Action<Vector2> OnEndDrag;
        public event Action<Vector2> OnStartDrag;

        [Inject]
        public InputService(InputHandlerService inputHandlerService, CoreGameSceneRefs refs)
        {
            _inputHandlerService = inputHandlerService;
            _mainCamera = refs.GameCamera;
        }

        public void Initialize()
        {
            _inputHandlerService.OnDown += HandlePointerDown;
            _inputHandlerService.OnUp += HandlePointerUp;
        }

        public void Dispose()
        {
            _inputHandlerService.OnDown -= HandlePointerDown;
            _inputHandlerService.OnUp -= HandlePointerUp;
            
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private void HandlePointerDown(PointerEventData eventData)
        {
            StartDrag(eventData);
        }

        private void StartDrag(PointerEventData data)
        {
            _cts = new CancellationTokenSource();
            OnStartDrag?.Invoke(GetWorldPosition(data.position));
            ProcessDrag(_cts.Token).Forget();
        }

        private async UniTaskVoid ProcessDrag(CancellationToken ctsToken)
        {
            while (ctsToken.IsCancellationRequested == false)
            {
                Vector3 currentPosition = Vector3.zero;
                
                #if UNITY_EDITOR
                currentPosition = Input.mousePosition;
                #elif UNITY_ANDROID || UNITY_IOS
                currentPosition = Input.GetTouch(0).position;
                #endif
                
                OnDrag?.Invoke(GetWorldPosition(currentPosition));
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        private void HandlePointerUp(PointerEventData eventData)
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private Vector3 GetWorldPosition(Vector2 position) => 
            _mainCamera.ScreenToWorldPoint(position);
    }
}