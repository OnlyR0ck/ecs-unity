using System;
using VS.Runtime.Services;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;
using VS.Runtime.Utilities.Logging;

namespace VS.Runtime.Meta
{
    public class MetaFlow : IStartable
    {
        private readonly ISceneService _sceneService;
        private readonly LifetimeScope _parent;

        [Inject]
        public MetaFlow(ISceneService sceneService, LifetimeScope parent)
        {
            _parent = parent;
            _sceneService = sceneService;
        }

        public async void Start()
        {
            try
            {
                using (LifetimeScope.EnqueueParent(_parent))
                {
                    await _sceneService.LoadSceneAsync(RuntimeConstants.Scenes.Core, LoadSceneMode.Additive);
                }
            }
            catch (Exception e)
            {
                Log.Loading.E("MetaFlow.Start", e);
            }
        }
    }
}