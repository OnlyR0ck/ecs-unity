using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace VS.Runtime.Services
{
    public interface ISceneService
    {
        UniTask LoadScene(int toLoadIndex);
        UniTask LoadSceneAsync(int toLoadIndex, LoadSceneMode mode = LoadSceneMode.Single);
        UniTask UnloadSceneAsync(int sceneIndex);
        void SetSceneEnabled(int sceneIndex, bool enabled);
    }
}