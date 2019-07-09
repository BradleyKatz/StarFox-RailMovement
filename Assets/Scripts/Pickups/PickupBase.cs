using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupBase : MonoBehaviour
{
    public float rotationSpeed = 120.0f;
    protected bool isGrabbed = false;

    protected abstract void OnPickupGrabbedAnimation(PlayerManager player);
    protected abstract void OnPickupGrabbedEffect(PlayerManager player);

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
            OnPickupGrabbedAnimation(player);
            OnPickupGrabbedEffect(player);
        }
    }
}
