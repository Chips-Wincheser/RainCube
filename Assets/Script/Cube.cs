using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Platform _platform;
    private WaitForSeconds _waitForSeconds;

    private bool _didCollision=false;
    private int _lifeTime;
    private Color _color;

    private void Awake()
    {
        int maxRandomValue = 5;
        int minRandomValue = 2;

        _lifeTime= Random.Range(minRandomValue, maxRandomValue);
        _color =Color.red;
        _waitForSeconds = new WaitForSeconds(_lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _platform =collision.gameObject.GetComponent<Platform>();

        if (_platform!=null && _didCollision== false)
        {
            Paint();
            StartCoroutine(DisableAfterDelay(gameObject));
            _didCollision = true;
        }
    }

    private void Paint()
    {
        Renderer _cubeRenderer = GetComponent<Renderer>();
        _cubeRenderer.material.color = _color;
    }

    private IEnumerator DisableAfterDelay(GameObject cube)
    {
        yield return _waitForSeconds;

        Destroy(cube);
    }
}
