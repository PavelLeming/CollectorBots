using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    [SerializeField] private Base _motherBase;
    [SerializeField] private BaseSpawner _baseSpawner;

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
                _isGoBuild = false;
                _motherBase = _baseSpawner.BuildNewBase(_target, this);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Resource>(out Resource resource))
        {
            if (resource == _targetResorce)
            {
                _target = _motherBase.transform.position;
                _isGoHome = true;
                resource.transform.SetParent(transform);
            }
        }
    }

    public void Inicialise(Vector3 position, Base motherBase, BaseSpawner baseSpawner)
    {
        transform.position = position;
        _motherBase = motherBase;
        _baseSpawner = baseSpawner;
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
