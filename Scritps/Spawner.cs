using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefabCube;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _maxCapacity = 15;
    [SerializeField] private Vector3 _spawnAreaCenter = new Vector3(0, 10, 0);
    [SerializeField] private Vector3 _spawnAreaSize = new Vector3(10, 0, 10);

    private ObjectPool<Cube> _pool;
    private int indexCubes;

    private void Awake()
    {
        indexCubes = 0;

        _pool = new ObjectPool<Cube>(
            createFunc: CreateCubesForPool,
            actionOnGet: ActionOnGet,
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _maxCapacity
            );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_spawnAreaCenter, _spawnAreaSize);
    }

    public Cube CreateCube()
    {
        Cube cube = _pool.Get();

        return cube;
    }

    public void DestroyCubeObject(Cube cubeObject)
    {
        _pool.Release(cubeObject);
    }

    private void ActionOnGet(Cube cubeObject)
    {
        cubeObject.gameObject.SetActive(true);
        cubeObject.gameObject.transform.position = GetRandomSpawnPosition();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(-_spawnAreaSize.x / 2, _spawnAreaSize.x / 2),
            0,
            Random.Range(-_spawnAreaSize.z / 2, _spawnAreaSize.z / 2)
        );

        return _spawnAreaCenter + randomPoint;
    }

    private Cube CreateCubesForPool()
    {
        Cube cube = Instantiate(_prefabCube);
        cube.gameObject.name += (++indexCubes).ToString();

        return cube;
    }
}
