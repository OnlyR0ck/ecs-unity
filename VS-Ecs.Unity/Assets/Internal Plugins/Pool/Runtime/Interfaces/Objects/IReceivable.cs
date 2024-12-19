namespace VS.Pool.Interfaces
{
    /// <summary>
    /// Интерфейс, который вызывается при взятии объекта из пула
    /// </summary>
    public interface IReceivable : IPoolable
    {
        /// <summary>
        /// Объект взят из пула
        /// </summary>
        public void OnReceived();
    }
}