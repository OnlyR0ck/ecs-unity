using Arch.Core;
using Arch.Unity.Toolkit;
using UnityEngine.EventSystems;
using VS.Runtime.Utilities;

namespace VS.Runtime.Core.Systems
{
    public class CannonAimLineSystem : UnitySystemBase
    {
        private readonly IInputHandlerService _inputService;

        public CannonAimLineSystem(World world, IInputHandlerService inputService) : base(world)
        {
            _inputService = inputService;
        }

        public override void Initialize()
        {
            _inputService.OnUp += OnUp_Handler;
            _inputService.OnDown += OnDown_Handler;
        }

        public override void Dispose()
        {
            _inputService.OnUp -= OnUp_Handler;
            _inputService.OnDown -= OnDown_Handler;
        }

        private void OnDown_Handler(PointerEventData _)
        {
            
        }

        private void OnUp_Handler(PointerEventData _)
        {
            
        }
    }
}