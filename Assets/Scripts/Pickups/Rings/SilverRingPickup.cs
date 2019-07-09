using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverRingPickup : RingPickupBase
{
    [Range(0.0f, 10.0f), Tooltip("The amount of health this ring replenishes when collected")]
    public float healAmount = 5.0f;

    protected override void OnPickupGrabbedEffect(PlayerManager player)
    {
        player.Heal(healAmount);
    }
}
