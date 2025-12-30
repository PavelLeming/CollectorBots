using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Flag _flag;
    [SerializeField] private LayerMask _baseMask;
    [SerializeField] private LayerMask _planeMask;

    private Base _clickedBase;
    private Flag _spawnedFlag;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit baseHit, Mathf.Infinity, _baseMask))
            {
                if (baseHit.collider.TryGetComponent<Base>(out Base hitedBase))
                {
                    if (_clickedBase == null)
                    {
                        _clickedBase = hitedBase;
                    }
                    else if (_clickedBase != hitedBase)
                    {
                        _clickedBase = hitedBase;
                    }
                }
            }
            else if (Physics.Raycast(ray, out RaycastHit planeHit, Mathf.Infinity, _planeMask))
            {
                if (_clickedBase != null)
                {
                    _spawnedFlag = Instantiate(_flag);
                    _spawnedFlag.transform.position = new Vector3(planeHit.point.x, planeHit.point.y + 1, planeHit.point.z);
                    _clickedBase.BuildBase(planeHit.point);
                    _clickedBase = null;
                }
            }
        }
    }
}
