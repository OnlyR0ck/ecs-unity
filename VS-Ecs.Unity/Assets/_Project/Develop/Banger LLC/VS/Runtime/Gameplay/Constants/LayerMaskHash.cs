using UnityEngine;

namespace VS.Runtime.Core.Constants
{
    public class LayerMaskHash
    {
        public static readonly int Level = LayerMask.GetMask(Layers.Level);
        public static readonly int UI = LayerMask.GetMask(Layers.UI);
        public static readonly int Default = LayerMask.GetMask(Layers.Default);
    }
}