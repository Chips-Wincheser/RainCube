using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Painter : MonoBehaviour
{
    private Renderer _cubeRenderer;
    private Color _color;

    private void Awake()
    {
        _color =new Color(Random.value, Random.value, Random.value);
        _cubeRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out Platform platform))
        {
            _cubeRenderer.material.color = _color;
        }
    }

    private void OnDisable()
    {
        _cubeRenderer.material.color = Color.white;
    }
}
