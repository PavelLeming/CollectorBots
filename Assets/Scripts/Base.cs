using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Worker _worker;
    [SerializeField] private List<Worker> _workers;
    [SerializeField] private ResourceSorter _bases;
    [SerializeField] private BaseSpawner _baseSpavner;

    private int _resourcesCount = 0;
    private bool _isPrepareToBuild = false;
    private List<Worker> _freeWorkers = new List<Worker>();
    private int _newWorkerPrice = 3;
    private int _newBasePrice = 5;

    public int Resources => _resourcesCount;
    public List<Worker> Workers => _workers;

    private void Update()
    {
        if (_resourcesCount >= _newWorkerPrice && _isPrepareToBuild == false)
        {
            Worker worker = Instantiate(_worker);
            worker.Inicialise(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), this, _baseSpavner);
            _workers.Add(worker);
            _resourcesCount -= _newWorkerPrice;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Worker>(out Worker worker))
        {
            if (worker.IsGoHome && _workers.Contains(worker))
            {
                worker.BecomeFree();
                _freeWorkers.Add(worker);
                _resourcesCount++;
                _bases.RemoveResourceInProgres(worker.TargetResource);
            }
        }
    }

    public void Inicialise(Vector3 position, List<Worker> workers, ResourceSorter bases, Base basePrefab)
    {
        transform.position = position;
        _workers = workers;
        _bases = bases;
    }

    public bool SendWorker(Resource resource)
    {
        bool isFreeFound = false;
        foreach (Worker worker in _workers)
        {
            if (worker.IsFree)
            {
                worker.SetTarget(resource);
                _freeWorkers.Remove(worker);
                isFreeFound = true;
                break;
            }
        }
        return isFreeFound;
    }

    public void BuildBase(Vector3 flagPosition)
    {
        StartCoroutine(WaitFreeWorker(flagPosition));
    }

    private IEnumerator WaitFreeWorker(Vector3 flagPosition)
    {
        _isPrepareToBuild = true;

        yield return new WaitUntil(() => _freeWorkers.Count != 0 && _resourcesCount >= _newBasePrice);

        _freeWorkers[0].GoBuildBase(flagPosition);
        _workers.Remove(_freeWorkers[0]);
        _isPrepareToBuild = false;
    }
}
