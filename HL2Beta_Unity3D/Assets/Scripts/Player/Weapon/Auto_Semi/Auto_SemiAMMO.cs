using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto_SemiAMMO : MonoBehaviour
{
    public GameObject Weapon;
    private WeaponScript ammo;
    public AudioClip pickupSound;

    // Use this for initialization
    void Start()
    {
        ammo = Weapon.GetComponent<WeaponScript>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && ammo.spareAmmo < ammo.maxSpareAmmo)
        {
            GetComponent<AudioSource>().PlayOneShot(pickupSound);
            ammo.spareAmmo++;
            Destroy(this.gameObject, pickupSound.length);
        }

    }
}
