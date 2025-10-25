using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private float _yPostition = 0.5f;
    private bool _isFree = true;

    public event Action<Resource> ReadyForRelease;
    public bool IsFree => _isFree;

    public void Release()
    {
        ReadyForRelease?.Invoke(this);
    }

    public void Initialize(float xPosition, float zPosition)
    {
        transform.position = new Vector3(xPosition, _yPostition, zPosition);
        _isFree = true;
    }

    public void GetBusy()
    {
        _isFree = false;
    }
}
