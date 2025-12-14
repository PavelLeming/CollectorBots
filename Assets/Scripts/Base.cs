using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Worker _worker;
    [SerializeField] private List<Worker> _workers;
    [SerializeField] private Bases _bases;
    [SerializeField] private Base _basePrefab;

    private int _resourcesCount = 0;
    private bool _isClicked = false;
    private bool _isPrepareToBuild = false;
    private List<Worker> _freeWorkers = new List<Worker>();

    public int Resources => _resourcesCount;
    public List<Worker> Workers => _workers;

    private void Update()
    {
        if (_resourcesCount >= 3 && _isPrepareToBuild == false)
        {
            Worker worker = Instantiate(_worker);
            worker.Inicialise(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), _basePrefab, _bases, this);
            _workers.Add(worker);
            _resourcesCount -= 3;
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

    public void Inicialise(Vector3 position, List<Worker> workers, Bases bases, Base basePrefab)
    {
        transform.position = position;
        _workers = workers;
        _bases = bases;
        _basePrefab = basePrefab;
    }

    public void BuildBase(Vector3 flagPosition)
    {
        StartCoroutine(WaitFreeWorker(flagPosition));
    }

    public void SetWorkerBusyStatus(Worker worker)
    {
        _freeWorkers.Remove(worker);
    }

    public void Click()
    {
        if (_isClicked)
        {
            _isClicked = false;
        }
        else
        {
            _isClicked = true;
        }
    }

    private IEnumerator WaitFreeWorker(Vector3 flagPosition)
    {
        _isPrepareToBuild = true;

        yield return new WaitUntil(() => _freeWorkers.Count != 0 && _resourcesCount >= 5);

        _freeWorkers[0].GoBuildBase(flagPosition);
        _workers.Remove(_freeWorkers[0]);
        _isPrepareToBuild = false;
    }
}
