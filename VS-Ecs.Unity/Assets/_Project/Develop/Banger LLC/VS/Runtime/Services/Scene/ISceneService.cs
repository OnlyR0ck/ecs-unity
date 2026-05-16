using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace VS.Runtime.Services
{
    public interface ISceneService
    {
        UniTask LoadScene(int toLoadIndex);
        UniTask LoadSceneAsync(int toLoadIndex, LoadSceneMode mode = LoadSceneMode.Single);
    }
}