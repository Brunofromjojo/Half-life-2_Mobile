using UnityEngine;
using System.Collections;

public class DestroyObj : MonoBehaviour
{
    [SerializeField] private GameObject DeletedObj;

    void Update()
    {
        Destroy(DeletedObj, 0.1f);
    }
}
