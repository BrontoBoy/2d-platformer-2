using UnityEngine;
using System.Collections;
using UnityEngine.Pool;

public class CollectableSpawner<T> : MonoBehaviour where T : CollectableItem
{
    [SerializeField] protected float _spawnDelay = 2f;
    [SerializeField] protected int _defaultPoolCapacity = 10;
    [SerializeField] protected int _maxPoolSize = 100;
    [SerializeField] protected T _collectablePrefab;
    [SerializeField] protected Transform[] _spawnPoints;
    
    protected bool _isSpawningActive = false;
    protected WaitForSeconds _spawnWait;
    protected IObjectPool<T> _objectPool;
    
    protected virtual void OnEnable()
    {
        InitializePool();
        _spawnWait = new WaitForSeconds(_spawnDelay);
        StartSpawning();
    }

    protected virtual void OnDisable()
    {
        StopSpawning();
    }
    
    public void StartSpawning()
    {
        if (_isSpawningActive) 
            return;

        _isSpawningActive = true;
        StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        _isSpawningActive = false;
    }
    
    protected virtual void InitializePool()
    {
        _objectPool = new ObjectPool<T>(
            createFunc: Create,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyObject,
            collectionCheck: true,
            defaultCapacity: _defaultPoolCapacity,
            maxSize: _maxPoolSize
        );
    }
    
    protected virtual T Create()
    {
        T item = Instantiate(_collectablePrefab, transform);
        item.gameObject.SetActive(false);
        
        return item;
    }
    
    protected virtual void OnGet(T item)
    {
        item.gameObject.SetActive(true);
        item.Collected += OnCollected;
    }
    
    protected virtual void OnRelease(T item)
    {
        item.gameObject.SetActive(false);
        item.transform.SetParent(transform);
        item.Collected -= OnCollected;
    }
    
    protected virtual void OnDestroyObject(T item)
    {
        item.Collected -= OnCollected;
        Destroy(item.gameObject);
    }
    
    protected virtual IEnumerator SpawnRoutine()
    {
        while (_isSpawningActive)
        {
            SpawnSingleObject();
            
            yield return _spawnWait;
        }
    }

    protected virtual void SpawnSingleObject()
    {
        if (_objectPool == null || _spawnPoints.Length == 0)
        {
            return;
        }
        
        int randomIndex = Random.Range(0, _spawnPoints.Length);
        T item = _objectPool.Get();
        item.transform.position = _spawnPoints[randomIndex].position;
    }
    
    protected virtual void OnCollected(ICollectable collectable)
    {
        if (collectable is T typedItem)
        {
            _objectPool.Release(typedItem);
        }
    }
}