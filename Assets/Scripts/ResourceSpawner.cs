using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _object;

    private int _poolCapacity = 10;
    private int _maxPoolCapacity = 10;
    private float _maxXNegative = -3f;
    private float _minXPositive = 3f;
    private float _maxZNegative = -3f;
    private float _minZPositive = 3f;
    private float _minXPosition = -9f;
    private float _maxXPosition = 9f;
    private float _minZPosition = -9f;
    private float _maxZPosition = 9f;
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
        resource.Initialize(RandomingInRange(_minXPosition, _maxXPosition, _minXPositive, _maxXNegative), 
                            RandomingInRange(_minZPosition, _maxZPosition, _minZPositive, _maxZNegative));
        resource.ReadyForRelease += Release;
    }

    private float RandomingInRange(float min, float max, float minPositive, float maxNegative)
    {
        float resoult;
        do
        {
            resoult = Random.Range(min, max);
        }
        while (resoult < minPositive && resoult > maxNegative);

        return resoult;
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
