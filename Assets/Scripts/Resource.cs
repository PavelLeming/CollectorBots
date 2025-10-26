using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private float _yPostition = 0.5f;

    public event Action<Resource> ReadyForRelease;

    public void Release()
    {
        ReadyForRelease?.Invoke(this);
    }

    public void Initialize(float xPosition, float zPosition)
    {
        transform.position = new Vector3(xPosition, _yPostition, zPosition);
    }
}
