using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class FP_Health : MonoBehaviour
{
    [SerializeField] public int m_life = 100;
    [SerializeField] public Text healthtext;
    [SerializeField] public GameObject DeathScreen;
    [SerializeField] public AudioSource Sound;
    [SerializeField] public AudioClip WarningSound;
    [SerializeField] public AudioClip DeathSound;
    [SerializeField] public float WaitBeforeRespawn;
    [SerializeField] public bool Godmode;

    void Awake()
    {
        DeathScreen.SetActive(false);
    }

    void Update()
    {
        healthtext.text = m_life.ToString();
    }

    //Reduce the player’s life and update the UI interface
    public void OnDamage(int damage)
    {
        if (Godmode == false)
        {
            m_life -= damage;

            if (m_life < 50)
            {
                PlayAudioEffect(WarningSound);
            }

            // If hp is 0, unlock mouse display
            if (m_life <= 0)
            {
                Cursor.visible = false;
                StartCoroutine(PlayerDeath());
            }
        }
    }

    IEnumerator PlayerDeath()
    {
        DeathScreen.SetActive(true);
        PlayAudioEffect(DeathSound);
        yield return new WaitForSeconds(WaitBeforeRespawn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
