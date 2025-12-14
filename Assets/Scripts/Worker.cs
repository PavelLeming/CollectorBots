using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    private Base _basePrefab;
    private Bases _bases;
    private bool _isFree = true;
    private bool _isGoHome = false;
    private bool _isGoBuild = false;
    private Vector3 _target;
    private Resource _targetResorce;
    private float _speed = 5f;

    public Resource TargetResource => _targetResorce;
    public bool IsFree => _isFree;
    public bool IsGoHome => _isGoHome;

    private void Update()
    {
        if (IsFree == false)
        {
            if (_isGoHome == false)
            {
                _target = _targetResorce.transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        }

        if (_isGoBuild)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);

            if (Mathf.Abs(transform.position.x - _target.x) < 0.01 && Mathf.Abs(transform.position.z - _target.z) < 0.01)
            {
                Base newBase = Instantiate(_basePrefab);
                newBase.Inicialise(_target, new List<Worker>{this}, _bases, _basePrefab);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Resource>(out Resource resource))
        {
            if (resource == _targetResorce)
            {
                _target = new Vector3(0, 1, 0);
                _isGoHome = true;
                resource.transform.SetParent(transform);
            }
        }
    }

    public void Inicialise(Vector3 position, Base basePrefab, Bases bases)
    {
        transform.position = position;
        _basePrefab = basePrefab;
        _bases = bases;
    }

    public void SetTarget(Resource resource)
    {
        _isFree = false;
        _target = resource.transform.position;
        _targetResorce = resource;
    }

    public void BecomeFree()
    {
        _isFree = true;
        _isGoHome = false;
        _targetResorce.transform.SetParent(null);
        _targetResorce.Release();
    }

    public void GoBuildBase(Vector3 flagPosition)
    {
        _isGoBuild = true;
        _target = flagPosition;
    }
}
