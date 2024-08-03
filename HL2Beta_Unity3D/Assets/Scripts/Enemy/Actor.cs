using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] public GameObject DeadObj;
    [SerializeField] public AudioSource Sound;
    [SerializeField] public AudioClip DamageSound;
    [SerializeField] public AudioClip DeathSound;
    public bool IsDead;
    public bool IsDamageTaken;

    void Awake()
    {
        currentHealth = maxHealth;
        IsDead= false;
    }

    private void PlayAudioEffect(AudioClip clip)
    {
        if (clip != null)
        {
            Sound.clip = clip;
            Sound.PlayOneShot(clip);
        }
    }

    public void TakeDamage(int amount)
    {
        IsDamageTaken= true;
        currentHealth -= amount;
        PlayAudioEffect(DamageSound);

        if (currentHealth <= 0)
        { Death(); }
    }

    void Death()
    {
        // Death function
        // TEMPORARY: Destroy Object
        PlayAudioEffect(DeathSound);
        IsDead = true;
        Destroy(gameObject);
        Instantiate(DeadObj, transform.position, transform.rotation);
    }
}
