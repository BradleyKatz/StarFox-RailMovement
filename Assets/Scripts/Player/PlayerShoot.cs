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
    [Range(0.0f, 300.0f)]
    public float laserRange = 150.0f;
    [SerializeField] protected Transform singleRayOrigin = null;
    [SerializeField] protected Transform doubleRayOriginLeft = null;
    [SerializeField] protected Transform doubleRayOriginRight = null;

    public Color normalLaserColor = Color.green;
    public Color plasmaLaserColor = Color.blue;

    [Range(0.0f, 1.0f), Tooltip("The amount of time it takes to fire a laser")]
    public float rateOfFireDelay = 0.3f;
    protected float rofDelayElapsed = 0.0f;

    protected LineRenderer centerLaserLineRenderer = null;
    protected LineRenderer leftLaserLineRenderer = null;
    protected LineRenderer rightLaserLineRenderer = null;

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

        if (singleRayOrigin)
        {
            centerLaserLineRenderer = singleRayOrigin.GetComponent<LineRenderer>();
        }

        if (doubleRayOriginLeft)
        {
            leftLaserLineRenderer = doubleRayOriginLeft.GetComponent<LineRenderer>();
        }

        if (doubleRayOriginRight)
        {
            rightLaserLineRenderer = doubleRayOriginRight.GetComponent<LineRenderer>();
        }
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
        else if (rofDelayElapsed > 0.0f)
        {
            centerLaserLineRenderer.enabled = false;
            leftLaserLineRenderer.enabled = false;
            rightLaserLineRenderer.enabled = false;
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
                    centerLaserLineRenderer.enabled = true;
                    centerLaserLineRenderer.startColor = normalLaserColor;
                    centerLaserLineRenderer.endColor = normalLaserColor;
                    centerLaserLineRenderer.SetPosition(0, singleRayOrigin.position);

                    Ray shootRay = new Ray();
                    shootRay.origin = singleRayOrigin.position;
                    shootRay.direction = singleRayOrigin.forward;

                    if (Physics.Raycast(shootRay, out RaycastHit hit, laserRange, shootableMask))
                    {
                        centerLaserLineRenderer.SetPosition(1, hit.point);
                    }
                    else
                    {
                        centerLaserLineRenderer.SetPosition(1, shootRay.origin + shootRay.direction * laserRange);
                    }
                }
                break;
            case ELaserType.Double:
                {
                    // Left Laser
                    {
                        leftLaserLineRenderer.enabled = true;
                        leftLaserLineRenderer.startColor = normalLaserColor;
                        leftLaserLineRenderer.endColor = normalLaserColor;
                        leftLaserLineRenderer.SetPosition(0, doubleRayOriginLeft.position);

                        Ray shootRay = new Ray();
                        shootRay.origin = doubleRayOriginLeft.position;
                        shootRay.direction = doubleRayOriginLeft.forward;

                        if (Physics.Raycast(shootRay, out RaycastHit hit, laserRange, shootableMask))
                        {
                            leftLaserLineRenderer.SetPosition(1, hit.point);
                        }
                        else
                        {
                            leftLaserLineRenderer.SetPosition(1, shootRay.origin + shootRay.direction * laserRange);
                        }
                    }

                    // Right Laser
                    {
                        rightLaserLineRenderer.enabled = true;
                        rightLaserLineRenderer.startColor = normalLaserColor;
                        rightLaserLineRenderer.endColor = normalLaserColor;
                        rightLaserLineRenderer.SetPosition(0, doubleRayOriginRight.position);

                        Ray shootRay = new Ray();
                        shootRay.origin = doubleRayOriginRight.position;
                        shootRay.direction = doubleRayOriginRight.forward;

                        if (Physics.Raycast(shootRay, out RaycastHit hit, laserRange, shootableMask))
                        {
                            rightLaserLineRenderer.SetPosition(1, hit.point);
                        }
                        else
                        {
                            rightLaserLineRenderer.SetPosition(1, shootRay.origin + shootRay.direction * laserRange);
                        }
                    }
                }
                break;
            case ELaserType.DoublePlasma:
                {
                    // Left Laser
                    {
                        leftLaserLineRenderer.enabled = true;
                        leftLaserLineRenderer.startColor = plasmaLaserColor;
                        leftLaserLineRenderer.endColor = plasmaLaserColor;
                        leftLaserLineRenderer.SetPosition(0, doubleRayOriginLeft.position);

                        Ray shootRay = new Ray();
                        shootRay.origin = doubleRayOriginLeft.position;
                        shootRay.direction = doubleRayOriginLeft.forward;

                        if (Physics.Raycast(shootRay, out RaycastHit hit, laserRange, shootableMask))
                        {
                            leftLaserLineRenderer.SetPosition(1, hit.point);
                        }
                        else
                        {
                            leftLaserLineRenderer.SetPosition(1, shootRay.origin + shootRay.direction * laserRange);
                        }
                    }

                    // Right Laser
                    {
                        rightLaserLineRenderer.enabled = true;
                        rightLaserLineRenderer.startColor = plasmaLaserColor;
                        rightLaserLineRenderer.endColor = plasmaLaserColor;
                        rightLaserLineRenderer.SetPosition(0, doubleRayOriginRight.position);

                        Ray shootRay = new Ray();
                        shootRay.origin = doubleRayOriginRight.position;
                        shootRay.direction = doubleRayOriginRight.forward;

                        if (Physics.Raycast(shootRay, out RaycastHit hit, laserRange, shootableMask))
                        {
                            rightLaserLineRenderer.SetPosition(1, hit.point);
                        }
                        else
                        {
                            rightLaserLineRenderer.SetPosition(1, shootRay.origin + shootRay.direction * laserRange);
                        }
                    }
                }
                break;
        }
    }
}
