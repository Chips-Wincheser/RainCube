using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    public event Action<Cube> LifeTimeOver;
    private WaitForSeconds _waitForSeconds;

    private bool _haveCollision=false;
    private int _lifeTime;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out Platform platform) && _haveCollision== false)
        {
            SetTimeLife();
            StartCoroutine(DisableAfterDelay());
            _haveCollision = true;
        }
    }

    private void OnDisable()
    {
        _haveCollision = false;
    }

    private void SetTimeLife()
    {
        int maxRandomValue = 5;
        int minRandomValue = 2;
        _lifeTime= UnityEngine.Random.Range(minRandomValue, maxRandomValue);
        _waitForSeconds = new WaitForSeconds(_lifeTime);
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return _waitForSeconds;
        
        LifeTimeOver?.Invoke(this);
    }
}
