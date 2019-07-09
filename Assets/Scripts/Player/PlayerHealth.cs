using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("The number of lives the player has left")]
    public int numLives = 1;

    [Range(1, 10), Tooltip("The maximum amount of health the player has at the start of a level")]
    public float baseMaxHealth;
    [Range(1, 20), Tooltip("The maximum amount of health the player has after collected enough gold rings")]
    public float enhancedMaxHealth;

    [Range(0, 5), Tooltip("The amount of gold rings the player needs to collect before their health meter gets upgraded")]
    public int numGoldRingsBeforeHealthUpgrade = 3;

    protected int numGoldRingsCollected;
    protected float currentHealth;
    protected float currentMaxHealth;

    protected bool isHealthUpgraded = false;

    // TODO Signal UI System to reflect changes in health meter and lives
    public void IncrementGoldRings()
    {
        if (++numGoldRingsCollected >= numGoldRingsBeforeHealthUpgrade)
        {
            if (isHealthUpgraded)
            {
                ++numLives;
                numGoldRingsCollected = 0;

                Debug.Log("You got an extra life!");
            }
            else
            {
                isHealthUpgraded = true;
                currentMaxHealth = enhancedMaxHealth;
                currentHealth = currentMaxHealth;
                numGoldRingsCollected = 0;

                Debug.Log("You got a health upgrade!");
            }
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0.0f, currentMaxHealth);

        Debug.Log("You recovered " + healAmount + " health!");
    }

    public void OnDamageTaken(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0.0f)
        {
            Debug.Log("PLAYER IS DEAD");
            // TODO Restart from last checkpoint and decrement lives

            --numLives;
        }
    }

    protected void Start()
    {
        currentMaxHealth = baseMaxHealth;
        currentHealth = currentMaxHealth;
    }

    protected void Update()
    {
        
    }
}
