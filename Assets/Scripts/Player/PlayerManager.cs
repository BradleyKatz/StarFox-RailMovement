using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerManager : MonoBehaviour
{
    protected PlayerMovement playerMovement = null;
    protected PlayerHealth playerHealth = null;

    protected void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void Heal(float healAmount)
    {
        playerHealth.Heal(healAmount);
    }

    public void IncrementGoldRings()
    {
        playerHealth.IncrementGoldRings();
    }
}
