namespace VS.Pool.Interfaces
{
    /// <summary>
    /// Интерфейс, который вызывается при создании объекта
    /// </summary>
    public interface ICreatable : IPoolable
    {
        /// <summary>
        /// Объект был создан
        /// </summary>
        public void OnCreate();
    }
}