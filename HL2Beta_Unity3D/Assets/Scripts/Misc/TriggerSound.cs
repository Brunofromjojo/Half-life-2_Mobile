

using System;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
[Serializable]
public class TriggerSound : MonoBehaviour
{
  public GameObject Object;
  [SerializeField] public AudioSource QSound;


    public virtual void OnTriggerEnter(Collider other)
  {
    if (!(other.GetComponent<Collider>().tag == this.Object.tag))
      return;
        QSound.Play();
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject.GetComponent<Collider>());
  }

  public virtual void Main()
  {
  }
}
