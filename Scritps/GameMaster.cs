using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private float _minTimeLife = 2f;
    [SerializeField] private float _maxTimeLife = 5f;
    [SerializeField] private ColorChanger _colorChanger;
    [SerializeField] private Spawner—oroutine _spawner;

    private void OnEnable()
    {
        _spawner.Created += SubscribeToEvent;
        _spawner.StartSpawn();
    }

    private void SubscribeToEvent(Cube cube)
    {
        cube.PlaneClashed += RunDestructionCube;
        cube.Died += DestructionCube;
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

        _spawner.DestroyObject(cube);
    }
}
