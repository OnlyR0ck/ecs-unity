using Cysharp.Threading.Tasks;

namespace VS.Runtime.Services
{
    public interface ILoadingService
    {
        UniTask BeginLoading(ILoadUnit loadUnit, bool skipExceptionThrow = false);
        UniTask BeginLoading(IDisposableLoadUnit unit, bool skipExceptionThrow = false);
        UniTask BeginLoading<T>(ILoadUnit<T> loadUnit, T param, bool skipExceptionThrow = false);
        UniTask BeginLoading<T>(IDisposableLoadUnit<T> unit, T param, bool skipExceptionThrow = false);
        UniTask BeginLoading(bool skipExceptionThrow = false, params ILoadUnit[] units);
        UniTask BeginLoadingParallel(string logName, bool skipExceptionThrow = false, params ILoadUnit[] units);
    }
}