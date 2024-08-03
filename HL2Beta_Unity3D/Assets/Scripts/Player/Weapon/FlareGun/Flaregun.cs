using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Flaregun : MonoBehaviour {
	
    [Header("Sounds")]
    [SerializeField] public AudioSource Sound;
    [SerializeField] public AudioClip ShotSound;
    [SerializeField] public AudioClip EmptySound;
    [SerializeField] public AudioClip reloadSound;

    [Header("Settings")]
    [SerializeField] public Rigidbody flareBullet;
    [SerializeField] public Transform barrelEnd;
    [SerializeField] public GameObject muzzleParticles;
    [SerializeField] public Text ammotext;
    [SerializeField] public Text roundtext;
    [SerializeField] public int bulletSpeed = 2000;
    [SerializeField] public int maxSpareRounds = 25;
    [SerializeField] public int spareRounds = 1;
    [SerializeField] public int currentRound = 0;
    [SerializeField] public float shootRate = 0.15F;
    [SerializeField] public float reloadTime = 1.0F;
    [SerializeField] public bool canShoot = true;
    [SerializeField] public bool reloading;
    [SerializeField] private float delay;

    [Header("Animator")]
    [SerializeField] public Animator animator;

    [Header("Recoil")]
    [SerializeField] private Recoil Recoil_Script;
    [SerializeField] private Recoil Recoil_ScriptForWeaponCamera;

    [Header("Input")]
    [SerializeField] public FP_Input playerInput;
    [SerializeField] public GameObject ReloadButton;
    [SerializeField] public GameObject Crosshair;
    [SerializeField] public GameObject Ammo;

    void Update () 
	{		
		if(playerInput.Shoot())
		{
            if (Time.time > delay)
            {
                Shoot();
            }
		}
		if(playerInput.Reload())
		{
            if (!reloading && currentRound<1)
            {
                StartCoroutine("Reload");
            }

        }

        roundtext.text = spareRounds.ToString();
        ammotext.text = currentRound.ToString();
        ReloadButton.SetActive(true);
        Crosshair.SetActive(true);
        Ammo.SetActive(true);
    }
	
	void Shoot()
	{
        if (currentRound > 0 && canShoot == true)
        {
            currentRound--;
            if (currentRound <= 0)
            {
                currentRound = 0;
            }

            Debug.Log("Shoot");
            animator.SetTrigger("Shoot");
            Recoil_ScriptForWeaponCamera.RecoilFire();
            Recoil_Script.RecoilFire();
            PlayAudioEffect(ShotSound);

            Rigidbody bulletInstance;
            bulletInstance = Instantiate(flareBullet, barrelEnd.position, barrelEnd.rotation) as Rigidbody; //INSTANTIATING THE FLARE PROJECTILE

            bulletInstance.AddForce(barrelEnd.forward * bulletSpeed); //ADDING FORWARD FORCE TO THE FLARE PROJECTILE

            Instantiate(muzzleParticles, barrelEnd.position, barrelEnd.rotation);   //INSTANTIATING THE GUN'S MUZZLE SPARKS	
        }
        else
        {
            Debug.Log("Empty");
            animator.SetTrigger("EmptyShoot");
            PlayAudioEffect(EmptySound);
        }

        delay = Time.time + shootRate;
    }

    IEnumerator Reload()
	{
		if(spareRounds >= 1 && currentRound < 1){
            reloading = true;
            canShoot = false;		
            Debug.Log("Reloading");
            animator.SetBool("Reload", true);
            PlayAudioEffect(reloadSound);
            yield return new WaitForSeconds(reloadTime);
            animator.SetBool("Reload", false);
            spareRounds--;
            currentRound++;
            Debug.Log("Reloading Complete");
            reloading = false;
            canShoot = true;
        }
		
	}

    private void PlayAudioEffect(AudioClip clip)
    {
        if (clip != null)
        {
            Sound.clip = clip;
            Sound.PlayOneShot(clip);
        }
    }

}
