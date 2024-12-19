# Pool Plugin API

## Создание пулов

### PoolSettingsConfig

В папке Resource лежит PoolSettingsConfig. В полях есть пути генерации ID пулов и место хранения репозиториев пулов

### PoolRepository

Чтобы создать репозиторий пула, требуется в верхней панели найти поле Tools

![img.png](img.png)z

Далее в этой панели нажимаем Create Repository. Далее появляется вот такое окно

![img_1.png](img_1.png)

Замечание: id репозитория допускаются только, если нет повторений или id не пустой.

Остальное генератор пулов сделает сам. И запихнет в префаб нужный репозиторий

Далее заполняете репозиторий требуемыми префабами и количеством создаваемых изначально

### PoolContainer

Основное хранилище пулов PoolsContainer. Он может быть использован в Zenject. Префаб контейнера лежит в Resource.
Вытащите его в объект, который будет существовать все время работы приложения.
Обращение к методам ТОЛЬКО через интерфейс IPoolContainer. Из него можно получить требумый пул

## Описание использования

Объект может быть использован в пулет ТОЛЬКО если MonoBehaviour объекта наследуется от PoolObject или на объекте висит
PoolObject. Реализовывать интерфейсы не обязательно

## Основное API

### Интрефейсы объекта в пуле

#### IPoolable

```
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
```

#### ICreatable

```
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
```

#### IReceivable

```
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
```

#### IReleasable

```
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
```

#### PoolObject

```
public class PoolObject : MonoBehaviour, IPoolable
{
    public string Id => string.IsNullOrEmpty(_id) ? gameObject.name.Replace(Constants.CloneName, "") : _id;
}
```

### Интрефейсы пула

#### IPoolContainer

```
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
```

#### IPool

```
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
```