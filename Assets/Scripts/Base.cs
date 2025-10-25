using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private LayerMask ResourceMask;
    [SerializeField] private List<Worker> _workers; 

    private Vector3 _boxParametrs = new Vector3(9f, 0, 9f);
    private float _scanTimer = 0.5f;

    private void Start()
    {
        StartCoroutine(ScanTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Worker>().GetFree();
    }

    private void Scan()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, _boxParametrs, Quaternion.identity, ResourceMask);

        foreach (Collider hit in hits)
        {
            Resource resource = hit.GetComponent<Resource>();
            if(resource.IsFree)
            {
                foreach (Worker worker in _workers)
                {
                    if (worker.IsFree)
                    {
                        worker.GetTarget(resource);
                        resource.GetBusy();
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator ScanTimer()
    {
        var wait = new WaitForSeconds(_scanTimer);

        while (enabled)
        {
            Scan();
            yield return wait;
        }
    }
}
