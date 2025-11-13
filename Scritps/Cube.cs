using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private bool _isClashed;
    private bool _isDied;
    private float _currentLifetime;
    private float _lifetimeTimer;

    public event Action<Cube> PlaneClashed;
    public event Action<Cube> Died;

    private void OnEnable()
    {
        _isClashed = false;
        _isDied = false;
        _lifetimeTimer = 0f;
        _currentLifetime = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isClashed == false)
        {
            _isClashed = true;
            PlaneClashed?.Invoke(this);
        }
    }

    private void Update()
    {
        if (_isClashed == true && _isDied == false && _currentLifetime > 0)
        {
            _lifetimeTimer += Time.deltaTime;
            if (_lifetimeTimer >= _currentLifetime)
            {
                _isDied = true;
                Died?.Invoke(this);
            }
        }
    }

    public void SetCurrentLifetime(float currentLifetime)
    {
        _currentLifetime = currentLifetime;
    }
}
