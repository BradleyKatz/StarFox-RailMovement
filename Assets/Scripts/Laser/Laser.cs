using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Range(0.0f, 10.0f), Tooltip("The maximum amount of time the laser travels before it destroys itself")]
    public float maxLifetime = 2.0f;
    [Range(0.0f, 100.0f), Tooltip("The speed at which the laser travels")]
    public float velocity = 20.0f;

    [HideInInspector] public float damage = 5.0f;
    [HideInInspector] public Transform parent;
    [HideInInspector] public Color laserColor;

    protected TrailRenderer trailRenderer;
    protected float lifetimeElapsed = 0.0f;

    protected void Start()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        if (trailRenderer)
        {
            trailRenderer.startColor = laserColor;
            trailRenderer.endColor = laserColor;
        }
    }

    protected void Update()
    {
        lifetimeElapsed += Time.deltaTime;
        if (lifetimeElapsed >= maxLifetime)
        {
            Destroy(this.gameObject);
        }

        transform.localPosition += transform.forward * velocity * Time.deltaTime;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.transform != parent)
        {
            Damageable damageable = other.gameObject.GetComponent<Damageable>();
            if (damageable)
            {
                damageable.OnDamageTaken(damage);
            }

            //Destroy(this.gameObject);
        }
    }
}
