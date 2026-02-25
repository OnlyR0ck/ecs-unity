using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DCFApixels.DragonECS;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using VContainer;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components.OneFrameComponents.Events;
using VS.Runtime.Core.Components.StateMachine;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Models;
using VS.Runtime.Core.Views;
using VS.Runtime.Extensions;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace VS.Runtime.Core.Systems
{
    public class DropAndPopSystem : IEcsRun
    {
        #if ENABLE_IL2CPP
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class EventAspect : EcsAspect
        {
            public EcsPool<RefreshFieldEvent> Events = Inc;
        }
        
        private readonly EcsDefaultWorld _world;
        private readonly GridModel _gridModel;
        private readonly GameplayRulesConfig _config;

        [Inject]
        public DropAndPopSystem(EcsDefaultWorld world, GridModel gridModel, GameplayRulesConfig config)
        {
            _config = config;
            _gridModel = gridModel;
            _world = world;
        }
        
        
        //TODO: replace dotween either with manual update, either with another allocation free lib
        public void Run()
        {
            foreach (int entity in _world.Where(out EventAspect aspect))
            { 
                Vector2Int index = aspect.Events.Get(entity).Index;

                HashSet<Vector2Int> indices = new HashSet<Vector2Int>();
                _gridModel.GetConnectedSameColoredCells(index, ref indices);

                if (indices.Count < _config.BubblesToPop)
                    continue;
                
                ref var pending = ref _world.Get<PendingAnimations>();
                
                int iter = 0;
                foreach (Vector2Int i in indices)
                {
                    var cell = _gridModel[i];
                    pending.Count++;
                    PopBubble(cell, iter).Forget();
                    iter++;
                }

                IReadOnlyCollection<Vector2Int> unattached = _gridModel.GetUnattached();
                foreach (var i in unattached)
                {
                    var cell = _gridModel[i];
                    GameObject content = cell.Content.gameObject;
                    cell.SetState(ECellState.Free);
                    cell.SetContent(null);
                    
                    pending.Count++;
                    LMotion.Create(0, -20.0f, 0.5f)
                        .WithDelay(0.05f * iter)
                        .WithEase(Ease.InSine)
                        .WithOnComplete(() => {
                            Object.Destroy(content);
                            ref var p = ref _world.Get<PendingAnimations>();
                            p.Count--;
                        })
                        .BindToLocalPositionY(content.transform)
                        .AddTo(content);
                    
                    iter++;
                }
            }
        }

        private async UniTaskVoid PopBubble(CellView cell, int iter)
        {
            GameObject content = cell.Content.gameObject;
            cell.SetState(ECellState.Free);
            cell.SetContent(null);
            await UniTask.Delay(100 * iter);
            Object.Destroy(content);
            
            _world.Get<PendingAnimations>().Count--;
        }
    }
}