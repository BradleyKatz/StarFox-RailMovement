using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRingPickup : RingPickupBase
{
    protected override void OnPickupGrabbedEffect(PlayerManager player)
    {
        player.IncrementGoldRings();
    }
}
