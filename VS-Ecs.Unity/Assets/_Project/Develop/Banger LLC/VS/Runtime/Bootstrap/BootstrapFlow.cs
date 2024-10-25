using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer.Unity;
using VS.Runtime.Bootstrap.Units;
using VS.Runtime.Utilities;
using SceneManager = VS.Runtime.Utilities.SceneManager;

namespace VS.Runtime.Bootstrap
{
    public class BootstrapFlow : IStartable
    {
        private readonly SceneManager _sceneManager;
        private readonly LifetimeScope _parent;

        public BootstrapFlow(LoadingService loadingService, SceneManager sceneManager, LifetimeScope parent)
        {
            _parent = parent;
            _sceneManager = sceneManager;
        }
        
        public async void Start()
        {
            using (LifetimeScope.EnqueueParent(_parent))
            {
                await _sceneManager.LoadSceneAsync(RuntimeConstants.Scenes.Core, LoadSceneMode.Additive);
            }
        }
    }
}
