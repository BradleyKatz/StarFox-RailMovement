using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public LayerMask shootableMask;

    protected enum ELaserType
    {
        Normal,
        Double,
        DoublePlasma,
    }
    protected ELaserType currentLaserType = ELaserType.Normal;

    [Header("Laser Properties")]
    [Range(0.0f, 30.0f)]
    public float baseLaserDamage = 5.0f;
    [Range(0.0f, 30.0f)]
    public float plasmaLaserDamage = 10.0f;

    [SerializeField] protected Laser laserPrefab = null;
    [SerializeField] protected Transform singleRayOrigin = null;
    [SerializeField] protected Transform doubleRayOriginLeft = null;
    [SerializeField] protected Transform doubleRayOriginRight = null;

    public Color normalLaserColor = Color.green;
    public Color plasmaLaserColor = Color.blue;

    [Range(0.0f, 1.0f), Tooltip("The amount of time it takes to fire a laser")]
    public float rateOfFireDelay = 0.3f;
    protected float rofDelayElapsed = 0.0f;

    [Header("Bomb Prefab Reference")]
    [SerializeField] protected GameObject bombPrefab = null;

    [Space]

    [Header("Bomb Properties")]
    [Tooltip("The current number of bombs the player has at their disposal")]
    public int numBombs;
    [Range(1, 5), Tooltip("The maxmimum number of bombs the player may carry at a time")]
    public int maxBombs = 3;

    public void IncrementBombs()
    {
        numBombs = (numBombs + 1 > maxBombs) ? numBombs + 1 : numBombs;
    }

    public void UpdateLaserType()
    {
        if (currentLaserType < ELaserType.DoublePlasma) 
        { 
            ++currentLaserType;
            Debug.Log("Current Laser Type: " + currentLaserType); 
        }
    }

    protected void Start()
    {
        numBombs = maxBombs;
    }

    protected void Update()
    {
        if (Input.GetButtonDown("Bomb") && numBombs > 0)
        {
            LaunchBomb();
        }

        rofDelayElapsed = Mathf.Max(rofDelayElapsed - Time.deltaTime, 0.0f);
        if (Input.GetButtonDown("Shoot") && rofDelayElapsed <= 0.0f)
        {
            ShootLaser();
        }
    }

    protected void LaunchBomb()
    {
        if (bombPrefab)
        {
            --numBombs;

            // TODO Actually spawn bomb
        }
    }

    protected void ShootLaser()
    {
        rofDelayElapsed = rateOfFireDelay;

        switch (currentLaserType)
        {
            case ELaserType.Normal:
                {
                    Laser normalLaser = Instantiate(laserPrefab, singleRayOrigin.position, Quaternion.identity);
                    normalLaser.parent = transform;
                    normalLaser.damage = baseLaserDamage;
                    normalLaser.transform.rotation = Quaternion.LookRotation(singleRayOrigin.forward, Vector3.up);
                    normalLaser.laserColor = normalLaserColor;
                }
                break;
            case ELaserType.Double:
                {
                    Laser doubleLaserLeft = Instantiate(laserPrefab, doubleRayOriginLeft.position, Quaternion.identity);
                    doubleLaserLeft.parent = transform;
                    doubleLaserLeft.damage = baseLaserDamage;
                    doubleLaserLeft.transform.rotation = Quaternion.LookRotation(doubleRayOriginLeft.forward, Vector3.up);
                    doubleLaserLeft.laserColor = normalLaserColor;

                    Laser doubleLaserRight = Instantiate(laserPrefab, doubleRayOriginRight.position, Quaternion.identity);
                    doubleLaserRight.parent = transform;
                    doubleLaserRight.damage = baseLaserDamage;
                    doubleLaserRight.transform.rotation = Quaternion.LookRotation(doubleRayOriginRight.forward, Vector3.up);
                    doubleLaserRight.laserColor = normalLaserColor; ;
                }
                break;
            case ELaserType.DoublePlasma:
                {
                    Laser doubleLaserLeft = Instantiate(laserPrefab, doubleRayOriginLeft.position, Quaternion.identity);
                    doubleLaserLeft.parent = transform;
                    doubleLaserLeft.damage = plasmaLaserDamage;
                    doubleLaserLeft.transform.rotation = Quaternion.LookRotation(doubleRayOriginLeft.forward, Vector3.up);
                    doubleLaserLeft.laserColor = plasmaLaserColor;

                    Laser doubleLaserRight = Instantiate(laserPrefab, doubleRayOriginRight.position, Quaternion.identity);
                    doubleLaserRight.parent = transform;
                    doubleLaserRight.damage = plasmaLaserDamage;
                    doubleLaserRight.transform.rotation = Quaternion.LookRotation(doubleRayOriginRight.forward, Vector3.up);
                    doubleLaserRight.laserColor = plasmaLaserColor;
                }
                break;
        }
    }
}
