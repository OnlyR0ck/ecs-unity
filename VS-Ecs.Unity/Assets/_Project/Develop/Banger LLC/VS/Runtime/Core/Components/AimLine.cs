using System;
using DCFApixels.DragonECS;
using UnityEngine;

namespace VS.Runtime.Core.Components
{
    [Serializable]
    public struct AimLine : IEcsComponent
    {
        public LineRenderer AimLineRenderer;
    }
}