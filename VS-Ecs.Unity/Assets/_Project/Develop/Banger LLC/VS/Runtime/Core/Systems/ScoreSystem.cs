using DCFApixels.DragonECS;
using VContainer;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Components.OneFrameComponents.Events;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace VS.Runtime.Core.Systems
{
    public class ScoreSystem : IEcsRun
    {
#if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
        private class PoppedAspect : EcsAspect
        {
            public EcsPool<BubblesPoppedEvent> Events = Inc;
        }

#if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
        private class DroppedAspect : EcsAspect
        {
            public EcsPool<BubblesDroppedEvent> Events = Inc;
        }

        private readonly EcsDefaultWorld _world;
        private readonly GameplayRulesConfig _config;

        [Inject]
        public ScoreSystem(EcsDefaultWorld world, GameplayRulesConfig config)
        {
            _world = world;
            _config = config;
        }

        public void Run()
        {
            ref var score = ref _world.Get<Score>();

            foreach (var entity in _world.Where(out PoppedAspect poppedAspect))
            {
                int n = poppedAspect.Events.Get(entity).Count;
                // sum(base + i*increment, i=0..n-1) = n*base + increment*(n*(n-1)/2)
                score.Total += n * _config.PopScoreBase + _config.PopScoreIncrement * (n * (n - 1) / 2);
            }

            foreach (var entity in _world.Where(out DroppedAspect droppedAspect))
            {
                int n = droppedAspect.Events.Get(entity).Count;
                score.Total += n * _config.DropScorePerBubble;
            }
        }
    }
}
