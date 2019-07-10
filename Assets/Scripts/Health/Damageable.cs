using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Range(1.0f, 50.0f)]
    public float maxHealth;
    protected float currentHealth;

    protected void Start()
    {
        currentHealth = maxHealth;   
    }

    public void OnDamageTaken(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0.0f)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(this.gameObject);
    }
}
