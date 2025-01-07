using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private WaitForSeconds _waitForSeconds;
    private ObjectPool<Cube> _pool;
    private BoxCollider _platformCollider;

    private void Awake()
    {
        _pool= new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => CreateCube(obj),
            actionOnRelease: (obj) => TurningOffCube(obj),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );

        _waitForSeconds = new WaitForSeconds(_repeatRate);
        _platformCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private void CreateCube(Cube obj)
    {
        obj.LifeTimeOver+=PutCubeInPool;

        Vector3 platformSize = _platformCollider.size;

        float randomX = Random.Range(-platformSize.x / 2, platformSize.x / 2);
        float randomZ = Random.Range(-platformSize.z / 2, platformSize.z / 2);
        float positionY = -2f;
        Vector3 randomPosition = new Vector3(randomX, positionY, randomZ);

        obj.transform.position = transform.position+randomPosition;
        obj.gameObject.SetActive(true);
    }

    private void TurningOffCube(Cube obj)
    {
        obj.LifeTimeOver -= PutCubeInPool;
        obj.gameObject.SetActive(false);
    }

    private IEnumerator SpawnCubes()
    {
        while (true)
        {
            if (_pool.CountActive<_poolMaxSize)
                _pool.Get();

            yield return _waitForSeconds;
        }
    }

    public void PutCubeInPool(Cube cube)
    {
        _pool.Release(cube);
    }
}