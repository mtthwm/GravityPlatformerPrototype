using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public Gun activeGun;


    public void Shoot ()
    {
        activeGun.Shoot();
    }
}
