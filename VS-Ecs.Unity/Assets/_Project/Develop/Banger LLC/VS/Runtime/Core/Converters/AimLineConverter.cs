using Arch.Unity.Conversion;
using UnityEngine;
using VS.Runtime.Core.Components;

namespace VS.Runtime.Core.Converters
{
    public class AimLineConverter : MonoBehaviour, IComponentConverter
    {
        [field: SerializeField] public LineRenderer LineRenderer { get; private set; }

        public void Convert(IEntityConverter converter)
        {
            converter.AddComponent(new AimLine
            {
                AimLineRenderer = LineRenderer
            });
        }
    }
}