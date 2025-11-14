using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner—oroutine : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _maxCapacity = 15;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private Vector3 _spawnAreaCenter = new Vector3(0, 10, 0);
    [SerializeField] private Vector3 _spawnAreaSize = new Vector3(10, 0, 10);

    private ObjectPool<Cube> _pool;
    private Coroutine _coroutine;
    private WaitForSeconds _wait;
    private int _index;
    private bool _isOnCounter;

    public event Action<Cube> Created;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_spawnAreaCenter, _spawnAreaSize);
    }

    public void StartSpawn()
    {
        _index = 0;
        _isOnCounter = true;
        _wait = new WaitForSeconds(_spawnInterval);

        _pool = new ObjectPool<Cube>(
            createFunc: CreateObjectForPool,
            actionOnGet: GetActionOn,
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _maxCapacity
            );

        _coroutine = StartCoroutine(CreateObject—oroutine());

    }

    public void DestroyObject(Cube cubeObject)
    {
        _pool.Release(cubeObject);
    }    

    private Cube CreateObjectForPool()
    {
        Cube cube = Instantiate(_prefab);
        cube.gameObject.name += (++_index).ToString();

        return cube;
    }

    private void GetActionOn(Cube cubeObject)
    {
        cubeObject.gameObject.SetActive(true);
        cubeObject.gameObject.transform.position = GetRandomSpawnPosition();

        Created?.Invoke(cubeObject);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPoint = new Vector3(
            UnityEngine.Random.Range(-_spawnAreaSize.x / 2, _spawnAreaSize.x / 2),
            0,
            UnityEngine.Random.Range(-_spawnAreaSize.z / 2, _spawnAreaSize.z / 2)
        );

        return _spawnAreaCenter + randomPoint;
    }

    private IEnumerator CreateObject—oroutine()
    {
        while (_isOnCounter)
        {
            _pool.Get();

            yield return _wait;
        }
    }
}