using Arch.Core;
using Arch.Unity.Toolkit;
using UnityEngine.EventSystems;
using VS.Runtime.Utilities;

namespace VS.Runtime.Core.Systems
{
    public class CannonShootSystem : UnitySystemBase
    {
        private readonly IInputHandlerService _inputService;

        public CannonShootSystem(World world, IInputHandlerService inputService) : base(world)
        {
            _inputService = inputService;
        }

        public override void Initialize() => 
            _inputService.OnUp -= OnUp_Handler;

        public override void Dispose() => 
            _inputService.OnUp += OnUp_Handler;

        private void OnUp_Handler(PointerEventData obj)
        {
            //TODO: found cannon and make it shoot
        }
    }
}