using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileTrigger : MonoBehaviour
{
    Hostile hostile;
    enum TriggerType { Inner, Outer };
    [SerializeField] private TriggerType type;

    private void Awake()
    {
        hostile = GetComponentInParent<Hostile>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (type)
        {
            case TriggerType.Outer:
                if (other.CompareTag("Herdable"))
                {
                    var herdable = other.transform.parent.GetComponent<Herdable>();
                    if (!hostile.herdablesWithinDistance.ContainsKey(herdable))
                    {
                        hostile.herdablesWithinDistance.Add(herdable, herdable);
                    }
                }
                break;
            case TriggerType.Inner:
                if (other.CompareTag("Player"))
                {
                    hostile.herderWithinDistance = true;
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (type)
        {
            case TriggerType.Outer:
                if (other.CompareTag("Herdable"))
                {
                    hostile.herdablesWithinDistance.Remove(other.transform.parent.GetComponent<Herdable>());
                }
                else if (other.CompareTag("Player"))
                {
                    hostile.herderWithinDistance = false;
                }
                break;
            case TriggerType.Inner:
                break;
            default:
                break;
        }
    }
}
