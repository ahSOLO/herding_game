using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdableTrigger : MonoBehaviour
{
    Herdable herdable;

    private void Awake()
    {
        herdable = GetComponentInParent<Herdable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            herdable.herderWithinDistance = true;
        }
        else if (other.CompareTag("Herdable"))
        {
            var herdable = other.transform.parent.GetComponent<Herdable>();
            if (!herdable.herdablesWithinDistance.ContainsKey(herdable))
            {
                herdable.herdablesWithinDistance.Add(herdable, herdable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            herdable.herderWithinDistance = false;
        }
        else if (other.CompareTag("Herdable"))
        {
            herdable.herdablesWithinDistance.Remove(other.transform.parent.GetComponent<Herdable>());
        }
    }

}
