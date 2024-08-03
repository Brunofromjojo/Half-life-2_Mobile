using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponScript : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] public Camera Cam;
    [SerializeField] public float shootRate = 0.15F;
    [SerializeField] public int shootDamage = 25;
    [SerializeField] public float impactForce = 150;
    [SerializeField] private Transform Barrel;
    [SerializeField] public float reloadTime = 1.0F;
    [SerializeField] public float range = 1000F;
    [SerializeField] public int maxSpareAmmo = 150;
    [SerializeField] public int spareAmmo = 36;
    [SerializeField] public int ammoCount;
    [SerializeField] public Text ammotext;
    [SerializeField] public Text roundtext;
    [SerializeField] private bool canShoot;

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
    bool IsEmpty;

    void DecalSpawn()
    {
        //Decals
        if (Physics.Raycast(transform.position, transform.forward, out HIT, range, ~ignoreLayer))
        {

            if (decalHitWall)
            {
                if (HIT.transform.tag == "concrete")
                {
                    var concreteHole = Instantiate(decalHitWall, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
                    concreteHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
                    concreteHole.transform.parent = HIT.transform;
                }
                if (HIT.transform.tag == "Untagged")
                {
                    var DefaultHole = Instantiate(decalHitWall, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
                    DefaultHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
                    DefaultHole.transform.parent = HIT.transform;
                }
                if (HIT.transform.tag == "softbody")
                {
                    var softbodyHole = Instantiate(decalHitSoftBody, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
                    softbodyHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
                    softbodyHole.transform.parent = HIT.transform;
                }
                if (HIT.transform.tag == "sand")
                {
                    var sandHole = Instantiate(decalHitSand, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
                    sandHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
                    sandHole.transform.parent = HIT.transform;
                }
                if (HIT.transform.tag == "wood")
                {
                    var woodHole = Instantiate(decalHitWood, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
                    woodHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
                    woodHole.transform.parent = HIT.transform;
                }
                if (HIT.transform.tag == "woodpanel")
                {
                    var woodpanelHole = Instantiate(decalHitWoodPanel, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
                    woodpanelHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
                    woodpanelHole.transform.parent = HIT.transform;
                }
                if (HIT.transform.tag == "dirt")
                {
                    var dirtHole = Instantiate(decalHitDirt, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
                    dirtHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
                    dirtHole.transform.parent = HIT.transform;
                }
                if (HIT.transform.tag == "glass")
                {
                    var glassHole = Instantiate(decalHitGlass, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
                    glassHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
                    glassHole.transform.parent = HIT.transform;
                }
                if (HIT.transform.tag == "tile")
                {
                    var tileHole = Instantiate(decalHitWall, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
                    tileHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
                    tileHole.transform.parent = HIT.transform;
                }
                if (HIT.transform.tag == "metal")
                {
                    var metalHole = Instantiate(decalHitMetal, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
                    metalHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
                    metalHole.transform.parent = HIT.transform;
                }
                if (HIT.transform.tag == "metalsparkles")
                {
                    var metalsparklesHole = Instantiate(decalHitMetalSparkles, HIT.point, Quaternion.LookRotation(HIT.normal)) as GameObject;
                    metalsparklesHole.transform.localPosition += floatInfrontOfWall * HIT.normal;
                    metalsparklesHole.transform.parent = HIT.transform;
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
        Recoil_ScriptForWeaponCamera.RecoilFire();
        Recoil_Script.RecoilFire();
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out HIT, range))
            {

            DecalSpawn();

            //ActorDamage
            if (HIT.transform.TryGetComponent<Actor>(out Actor T))
            { T.TakeDamage(shootDamage); }

            var bulletDirection = transform.TransformDirection(Vector3.forward);
            //Impact Physics
            if (HIT.collider.gameObject.GetComponent<Rigidbody>() == true)
            {
                    HIT.rigidbody.AddForceAtPosition(bulletDirection * impactForce, HIT.point);
            }


        }

    }

    void Shoot()
    {
        if (ammoCount > 0)
        {
            IsEmpty= false;
            Debug.Log("Shoot");
            ammoCount--;
            animator.SetTrigger("IsShooting");
            Sound.PlayOneShot(ShootSounds[UnityEngine.Random.Range(0, ShootSounds.Length)]);
            muzzleFlash.Play();
            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            Instantiate(ShootLight, Barrel.position, Barrel.rotation);

        }
        else
        {
            IsEmpty= true;
            PlayAudioEffect(EmptySound);
            animator.SetTrigger("IsShootingEmpty");
            Debug.Log("Empty");
        }
            delay = Time.time + shootRate;

    }

        IEnumerator Reload()
        {
        if (spareAmmo >= 1 && ammoCount < ammo)
        {
            reloading = true;
            canShoot = false;
            Debug.Log("Reloading");
            PlayAudioEffect(ReloadSound);
            animator.SetBool("IsReloading", reloading);
            yield return new WaitForSeconds(reloadTime);
            ammoCount = ammo;
            spareAmmo--;
            Debug.Log("Reloading Complete");
            reloading = false;
            canShoot = true;
            animator.SetBool("IsReloading", reloading);
        }
        }

    IEnumerator DebugWait()
    {
        yield return new WaitForSeconds(1);
        canShoot=true;
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
        if (ammoCount > 0)
        {
            IsEmpty = false;
        }
        ammo = ammoCount;
        
        }

        void OnEnable()
        {
            animator.SetBool("IsReloading", false);
        }

        void Update()
        {
            ammotext.text = ammoCount.ToString();
            roundtext.text = spareAmmo.ToString();
        ReloadButton.SetActive(true);
        Crosshair.SetActive(true);
        Ammo.SetActive(true);

        if (playerInput.UseMobileInput == true)
        {

            if (playerInput.Shoot() && canShoot == true)                         //IF SHOOT BUTTON IS PRESSED (Replace your mouse input)
                if (Time.time > delay)
                {
                    Shoot();
                    if (IsEmpty == false)
                    {
                        RaycastShooting();
                    }
                }
        }
        if (playerInput.UseMobileInput == false)
        {

            if (Input.GetMouseButtonDown(0) && canShoot == true)                         //IF SHOOT BUTTON IS PRESSED (Replace your mouse input)
                if (Time.time > delay)
                {
                    Shoot();
                    if (IsEmpty == false)
                    {
                        RaycastShooting();
                    }
                }
        }

        if (playerInput.Reload() || Input.GetKeyDown(KeyCode.R))                        //IF RELOAD BUTTON WAS PRESSED (Replace your keyboard input)
                if (!reloading && ammoCount < ammo)
                    StartCoroutine("Reload");

        }
    }
