using DCFApixels.DragonECS;
using UnityEngine;

namespace VS.Runtime.Core.Views
{
    public class EcsView : View
    {
        [field: SerializeField] public EcsEntityConnect Connector { get; private set; }
    }
}