using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VS.Runtime.Core.Components;

namespace VS.Runtime.Core.Systems
{
    public class TimerTickSystem : IEcsRun
    {
#if ENABLE_IL2CPP
        using Unity.IL2CPP.CompilerServices;
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
        private class TimerAspect : EcsAspect
        {
            public EcsPool<Timer> Timers = Inc;
        }
        
        private readonly EcsDefaultWorld _world;
        private float _tick;

        [Inject]
        public TimerTickSystem(EcsDefaultWorld world)
        {
            _world = world;
        }

        public void Run()
        {
            _tick += Time.deltaTime;
            if (_tick < 1)
                return;

            _tick -= 1;
            
            foreach (var entity in _world.Where(out TimerAspect aspect))
            {
                ref var timer = ref aspect.Timers.Get(entity);
                timer.TimeRemaining = Mathf.Max(timer.TimeRemaining - 1, 0);
            }
        }
    }
}