using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    protected enum ELaserType
    {
        Normal,
        Double,
        DoublePlasma,
    }
    protected ELaserType currentLaserType = ELaserType.Normal;

    [Header("Laser Prefab References")]
    [SerializeField] protected GameObject normalLaser = null;
    [SerializeField] protected GameObject doubleLaser = null;
    [SerializeField] protected GameObject doublePlasmaLaser = null;

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
    }

    protected void LaunchBomb()
    {
        if (bombPrefab)
        {
            --numBombs;

            // TODO Actually spawn bomb
        }
    }
}
