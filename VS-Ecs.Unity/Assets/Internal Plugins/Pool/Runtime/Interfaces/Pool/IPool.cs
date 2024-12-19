using UnityEngine;

namespace VS.Pool.Interfaces
{
    /// <summary>
    /// Интерфейс пула
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// Препул пула
        /// </summary>
        public void PrePool();
        
        /// <summary>
        /// Получить элемент из пула, если элемента нет в пуле - создаст
        /// </summary>
        /// <param name="id">ID элемента</param>
        /// <param name="parent">Родитель элемента</param>
        /// <param name="position">Глобальная позиция элемента</param>
        /// <param name="rotation">Глобальный поворот элемента</param>
        /// <param name="scale">Размер элемента</param>
        /// <param name="minObjectInPool">Минимальное количество, которое должно существовать в пуле</param>
        /// <typeparam name="T">Тип объекта (PoolObject)</typeparam>
        /// <returns>Объект из пула</returns>
        public T Get<T>
        (
            string id,
            Transform parent = null,
            Vector3 position = default,
            Quaternion rotation = default,
            Vector3 scale = default,
            int minObjectInPool = 0
        ) where T : PoolObject;

        /// <summary>
        /// Получить элемент из пула, если элемента нет в пуле - создаст
        /// </summary>
        /// <param name="prefab">Префаб элемента</param>
        /// <param name="parent">Родитель элемента</param>
        /// <param name="position">Глобальная позиция элемента</param>
        /// <param name="rotation">Глобальный поворот элемента</param>
        /// <param name="scale">Размер элемента</param>
        /// <param name="minObjectInPool">Минимальное количество, которое должно существовать в пуле</param>
        /// <typeparam name="T">Тип объекта (PoolObject)</typeparam>
        /// <returns>Объект из пула</returns>
        public T Get<T>
        (
            PoolObject prefab,
            Transform parent = null,
            Vector3 position = default,
            Quaternion rotation = default,
            Vector3 scale = default,
            int minObjectInPool = 0
        ) where T : PoolObject;

        /// <summary>
        /// Отправить элеменет в пул через какой-то делэй
        /// </summary>
        /// <param name="poolObject">Элемент</param>
        /// <param name="time">Время, через которое отправиться в пул (в секундах)</param>
        public void ReleaseAfter(PoolObject poolObject, float time);

        /// <summary>
        /// Отправить элеменет в пул 
        /// </summary>
        /// <param name="poolObject">Элемент</param>
        public void Release(PoolObject poolObject);

        /// <summary>
        /// Отправить в пул все элементы с таким id
        /// </summary>
        /// <param name="id">Id элемента</param>
        public void ReleaseAllId(string id);

        /// <summary>
        /// Отправить все объекты в пул
        /// </summary>
        public void ReleaseAll();
    }
}