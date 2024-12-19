namespace VS.Pool.Interfaces
{
    /// <summary>
    /// Контейнер пулов
    /// </summary>
    public interface IPoolContainer
    {
        /// <summary>
        /// Загрузить все объекты
        /// </summary>
        public void PrePool();

        /// <summary>
        /// Получить пул
        /// </summary>
        /// <param name="id">Id пула</param>
        /// <returns>Пул</returns>
        public IPool GetPool(string id);

        /// <summary>
        /// Очистить пул
        /// </summary>
        /// <param name="id">Id пула</param>
        public void ReleasePool(string id);

        /// <summary>
        /// Очисить все пулы
        /// </summary>
        public void ReleaseAllPools();
    }
}