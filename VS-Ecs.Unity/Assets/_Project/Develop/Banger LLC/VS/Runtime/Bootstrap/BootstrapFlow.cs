using VS.Runtime.Services;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace VS.Runtime.Bootstrap
{
    public class BootstrapFlow : IStartable
    {
        private readonly SceneService _sceneService;
        private readonly LifetimeScope _parent;

        public BootstrapFlow(SceneService sceneService, LifetimeScope parent)
        {
            _parent = parent;
            _sceneService = sceneService;
        }
        
        public async void Start()
        {
            using (LifetimeScope.EnqueueParent(_parent))
            {
                await _sceneService.LoadSceneAsync(RuntimeConstants.Scenes.Core, LoadSceneMode.Additive);
            }
        }
    }
}
