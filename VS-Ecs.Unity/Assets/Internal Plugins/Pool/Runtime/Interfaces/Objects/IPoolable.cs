namespace VS.Pool.Interfaces
{
    /// <summary>
    /// Базовый интерфейс пула
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Id объекта (либо вводить в инспекторе, если пустой - принимает имя объекта)
        /// </summary>
        public string Id { get; }
    }
}