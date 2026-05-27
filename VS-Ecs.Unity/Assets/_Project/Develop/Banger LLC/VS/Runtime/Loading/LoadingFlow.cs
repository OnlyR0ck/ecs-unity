using System;
using VS.Runtime.Services;
using VS.Runtime.Bootstrap.Units;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;
using VS.Runtime.Utilities.Logging;

namespace VS.Runtime.Loading
{
    public class LoadingFlow : IStartable
    {
        private readonly ISceneService _sceneService;
        private readonly ILoadingService _loadingService;
        private readonly LifetimeScope _parent;

        [Inject]
        public LoadingFlow(ISceneService sceneService, ILoadingService loadingService, LifetimeScope parent)
        {
            _sceneService = sceneService;
            _loadingService = loadingService;
            _parent = parent;
        }

        public async void Start()
        {
            try
            {
                await _loadingService.BeginLoading(new FooLoadingUnit(3));

                using (LifetimeScope.EnqueueParent(_parent.Parent))
                {
                    await _sceneService.LoadSceneAsync(RuntimeConstants.Scenes.Meta, LoadSceneMode.Additive);
                }

                await _sceneService.UnloadSceneAsync(RuntimeConstants.Scenes.Loading);
            }
            catch (Exception e)
            {
                Log.Loading.E("LoadingFlow.Start", e);
            }
        }
    }
}