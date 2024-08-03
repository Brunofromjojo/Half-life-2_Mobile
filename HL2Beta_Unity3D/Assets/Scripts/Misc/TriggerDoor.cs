

using System;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
[Serializable]
public class TriggerDoor : MonoBehaviour
{
  public GameObject Object;
  public AnimationClip anim_open;
  public AudioClip sound_open;

  public virtual void OnTriggerEnter(Collider other)
  {
    if (!(other.GetComponent<Collider>().tag == this.Object.tag))
      return;
    this.GetComponent<AudioSource>().PlayOneShot(this.sound_open);
    this.GetComponent<Animation>().CrossFade(this.anim_open.name);
    this.GetComponent<Animation>().clip = this.anim_open;
    this.GetComponent<Animation>().Play();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject.GetComponent<Collider>());
  }

  public virtual void Main()
  {
  }
}
