using DCFApixels.DragonECS;
using VContainer;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components.OneFrameComponents.Events;
using VS.Runtime.Services.Session;

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
        private readonly IScoreService _scoreService;

        [Inject]
        public ScoreSystem(EcsDefaultWorld world, GameplayRulesConfig config, IScoreService scoreService)
        {
            _world = world;
            _config = config;
            _scoreService = scoreService;
        }

        public void Run()
        {
            foreach (var entity in _world.Where(out PoppedAspect poppedAspect))
            {
                int n = poppedAspect.Events.Get(entity).Count;
                _scoreService.Add(n * _config.PopScoreBase + _config.PopScoreIncrement * (n * (n - 1) / 2));
            }

            foreach (var entity in _world.Where(out DroppedAspect droppedAspect))
            {
                int n = droppedAspect.Events.Get(entity).Count;
                _scoreService.Add(n * _config.DropScorePerBubble);
            }
        }
    }
}
