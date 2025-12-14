using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _object;

    private int _poolCapacity = 10;
    private int _maxPoolCapacity = 10;
    private float _minX = -9f;
    private float _maxX = 9f;
    private float _minZ = -9f;
    private float _maxZ = 9f;
    private float _spawnTimer = 1f;

    protected ObjectPool<Resource> Objects;

    private void Awake()
    {
        Objects = new ObjectPool<Resource>(
            createFunc: () => Instantiate(_object),
            actionOnGet: (poolableObject) => GetAction(poolableObject),
            actionOnRelease: (poolableObject) => poolableObject.gameObject.SetActive(false),
            actionOnDestroy: (poolableObject) => Destroy(poolableObject.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _maxPoolCapacity
            );
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private void GetAction(Resource resource)
    {
        resource.gameObject.SetActive(true);
        resource.Initialize(Random.Range(_minX, _maxX), Random.Range(_minZ, _maxZ));
        resource.ReadyForRelease += Release;
    }

    protected void Release(Resource resource)
    {
        resource.ReadyForRelease -= Release;
        Objects.Release(resource);
    }

    private IEnumerator Spawn()
    {
        var wait = new WaitForSeconds(_spawnTimer);

        while (enabled)
        {
            Objects.Get();
            yield return wait;
        }
    }
}
