using Arch.Unity.Conversion;
using UnityEngine;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Views;

namespace VS.Runtime.Core.Converters
{
    public class BubbleConverter : MonoBehaviour, IComponentConverter
    {
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public BubbleView View { get; private set; }
        
        public void Convert(IEntityConverter converter)
        {
             converter.AddComponent(new BubbleComponent
             {
                 Transform = Transform,
                 View = View
             });
        }
    }
}
