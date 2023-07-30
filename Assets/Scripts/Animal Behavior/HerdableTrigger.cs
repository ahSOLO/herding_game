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
        if (other.GetComponent<HostileTrigger>() != null)
        {
            return;
        }

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
        else if (other.CompareTag("Hostile"))
        {
            var hostile = other.transform.parent.GetComponent<Hostile>();
            if (!herdable.hostilesWithinDistance.ContainsKey(hostile))
            {
                herdable.hostilesWithinDistance.Add(hostile, hostile);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HostileTrigger>() != null)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            herdable.herderWithinDistance = false;
        }
        else if (other.CompareTag("Herdable"))
        {
            herdable.herdablesWithinDistance.Remove(other.transform.parent.GetComponent<Herdable>());
        }
        else if (other.CompareTag("Hostile"))
        {
            herdable.hostilesWithinDistance.Remove(other.transform.parent.GetComponent<Hostile>());
        }
    }

}
