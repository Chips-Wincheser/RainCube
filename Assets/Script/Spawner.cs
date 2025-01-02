using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _StartPoint;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize=5;

    private ObjectPool<GameObject> _pool;
    private BoxCollider platformCollider;

    private void Awake()
    {
        _pool= new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );
    }

    private void ActionOnGet(GameObject obj)
    {
        platformCollider = _StartPoint.GetComponent<BoxCollider>();
        Vector3 platformSize = platformCollider.size;

        float randomX = Random.Range(-platformSize.x / 2, platformSize.x / 2);
        float randomZ = Random.Range(-platformSize.z / 2, platformSize.z / 2);
        float positionY = -2f;
        Vector3 randomPosition = new Vector3(randomX, positionY, randomZ);

        obj.transform.position = _StartPoint.transform.position+randomPosition;
        obj.GetComponent<Rigidbody>().velocity =Vector3.zero;
        obj.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetSphere), 0.0f, _repeatRate);
    }

    private void GetSphere()
    {
        _pool.Get();
    }

    private void OnTriggerEnter(Collider other)
    {
        _pool.Release(other.gameObject);
    }
}
