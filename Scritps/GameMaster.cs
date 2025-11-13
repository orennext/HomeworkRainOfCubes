using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _minTimeLife = 2f;
    [SerializeField] private float _maxTimeLife = 5f;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private ColorChanger _colorChanger;

    private float _spawnTimer = 0f;

    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;

            Cube cube = _spawner.CreateCube();
            cube.PlaneClashed += RunDestructionCube;
            cube.Died += DestructionCube;
        }
    }

    private void RunDestructionCube(Cube cubeObject)
    {
        float currentLifetime = Random.Range(_minTimeLife, _maxTimeLife);

        cubeObject.SetCurrentLifetime(currentLifetime);
        _colorChanger.SelectRandomColor(cubeObject);
    }

    private void DestructionCube(Cube cube)
    {
        cube.PlaneClashed -= RunDestructionCube;
        cube.Died -= DestructionCube;

        _spawner.DestroyCubeObject(cube);
    }
}
