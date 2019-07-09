using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerShoot))]
public class PlayerManager : MonoBehaviour
{
    protected PlayerMovement playerMovement = null;
    protected PlayerHealth playerHealth = null;
    protected PlayerShoot playerShoot = null;

    protected void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        playerShoot = GetComponent<PlayerShoot>();
    }

    public void Heal(float healAmount)
    {
        playerHealth.Heal(healAmount);
    }

    public void IncrementGoldRings()
    {
        playerHealth.IncrementGoldRings();
    }

    public void IncrementBombs()
    {
        playerShoot.IncrementBombs();
    }

    public void UpdateLaserType()
    {
        playerShoot.UpdateLaserType();
    }
}
