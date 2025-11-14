using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private bool _isClashed;
    private Coroutine _coroutine;
    private WaitForSeconds _wait;

    public event Action<Cube> PlaneClashed;
    public event Action<Cube> Died;

    private void OnEnable()
    {
        _isClashed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isClashed == false && collision.gameObject.TryGetComponent<Plane>(out _))
        {
            _isClashed = true;
            PlaneClashed?.Invoke(this);
        }
    }

    public void SetCurrentLifetime(float currentLifetime)
    {
        _wait = new WaitForSeconds(currentLifetime);
        _coroutine = StartCoroutine(LiveCurrentLife());
    }

    private IEnumerator LiveCurrentLife()
    {
        yield return _wait;

        Died?.Invoke(this);
    }
}
