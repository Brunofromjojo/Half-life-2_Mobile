using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FP_Flashlight : MonoBehaviour
{
    // Public variables
    [SerializeField] public AudioClip turnOnSound;
    [SerializeField] public AudioClip turnOffSound;
    [SerializeField] public FP_Input playerInput;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public KeyCode control = KeyCode.F;
    [SerializeField] public GameObject flashlight;
    [SerializeField] private bool LightActive;


    void Start()
    {
         LightActive= false;    
    }

    void Update()
    {
        if (Input.GetKeyDown(control) || playerInput.Flashlight())
        {
            LightActive = !LightActive;
            flashlight.SetActive(LightActive);

            if (flashlight.activeSelf)
            {
                PlayAudioEffect(turnOnSound);
            }
            else
            {
                PlayAudioEffect(turnOffSound);
            }

        }
        

    }

    private void PlayAudioEffect(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

}
