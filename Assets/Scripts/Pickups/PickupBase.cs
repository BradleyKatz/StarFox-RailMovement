using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupBase : MonoBehaviour
{
    public float rotationSpeed = 120.0f;
    protected bool isGrabbed = false;
    protected Collider triggerCollider;

    protected abstract void OnPickupGrabbedAnimation(PlayerManager player);
    protected abstract void OnPickupGrabbedEffect(PlayerManager player);

    protected void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        if (triggerCollider == null)
        {
            Debug.LogWarning(gameObject.name + " does not have a collider, the pickup won't function without it");
        }
        else if (!triggerCollider.isTrigger)
        {
            Debug.LogWarning(gameObject.name + "'s collider is not a trigger, pickup objects are expected to have trigger colliders");
            triggerCollider.isTrigger = true;
        }
    }

    protected void Update()
    {
        if (!isGrabbed)
        {
            transform.eulerAngles += new Vector3(0.0f, rotationSpeed, 0.0f) * Time.deltaTime;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();
        if (player)
        {
            isGrabbed = true;
            triggerCollider.enabled = false;

            OnPickupGrabbedAnimation(player);
            OnPickupGrabbedEffect(player);
        }
    }
}
