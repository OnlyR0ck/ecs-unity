using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VS.Runtime.Utilities.Logging;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace VS.Runtime.Services
{
    public class SceneService : ISceneService
    {
        private const string LogTag = "SCENE";
        
        public async UniTask LoadScene(int toLoadIndex)
        {
            int currentSceneIndex = UnitySceneManager.GetActiveScene().buildIndex;
            bool isSkipEmpty = currentSceneIndex == RuntimeConstants.Scenes.Loading || currentSceneIndex == RuntimeConstants.Scenes.Bootstrap || toLoadIndex == currentSceneIndex;

            if (isSkipEmpty)
            {
                Log.Default.D(LogTag, $"Empty scene skipped. {SceneUtility.GetScenePathByBuildIndex(toLoadIndex)} is loading.");
                UnitySceneManager.LoadScene(toLoadIndex);
                return;
            }
            
            bool needLoadEmpty = toLoadIndex == RuntimeConstants.Scenes.Meta || toLoadIndex == RuntimeConstants.Scenes.Core || toLoadIndex == RuntimeConstants.Scenes.Loading;

            if (needLoadEmpty)
            {
                Log.Default.D(LogTag, $"{SceneUtility.GetScenePathByBuildIndex(RuntimeConstants.Scenes.Empty)} is loading.");
                UnitySceneManager.LoadScene(RuntimeConstants.Scenes.Empty);
            }
            
            await UniTask.NextFrame();
            
            Log.Default.D(LogTag, $"{SceneUtility.GetScenePathByBuildIndex(toLoadIndex)} is loading.");
            UnitySceneManager.LoadScene(toLoadIndex);
        }
        
        public async UniTask LoadSceneAsync(int toLoadIndex, LoadSceneMode mode = LoadSceneMode.Single)
        {
            int currentSceneIndex = UnitySceneManager.GetActiveScene().buildIndex;
            bool isSkipEmpty = currentSceneIndex == RuntimeConstants.Scenes.Loading || currentSceneIndex == RuntimeConstants.Scenes.Bootstrap || toLoadIndex == currentSceneIndex;

            if (isSkipEmpty)
            {
                Log.Default.D(LogTag, $"Empty scene skipped. {SceneUtility.GetScenePathByBuildIndex(toLoadIndex)} is loading.");
                await UnitySceneManager.LoadSceneAsync(toLoadIndex, LoadSceneMode.Additive);
                return;
            }
            
            bool needLoadEmpty = toLoadIndex == RuntimeConstants.Scenes.Meta || toLoadIndex == RuntimeConstants.Scenes.Core || toLoadIndex == RuntimeConstants.Scenes.Loading;

            if (needLoadEmpty)
            {
                Log.Default.D(LogTag, $"{SceneUtility.GetScenePathByBuildIndex(RuntimeConstants.Scenes.Empty)} is loading.");
                UnitySceneManager.LoadSceneAsync(RuntimeConstants.Scenes.Empty);
            }
            
            Log.Default.D(LogTag, $"{SceneUtility.GetScenePathByBuildIndex(toLoadIndex)} is loading.");
            await UnitySceneManager.LoadSceneAsync(toLoadIndex, mode);
        }

        public async UniTask UnloadSceneAsync(int sceneIndex)
        {
            Scene scene = UnitySceneManager.GetSceneByBuildIndex(sceneIndex);

            if (!scene.isLoaded)
            {
                Log.Default.D(LogTag, $"Scene {SceneUtility.GetScenePathByBuildIndex(sceneIndex)} is not loaded, skipping unload.");
                return;
            }

            Log.Default.D(LogTag, $"{SceneUtility.GetScenePathByBuildIndex(sceneIndex)} is unloading.");
            await UnitySceneManager.UnloadSceneAsync(sceneIndex);
        }

        public void SetSceneEnabled(int sceneIndex, bool enabled)
        {
            var scene = UnitySceneManager.GetSceneByBuildIndex(sceneIndex);
            if (!scene.isLoaded) return;
            foreach (var go in scene.GetRootGameObjects())
            {
                go.SetActive(enabled);
            }
            
            if (enabled)
            {
                UnitySceneManager.SetActiveScene(scene);
            }
            
            Log.Default.D(LogTag, $"Scene {SceneUtility.GetScenePathByBuildIndex(sceneIndex)} {(enabled ? "enabled" : "disabled")}.");
        }
    }
}