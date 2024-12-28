using System;
using DCFApixels.DragonECS;
using UnityEngine;

namespace VS.Runtime.Core.Components
{
    [Serializable]
    public struct AimLineComponent : IEcsComponent
    {
        public LineRenderer AimLineRenderer;
    }

    public class AimLineTemplate : ComponentTemplate<AimLineComponent> { }
}