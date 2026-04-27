using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnObjectsFromPool : MonoBehaviour
{
    [Header("Pooled Prefabs")]
    [SerializeField] private HandlePooling[] _objectPrefabs;
    private IObjectPool<HandlePooling> _objectPool;

    public bool ParentToTransform = true; // Toggle for parenting spawned objects to the spawner

    [Header("Spawn Position")]
    public bool RandomizeSpawnPosition = true;
    public Vector3 FixedSpawnPosition = Vector3.zero; // Fixed spawn position if randomization is off
    [SerializeField] private float _xSpawnRange = 5f; // Horizontal spawn range
    [SerializeField] private float _ySpawnRange = 5f; // Vertical spawn range
    private Vector3 _spawnPosition;

    [Header("Time Based Settings")]
    [SerializeField] private bool _delayInitalSpawn = false;
    [SerializeField] private float _initialSpawnDelay = 0f; // Initial delay before spawning

    [SerializeField] private bool _randomizeSpawnInterval = false; // Toggle for randomizing spawn interval
    [SerializeField] private float _standardSpawnInterval = 4f; // Time between spawns
    [SerializeField] private float _minSpawnInterval = 2f; // Minimum spawn interval
    [SerializeField] private float _maxSpawnInterval = 4f; // Maximum time between spawns

    public bool IncrementSpawnRate = false;
    [SerializeField] private float _spawnIncrement = 0.1f; // Amount to decrease spawn interval with each spawn

    private void Awake()
    {
        _objectPool = new ObjectPool<HandlePooling>(
            CreateObject,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            true,   // Collection check
            50,     // Default size
            100     // Increase max size if necessary
        );

    }

    private void OnEnable()
    {
        LevelEnd.OnPlayerEnterLevelEnd += LevelEnd_OnPlayerEnterLevelEnd;
    }

    private void OnDisable()
    {
        LevelEnd.OnPlayerEnterLevelEnd -= LevelEnd_OnPlayerEnterLevelEnd;
    }

    private void LevelEnd_OnPlayerEnterLevelEnd(int sceneIndex)
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        while (true)
        {
            if (_delayInitalSpawn)
            {
                yield return new WaitForSeconds(_initialSpawnDelay);
                _delayInitalSpawn = false;
            }

            if (RandomizeSpawnPosition)
            {
                RandomiseSpawnPosition();
            }
            else
            {
                _spawnPosition = FixedSpawnPosition;
            }

            HandlePooling pooledObject = _objectPool.Get();
            pooledObject.transform.position = _spawnPosition;
            pooledObject.transform.rotation = Quaternion.identity;

            // Set parent to hold the pooled objects
            if (ParentToTransform)
            {
                pooledObject.transform.SetParent(transform);
            }

            if (IncrementSpawnRate)
            {
                IncrementSpawnInterval(_spawnIncrement);
            }

            if (_randomizeSpawnInterval)
            {
                _standardSpawnInterval = Random.Range(_minSpawnInterval, _maxSpawnInterval);
            }

            yield return new WaitForSeconds(_standardSpawnInterval);
        }
    }

    private void RandomiseSpawnPosition()
    {
        // Randomize the local spawn position
        Vector3 localSpawnPosition = new Vector3(
            Random.Range(-_xSpawnRange, _xSpawnRange),
            Random.Range(-_ySpawnRange, _ySpawnRange),
            0 // Keep z at zero
        );

        // Convert local position to world position
        _spawnPosition = transform.TransformPoint(localSpawnPosition);
    }

    private void IncrementSpawnInterval(float spawnIncrement)
    {
        _standardSpawnInterval -= spawnIncrement;
        if (_standardSpawnInterval <= _minSpawnInterval)
        {
            _standardSpawnInterval = _minSpawnInterval;
        }
        // dont let the spawn interval go above the max spawn interval
        if (_standardSpawnInterval >= _maxSpawnInterval)
        {
            _standardSpawnInterval = _maxSpawnInterval;
        }
    }

    private HandlePooling CreateObject()
    {
        HandlePooling pooledObjectInstance = Instantiate(_objectPrefabs[Random.Range(0, _objectPrefabs.Length)], _spawnPosition, Quaternion.identity);
        pooledObjectInstance.ObjectPool = _objectPool;
        return pooledObjectInstance;
    }

    private void OnGetFromPool(HandlePooling pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(HandlePooling pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(HandlePooling pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}
