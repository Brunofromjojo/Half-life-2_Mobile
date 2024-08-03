using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class TriggerLevel : MonoBehaviour
{
  public GameObject Object;
  public float level;
  public GUISkin MenuSkin;


  public virtual void OnTriggerEnter(Collider other)
  {
    if (!(other.GetComponent<Collider>().tag == this.Object.tag))
      return;
    SceneManager.LoadScene((int) this.level);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject.GetComponent<Collider>());
  }

  public virtual void Main()
  {
  }
}
