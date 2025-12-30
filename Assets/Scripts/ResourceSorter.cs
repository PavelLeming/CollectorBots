using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSorter : MonoBehaviour
{
    [SerializeField] private List<Base> _bases = new List<Base>();

    private List<Resource> _resources = new List<Resource>();
    private List<Resource> _resourcesInProgress = new List<Resource>();

    [SerializeField] private Scaner _scaner;

    private void OnEnable()
    {
        _scaner.ResourcesScaned += SortResources;
    }

    private void OnDisable()
    {
        _scaner.ResourcesScaned -= SortResources;
    }

    private void SortResources(Resource[] resources, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_resources.Contains(resources[i]) == false &&
                _resourcesInProgress.Contains(resources[i]) == false)
            {
                _resources.Add(resources[i]);
            }
        }

        SendWorkers();
    }

    private void SendWorkers()
    {
        foreach (Base oneBase in _bases)
        {
            if (_resources.Count > 0)
            {
                if (oneBase.SendWorker(_resources[0]))
                {
                    _resourcesInProgress.Add(_resources[0]);
                    _resources.RemoveAt(0);
                }
            }
        }
    }

    public void RemoveResourceInProgres(Resource resource)
    {
        _resourcesInProgress.Remove(resource);
    }

    public void AddBase(Base newBase)
    {
        _bases.Add(newBase);
    }
}
