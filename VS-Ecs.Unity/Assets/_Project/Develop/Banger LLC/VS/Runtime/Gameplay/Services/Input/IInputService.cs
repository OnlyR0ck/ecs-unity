using System;
using UnityEngine;

namespace VS.Runtime.Services.Input
{
    public interface IInputService
    {
        public event Action<Vector2> OnDrag;
        public event Action<Vector2> OnEndDrag;
        public event Action<Vector2> OnStartDrag;
    }
}