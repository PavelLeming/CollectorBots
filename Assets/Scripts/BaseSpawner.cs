using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private ResourceSorter _bases;
    [SerializeField] private Base _basePrefab;

    public Base BuildNewBase(Vector3 target, Worker worker)
    {
        Base newBase = Instantiate(_basePrefab);
        newBase.Inicialise(target, new List<Worker> { worker }, _bases, _basePrefab);
        _bases.AddBase(newBase);
        return newBase;
    }
}
