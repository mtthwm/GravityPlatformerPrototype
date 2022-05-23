using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public List<Gun> primaryGuns;
    public List<Gun> secondaryGuns;

    private int activePrimary = 0;
    private int activeSecondary = 0;


    public void BeginShootPrimary()
    {
        primaryGuns[activePrimary].BeginShoot();
    }

    public void EndShootPrimary()
    {
        primaryGuns[activePrimary].EndShoot();
    }

    public void BeginShootSecondary()
    {
        secondaryGuns[activeSecondary].BeginShoot();
    }

    public void EndShootSecondary()
    {
        secondaryGuns[activeSecondary].EndShoot();
    }
}
