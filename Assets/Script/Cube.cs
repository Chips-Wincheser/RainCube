using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    public event Action<Cube> LifeTimeOver;

    private WaitForSeconds _waitForSeconds;
    private Renderer _cubeRenderer;

    private bool _didCollision=false;
    private int _lifeTime;
    private Color _color;

    private void Awake()
    {
        _color =Color.red;
        _cubeRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Platform platform =collision.gameObject.GetComponent<Platform>();

        if (platform!=null && _didCollision== false)
        {
            SetTimeLife();
            Paint();
            StartCoroutine(DisableAfterDelay());
            _didCollision = true;
        }
    }

    private void Paint()
    {
        _cubeRenderer.material.color = _color;
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

    private void OnDisable()
    {
        _cubeRenderer.material.color = Color.white;
        _didCollision = false;
    }
}
