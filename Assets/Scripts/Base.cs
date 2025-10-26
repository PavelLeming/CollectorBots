using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Worker> _workers;
    [SerializeField] private Scaner _scaner;

    public event Action NewResource;

    private int _resourcesCount = 0;
    private List<Resource> _resources = new List<Resource>();
    private List<Resource> _resourcesInProgress = new List<Resource>();

    public int Resources => _resourcesCount;

    private void OnEnable()
    {
        _scaner.ResourcesScaned += SortResources;
    }

    private void OnDisable()
    {
        _scaner.ResourcesScaned -= SortResources;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Worker>(out Worker worker))
        {
            if (worker.IsGoHome)
            {
                worker.BecomeFree();
                _resourcesCount++;
                NewResource?.Invoke();
                _resourcesInProgress.Remove(worker.TargetResource);
            }
        }
    }

    private void SortResources(Resource[] resources, int count)
    {
        for (int i = 0; i < count; i++) 
        {
            if (_resources.Find(sample => sample == resources[i]) == null && 
                _resourcesInProgress.Find(sample => sample == resources[i]) == null)
            {
                _resources.Add(resources[i]);
            }
        }

        SendWorkers();
    }

    private void SendWorkers()
    {
        foreach (Worker worker in _workers)
        {
            if (_resources.Count > 0 && worker.IsFree)
            {
                worker.GetTarget(_resources[0]);
                _resourcesInProgress.Add(_resources[0]);
                _resources.RemoveAt(0);
            }
        }
    }
}
