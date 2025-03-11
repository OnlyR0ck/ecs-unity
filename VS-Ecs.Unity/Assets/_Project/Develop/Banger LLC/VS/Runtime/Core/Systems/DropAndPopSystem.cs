using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VS.Core.Configs.Features;
using VS.Runtime.Core.Components.OneFrameComponents.Events;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Models;
using VS.Runtime.Core.Views;
using VS.Runtime.Extensions;

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
        
        public void Run()
        {
            foreach (int entity in _world.Where(out EventAspect aspect))
            { 
                Vector2Int index = aspect.Events.Get(entity).Index;

                HashSet<Vector2Int> indices = new HashSet<Vector2Int>();
                _gridModel.GetConnectedSameColoredCells(index, ref indices);

                if (indices.Count < _config.BubblesToPop)
                    continue;
                
                int iter = 0;
                foreach (Vector2Int i in indices)
                {
                    var cell = _gridModel[i];
                    PopBubble(cell, iter).Forget();
                    iter++;
                }
            }
        }

        private async UniTaskVoid PopBubble(CellView cell, int iter)
        {
            await UniTask.Delay(100 * iter);
            Object.Destroy(cell.Content.gameObject);
            cell.SetContent(null);
            cell.SetState(ECellState.Free);
        }
    }
}