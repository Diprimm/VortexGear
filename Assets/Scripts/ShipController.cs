using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using System.Collections;
using System;
using Unity.VisualScripting;

public class ShipController : MonoBehaviour
{
    [Header("Ship config")]
    [SerializeField] private float boostForce;
    [SerializeField] private float forwardForce;
    [SerializeField] private float baseForwardForce;
    [SerializeField] private float strafeForce;
    [SerializeField] private float rollForce;
    [SerializeField] private float elevateForce;
    [SerializeField] private float acrobaticsForce;
    [SerializeField] private float pitchTorqueForce;
    [SerializeField] private float yawTorqueForce;
    [SerializeField] private float rollAngle;
    [SerializeField] private float rollDuration;
    [SerializeField] private float rollSpeed;
    [SerializeField] private AnimationCurve rollEasingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve returnEasingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float rollDurationReturn;

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private Transform shipModel;

    [Header("InputsValues (Read only)")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 lookInput;
    [SerializeField] private float rollInput;
    [SerializeField] private float elevateInput;
    [SerializeField] private bool boostInput;
    [SerializeField] private bool isRolling;
    public bool IsBoosting => boostInput;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        rollInput = context.ReadValue<float>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnElevate(InputAction.CallbackContext context)
    {
        elevateInput = context.ReadValue<float>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            impulseSource.GenerateImpulseWithForce(0.1f);
            boostInput = true;
            rb.AddForce(transform.forward * 18000 * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        else if (context.canceled)
        {
            boostInput = false;
        }
    }

    public void OnAcrobatics(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AcrobaticsMovement();
        }       
    }


    public void ShipTranslation()
    {
        Vector3 forward = transform.forward * baseForwardForce;

        if (boostInput)
        {
            forward += transform.forward * boostForce;
        }

        else
        {
            forward += transform.forward * moveInput.y * forwardForce;
        }

        
        Vector3 finalForce = forward + (transform.right * moveInput.x * strafeForce) + (transform.up * elevateInput * elevateForce);
        rb.AddForce(finalForce * Time.fixedDeltaTime, ForceMode.Force);
    }

    public void ShipRotation()
    {
        Vector3 finalTorque = (transform.right * -lookInput.y * pitchTorqueForce) + (transform.up * lookInput.x * yawTorqueForce) + (transform.forward * rollInput * rollForce);
        rb.AddTorque(finalTorque * Time.fixedDeltaTime, ForceMode.Force);
    }

    public void AcrobaticsMovement()
    {
        if (isRolling) return;

        float direction = Mathf.Sign(moveInput.x);

        Vector3 dashVector = transform.right * direction;

        Vector3 lateralVelocity = Vector3.Project(rb.linearVelocity, transform.right);
        rb.linearVelocity -= lateralVelocity;

        rb.AddForce(dashVector * acrobaticsForce, ForceMode.Impulse);

        StartCoroutine(RollCorroutine(-direction));
    }
    
    public IEnumerator RollCorroutine(float direction)
    {
        //To block input movement during roll
        isRolling = true;

        //1 phase duration is 40% of the total duration of the roll
        float rollTime = rollDuration * 0.4f;
        float elapsed = 0f;
        float previousEasedAngle = 0f;

        Quaternion originalRotation = shipModel.localRotation;

        // - Phase1 -
        while (elapsed < rollTime)
        {         
            elapsed += Time.fixedDeltaTime;

            // "t" represents the progress of time in a normalized value
            float t = Mathf.Clamp01(elapsed / rollTime);

            float easedT = rollEasingCurve.Evaluate(t);
            float easedAngle = easedT * rollAngle;

            //"step" is how much is should advance in this specific frame.
            //Calculated by subtracting the angle already applied in the previous frame
            //from the current angle.
            float step = easedAngle - previousEasedAngle;

            //Here we apply the rotation calculated.
            shipModel.localRotation *= Quaternion.Euler(0, 0, step * direction);

            previousEasedAngle = easedAngle;
            yield return new WaitForFixedUpdate();
        }


        // - Phase2 - (Correction. May be optional)

        //If the angle is not multiple of 360 degrees, we secure a smooth transition back to
        //original rotation.
        if (rollAngle % 360f != 0f)
        {
            Quaternion rotBeforeReturn = shipModel.localRotation;
            float returnTime = rollDurationReturn;
            float t2 = 0f;
            elapsed = 0f;

            while (elapsed < returnTime)
            {
                elapsed += Time.fixedDeltaTime;
                t2 = Mathf.Clamp01(elapsed / returnTime);
                float eased = returnEasingCurve.Evaluate(t2);

                //Interpolation with Slerp between current rotation and original rotation.
                shipModel.localRotation = Quaternion.Slerp(rotBeforeReturn, originalRotation, eased);
                yield return new WaitForFixedUpdate();
            }
            shipModel.localRotation = originalRotation;
        }


        // - Phase3 -
        //We clean the rotation
        float remaining = rollAngle - previousEasedAngle;
        shipModel.localRotation *= Quaternion.Euler(0, 0, remaining * direction);

        isRolling = false;
    }

    private void FixedUpdate()
    {
        ShipTranslation();
        ShipRotation();
    }
}
