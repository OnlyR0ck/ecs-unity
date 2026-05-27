using System;
using R3;
using VS.Runtime.Meta.UI;
using VS.Runtime.Services;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;
using VS.Runtime.Shared.UI;
using VS.Runtime.Utilities.Logging;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace VS.Runtime.Meta
{
    public class MetaFlow : IStartable, IDisposable
    {
        private readonly ISceneService _sceneService;
        private readonly IScreenService _screenService;
        private readonly LobbyViewModel _lobbyVm;
        private readonly LifetimeScope _parent;

        [Inject]
        public MetaFlow(ISceneService sceneService, IScreenService screenService, LobbyViewModel lobbyVm, LifetimeScope parent)
        {
            _parent = parent;
            _sceneService = sceneService;
            _screenService = screenService;
            _lobbyVm = lobbyVm;
        }

        public async void Start()
        {
            try
            {
                await _screenService.Show<LobbyScreen>();
                _lobbyVm.EnterGame.Subscribe(OnEnterGame).AddTo(_lobbyVm.Disposables);
            }
            catch (Exception e)
            {
                Log.Loading.E("MetaFlow.Start", e);
            }
        }

        public void Dispose() => SceneManager.sceneUnloaded -= OnSceneUnloaded;

        private void OnEnterGame(Unit _) => LoadCoreAsync();

        private async void LoadCoreAsync()
        {
            using (LifetimeScope.EnqueueParent(_parent))
            {
                await _sceneService.LoadSceneAsync(RuntimeConstants.Scenes.Core, LoadSceneMode.Additive);
            }
            _sceneService.SetSceneEnabled(RuntimeConstants.Scenes.Meta, false);
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(UnityScene scene)
        {
            if (scene.buildIndex != RuntimeConstants.Scenes.Core) return;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            _sceneService.SetSceneEnabled(RuntimeConstants.Scenes.Meta, true);
        }
    }
}