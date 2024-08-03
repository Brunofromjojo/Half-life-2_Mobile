using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] public FP_Input playerInput;
    [SerializeField] public GameObject ReloadButton;
    [SerializeField] public GameObject PistolCrosshair;
    [SerializeField] public GameObject Ammo;

    [Header("Animator")]
    [SerializeField] public Animator animator;

    [Header("Settings")]
    [Header("Attacking")]
    [SerializeField] public float attackDistance = 2.5f;
    [SerializeField] public float attackDelay = 0.4f;
    [SerializeField] public float attackSpeed = 1f;
    [SerializeField] public int attackDamage = 25;
    [SerializeField] public float impactForce = 150;

    [Header("Decals")]
    [Header("DecalsPrefabs")]
    [SerializeField] public GameObject decalHitWall;
    [SerializeField] public GameObject decalHitWood;
    [SerializeField] public GameObject decalHitWoodPanel;
    [SerializeField] public GameObject decalHitMetal;
    [SerializeField] public GameObject decalHitMetalSparkles;
    [SerializeField] public GameObject decalHitSand;
    [SerializeField] public GameObject decalHitDirt;
    [SerializeField] public GameObject decalHitGlass;
    [SerializeField] public GameObject bloodEffect;
    [SerializeField] public GameObject bloodHeadEffect;
    [SerializeField] public GameObject WaterEffect;
    [SerializeField] public GameObject decalHitSoftBody;
    [Header("DecalsSettings")]
    [SerializeField] public float floatInfrontOfWall = 0.01f; 
    [SerializeField] public Camera Cam;
    [SerializeField] public LayerMask ignoreLayer;

    private RaycastHit HIT;

    [SerializeField] bool attacking = false;
    [SerializeField] bool readyToAttack = true;
    [SerializeField] public AudioSource Sound;
    [SerializeField] public AudioClip[] HitSounds;
    [SerializeField] public AudioClip[] MissSounds;


    private void AttackAnimation()
    {

        if (HIT.collider != null)
        {
            animator.SetTrigger("Hit");
        }
        if (HIT.collider == null)
        {
            animator.SetTrigger("Miss");
        }
    }

    public void Attack()
    {
        if (!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);


        Sound.pitch = Random.Range(0.9f, 1.1f);
        Sound.PlayOneShot(MissSounds[UnityEngine.Random.Range(0, MissSounds.Length)]);
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {

        AttackAnimation();

        if (Physics.Raycast(transform.position, transform.forward, out HIT, attackDistance, ~ignoreLayer))
        {
            HitTarget(HIT.point);

            if (HIT.transform.TryGetComponent<Actor>(out Actor T))
            { T.TakeDamage(attackDamage); }
        }
    }

    void DecalSpawn()
    {

        if (HIT.transform.tag == "concrete")
        {
            var concreteHole = Instantiate(decalHitWall, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
            concreteHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
            concreteHole.transform.parent = HIT.transform;
            Sound.pitch = 1;
            Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
        if (HIT.transform.tag == "Untagged")
        {
            var DefaultHole = Instantiate(decalHitWall, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
            DefaultHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
            DefaultHole.transform.parent = HIT.transform;
            Sound.pitch = 1;
            Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
        if (HIT.transform.tag == "sand")
        {
            var sandHole = Instantiate(decalHitSand, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
            sandHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
            sandHole.transform.parent = HIT.transform;
            Sound.pitch = 1;
            Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
        if (HIT.transform.tag == "wood")
        {
            var woodHole = Instantiate(decalHitWood, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
            woodHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
            woodHole.transform.parent = HIT.transform;
            Sound.pitch = 1;
            Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);

        }
        if (HIT.transform.tag == "woodpanel")
        {
            var woodpanelHole = Instantiate(decalHitWoodPanel, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
            woodpanelHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
            woodpanelHole.transform.parent = HIT.transform;
            Sound.pitch = 1;
            Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
        if (HIT.transform.tag == "dirt")
        {
            var dirtHole = Instantiate(decalHitDirt, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
            dirtHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
            dirtHole.transform.parent = HIT.transform;
            Sound.pitch = 1;
            Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
        if (HIT.transform.tag == "glass")
        {
            var glassHole = Instantiate(decalHitGlass, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
            glassHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
            glassHole.transform.parent = HIT.transform;
            Sound.pitch = 1;
            Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
        if (HIT.transform.tag == "tile")
        {
            var tileHole = Instantiate(decalHitWall, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
            tileHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
            tileHole.transform.parent = HIT.transform;
            Sound.pitch = 1;
            Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
        if (HIT.transform.tag == "metal")
        {
            var metalHole = Instantiate(decalHitMetal, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
            metalHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
            metalHole.transform.parent = HIT.transform;
            Sound.pitch = 1;
            Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
        if (HIT.transform.tag == "metalsparkles")
        {
            var metalsparklesHole = Instantiate(decalHitMetalSparkles, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
            metalsparklesHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
            metalsparklesHole.transform.parent = HIT.transform;
            Sound.pitch = 1;
            Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
        if (HIT.transform.tag == "enemy")
        {
            Instantiate(bloodEffect, HIT.point, Quaternion.LookRotation(HIT.normal));
         //   Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
        if (HIT.transform.tag == "enemyhead")
        {
            Instantiate(bloodHeadEffect, HIT.point, Quaternion.LookRotation(HIT.normal));
          //  Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
        if (HIT.transform.tag == "Water Volume")
        {
            Instantiate(WaterEffect, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
        }
        if (HIT.transform.tag == "softbody")
        {
            var softbodyHole = Instantiate(decalHitSoftBody, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
            softbodyHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
            softbodyHole.transform.parent = HIT.transform;
            Sound.pitch = 1;
            Sound.PlayOneShot(HitSounds[UnityEngine.Random.Range(0, HitSounds.Length)]);
        }
    }

    void HitTarget(Vector3 pos)
    {

        var HitDirection = transform.TransformDirection(Vector3.forward);

            DecalSpawn();

        //Apply impact force
        if (HIT.collider.gameObject.GetComponent<Rigidbody>() == true)
            {
                HIT.rigidbody.AddForceAtPosition(HitDirection * impactForce, HIT.point);
            }
    }

    void Update()
    {     
        ReloadButton.SetActive(false);
        PistolCrosshair.SetActive(false);
        Ammo.SetActive(false);
        if (playerInput.UseMobileInput == true)
        {
            if (playerInput.Shoot())
            {
                Attack();
            }
        }
        if (playerInput.UseMobileInput == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }
    }
}
