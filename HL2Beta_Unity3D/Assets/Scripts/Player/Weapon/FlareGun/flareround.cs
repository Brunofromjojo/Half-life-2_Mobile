using UnityEngine;
using System.Collections;

public class flareround : MonoBehaviour {
	public Flaregun flare;
	public AudioClip pickupSound;
    public AudioSource Sound;

    void OnTriggerEnter(Collider other)
	{
		
		if(other.tag == "Player" && flare.spareRounds < flare.maxSpareRounds)
		{
			Sound.PlayOneShot(pickupSound);			
			flare.spareRounds++;
			Destroy(this.gameObject,pickupSound.length);				
		}
		
	}
}

