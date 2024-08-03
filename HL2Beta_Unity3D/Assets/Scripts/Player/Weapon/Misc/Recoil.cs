using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Recoil Rotation")]
    [SerializeField] private Vector3 currentRotation;
    [SerializeField] private Vector3 targetRotation;

    [Header("Recoil Value")]
    [SerializeField] private float recolX;
    [SerializeField] private float recolY;
    [SerializeField] private float recolZ;

    [Header("Recoil Settings")]
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;


    public void RecoilFire()
    {
            targetRotation += new Vector3(recolX, Random.Range(-recolY, recolY), Random.Range(-recolZ, recolZ));
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
}
