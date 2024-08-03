using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotgunScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public Camera Cam;
    [SerializeField] public float shootRate = 0.15F;
    [SerializeField] public int shootDamage = 25;
    [SerializeField] public float impactForce = 150;
    [SerializeField] private Transform Barrel;
    [SerializeField] public float reloadTime = 1.0F;
    [SerializeField] public float range = 1000F;
    [SerializeField] public int ammoCount;
    [SerializeField] public Text ammotext;
    [SerializeField] private bool canShoot;
    [SerializeField] public int numberOfProjectiles = 10;
    [SerializeField] public float sprayFactor = 0.1f;

    [Header("Decals")]
    [Tooltip("Prefab of wall damange hit. The object needs its own tag to create decal on it.")]
    [Header("DecalsPrefabs")]
    public GameObject decalHitWall;
    public GameObject decalHitSoftBody;
    public GameObject decalHitWood;
    public GameObject decalHitWoodPanel;
    public GameObject decalHitMetal;
    public GameObject decalHitMetalSparkles;
    public GameObject decalHitSand;
    public GameObject decalHitDirt;
    public GameObject decalHitGlass;
    [Tooltip("Blood prefab particle this bullet will create upoon hitting enemy")]
    public GameObject bloodEffect;
    public GameObject bloodHeadEffect;
    [Tooltip("Water Splash prefab particle this bullet will create upoon hitting water surface")]
    public GameObject WaterEffect;
    [Tooltip("Decal will need to be sligtly infront of the wall so it doesnt cause rendeing problems so for best feel put from 0.01-0.1.")]
    [Header("DecalsSettings")]
    public float floatInfrontOfWall = 0.01f;
    [Tooltip("Put Weapon layer and Player layer to ignore bullet raycast.")]
    public LayerMask ignoreLayer;

    [SerializeField] private int ammo;
    [SerializeField] private float delay;
    [SerializeField] private bool reloading;
    [SerializeField] private RaycastHit HIT;

    [Header("Effects")]
    [SerializeField] public ParticleSystem muzzleFlash;
    [SerializeField] private GameObject ShootLight;
    [SerializeField] public Transform shell;
    [SerializeField] public Transform shellEjection;
    [SerializeField] public AudioClip[] ShootSounds;
    [SerializeField] public AudioClip ReloadSound;
    [SerializeField] public AudioClip EmptySound;
    [SerializeField] public AudioSource Sound;

    [Header("Recoil")]
    [SerializeField] private Recoil Recoil_Script;
    [SerializeField] private Recoil Recoil_ScriptForWeaponCamera;

    [Header("Animator")]
    [SerializeField] public Animator animator;

    [Header("Input")]
    [SerializeField] public FP_Input playerInput;
    public GameObject ReloadButton;
    public GameObject Crosshair;
    public GameObject Ammo;
    int count;

    void DecalSpawn()
    {
        //Decals
        if (Physics.Raycast(transform.position, transform.forward, out HIT, range, ~ignoreLayer))
        {

            if (decalHitWall)
            {
                if (HIT.transform.tag == "concrete")
                {
                    Instantiate(decalHitWall, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "Untagged")
                {
                    Instantiate(decalHitWall, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "softbody")
                {
                    Instantiate(decalHitSoftBody, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "sand")
                {
                    Instantiate(decalHitSand, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "wood")
                {
                    Instantiate(decalHitWood, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "woodpanel")
                {
                    Instantiate(decalHitWoodPanel, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "dirt")
                {
                    Instantiate(decalHitDirt, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "glass")
                {
                    Instantiate(decalHitGlass, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "tile")
                {
                    Instantiate(decalHitWall, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "metal")
                {
                    Instantiate(decalHitMetal, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "metalsparkles")
                {
                    Instantiate(decalHitMetalSparkles, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "enemy")
                {
                    Instantiate(bloodEffect, HIT.point, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "enemyhead")
                {
                    Instantiate(bloodHeadEffect, HIT.point, Quaternion.LookRotation(HIT.normal));
                }
                if (HIT.transform.tag == "Water Volume")
                {
                    Instantiate(WaterEffect, HIT.point + HIT.normal * floatInfrontOfWall, Quaternion.LookRotation(HIT.normal));
                }
            }
        }
    }

    void RaycastShooting()
    {
        var bulletDirection = transform.TransformDirection(Vector3.forward);
        Vector3 rayOrigin = Cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Recoil_ScriptForWeaponCamera.RecoilFire();
        Recoil_Script.RecoilFire();
        for (int i = 0; i < numberOfProjectiles; i++)
        {

            Vector3 direction = Cam.transform.forward;
            direction += Cam.transform.up * Random.Range(-sprayFactor, sprayFactor);
            direction += Cam.transform.right * Random.Range(-sprayFactor, sprayFactor);

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, direction.normalized, out hit, range))
            {

                DecalSpawn();

                //Impact Physics
                if (HIT.collider.gameObject.GetComponent<Rigidbody>() == true)
                {
                    HIT.rigidbody.AddForceAtPosition(bulletDirection * impactForce, HIT.point);
                }

                //ActorDamage
                if (HIT.transform.TryGetComponent<Actor>(out Actor T))
                { T.TakeDamage(shootDamage); }

            }

        }

    }

    void Shoot()
    {
        if (ammoCount > 0)
        {
            Debug.Log("Shoot");
            ammoCount--;
            animator.SetTrigger("IsShooting");
            Sound.PlayOneShot(ShootSounds[UnityEngine.Random.Range(0, ShootSounds.Length)]);
            muzzleFlash.Play();
            //     Instantiate(shell, shellEjection.position, shellEjection.rotation);
            Instantiate(ShootLight, Barrel.position, Barrel.rotation);
            RaycastShooting();

        }
        else
        {
            PlayAudioEffect(EmptySound);
            Debug.Log("Empty");
        }
        delay = Time.time + shootRate;

    }

    IEnumerator Reload()
    {
        reloading = true;
        canShoot = false;
        Debug.Log("Reloading");
        PlayAudioEffect(ReloadSound);
        animator.SetBool("IsReloading", reloading);
        yield return new WaitForSeconds(reloadTime);
        ammoCount = ammo;

        Debug.Log("Reloading Complete");
        reloading = false;
        canShoot = true;
        animator.SetBool("IsReloading", reloading);
    }

    private void PlayAudioEffect(AudioClip clip)
    {
        if (clip != null)
        {
            Sound.clip = clip;
            Sound.PlayOneShot(clip);
        }
    }

    void Start()
    {
        canShoot = true;
        ammo = ammoCount;

    }

    void OnEnable()
    {
        animator.SetBool("IsReloading", false);
    }

    void Update()
    {
        ammotext.text = ammoCount.ToString();
        ReloadButton.SetActive(true);
        Crosshair.SetActive(true);
        Ammo.SetActive(true);

        if (playerInput.Shoot() && canShoot == true)                         //IF SHOOT BUTTON IS PRESSED (Replace your mouse input)
            if (Time.time > delay)
                Shoot();

        if (playerInput.Reload() || Input.GetKeyDown(KeyCode.R))                        //IF RELOAD BUTTON WAS PRESSED (Replace your keyboard input)
            if (!reloading && ammoCount < ammo)
                StartCoroutine("Reload");

    }
}
