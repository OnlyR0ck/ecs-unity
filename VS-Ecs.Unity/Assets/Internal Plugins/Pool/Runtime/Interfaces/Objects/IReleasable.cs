namespace VS.Pool.Interfaces
{
    /// <summary>
    /// Интерфейс, который вызывается при отправки объекта в пул
    /// </summary>
    public interface IReleasable : IPoolable
    {
        /// <summary>
        /// Объект был отправлен в пул
        /// </summary>
        public void OnRelease();
    }
}