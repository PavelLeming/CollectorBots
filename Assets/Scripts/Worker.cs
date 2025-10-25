using UnityEngine;

public class Worker : MonoBehaviour
{
    private bool _isFree = true;
    private bool _isGoHome = false;
    private Vector3 _target;
    private Resource _targetResorce;
    private float _speed = 5f;

    public bool IsFree => _isFree;

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

    public void GetTarget(Resource resource)
    {
        _isFree = false;
        _target = resource.transform.position;
        _targetResorce = resource;
    }

    public void GetFree()
    {
        _isFree = true;
        _isGoHome = false;
        _targetResorce.transform.SetParent(null);
        _targetResorce.Release();
    }
}
