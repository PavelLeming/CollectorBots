using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    const int ResourcesLength = 32;

    [SerializeField] private LayerMask ResourceMask;

    private Vector3 _boxParametrs = new Vector3(10f, 1f, 10f);
    private float _scanTimer = 0.5f;
    private Collider[] _hits = new Collider[ResourcesLength];
    private Resource[] resources = new Resource[ResourcesLength];

    public event Action<Resource[], int> ResourcesScaned;

    private void Start()
    {
        StartCoroutine(ScanTimer());
    }

    private void Scan()
    {
        int hitsCount = Physics.OverlapBoxNonAlloc(transform.position, _boxParametrs, _hits, Quaternion.identity, ResourceMask);

        for (int i = 0; i < hitsCount; i++)
        {
            if (_hits[i] != null && _hits[i].TryGetComponent<Resource>(out Resource res))
            {
                resources[i] = res;
            }
        }

        for (int i = 0; i < ResourcesLength; i++)
        {
            _hits[i] = null;
        }

        if (hitsCount > 0)
        {
            ResourcesScaned?.Invoke(resources, hitsCount);
        }
    }

    private IEnumerator ScanTimer()
    {
        var wait = new WaitForSeconds(_scanTimer);

        while (enabled)
        {
            Scan();
            yield return wait;
        }
    }
}
