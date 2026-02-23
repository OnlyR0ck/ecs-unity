using System;
using DCFApixels.DragonECS;
using UnityEngine;

namespace VS.Runtime.Core.Components
{
    [Serializable]
    public struct RippleEffect : IEcsComponent
    {
        public Vector2 RestPosition;
        public Vector2 Velocity;
        public Vector2 SourcePosition;
        public float Stiffness;
        public float Damping;
        public float Intensity;
    }
    
    public class RippleEffectComponentTemplate : ComponentTemplate<RippleEffect> {}
}
