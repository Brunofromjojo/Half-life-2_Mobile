using System.Collections;
using UnityEngine;

public class SwayNBobScript : MonoBehaviour
{
    public CharacterController mover;

    [Header("Sway")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayEulerRot;

    public float smooth = 10f;
    float smoothRot = 12f;

    [Header("Bobbing")]
    public float speedCurve;
    float curveSin => Mathf.Sin(speedCurve);
    float curveCos => Mathf.Cos(speedCurve);

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    Vector3 bobPosition;
    public float bobExaggeration;

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;

    [Header("Mobile Input")]
    [SerializeField] public FP_CameraLook playerLook;

    void Update()
    {
        GetInput();
        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();
        CompositePositionRotation();
    }

    Vector2 walkInput;
    Vector2 lookInput;

    void GetInput()
    {
        walkInput = new Vector2(mover.velocity.x, mover.velocity.z).normalized;
        lookInput = new Vector2(playerLook.InputX, playerLook.InputY);
    }

    void Sway()
    {
        Vector3 invertLook = lookInput * -step;
        swayPos = new Vector3(
            Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance),
            Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance),
            0
        );
    }

    void SwayRotation()
    {
        Vector2 invertLook = lookInput * -rotationStep;
        swayEulerRot = new Vector3(
            Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep),
            Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep),
            invertLook.x
        );
    }

    void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    void BobOffset()
    {
        speedCurve += Time.deltaTime * (mover.isGrounded ? (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical")) * bobExaggeration : 1f) + 0.01f;
        bobPosition = new Vector3(
            (curveCos * bobLimit.x * (mover.isGrounded ? 1 : 0)) - (walkInput.x * travelLimit.x),
            (curveSin * bobLimit.y) - (Input.GetAxis("Vertical") * travelLimit.y),
            -(walkInput.y * travelLimit.z)
        );
    }

    void BobRotation()
    {
        bobEulerRotation = new Vector3(
            (walkInput != Vector2.zero ? multiplier.x * Mathf.Sin(2 * speedCurve) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2)),
            (walkInput != Vector2.zero ? multiplier.y * curveCos : 0),
            (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0)
        );
    }
}
