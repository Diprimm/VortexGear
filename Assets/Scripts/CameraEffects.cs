using UnityEngine;
using Unity.Cinemachine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private ShipController shipController;

    public Material speedScreenShaderMat;

    public float normalFOV;
    public float boostFOV;
    public float FOVlerpSpeed;

    public float boostNoiseAmplitude;
    public float noiseTransitionSpeed;

    private CinemachineBasicMultiChannelPerlin noiseComponent;

    public Rigidbody playerRb;

    private void Awake()
    {
        noiseComponent = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {
        normalFOV = 60f;
    }

    
    void Update()
    {
        float fov;
        float targetAmplitude;

        if (shipController.IsBoosting)
        {
            fov = boostFOV;
            targetAmplitude = boostNoiseAmplitude;
        }

        else
        {
            fov = normalFOV;
            targetAmplitude = 0f;
        }


        //FOV
        float currentFOV = virtualCamera.Lens.FieldOfView;

        if (Mathf.Abs(currentFOV - fov) > 0.01f)
        {
            virtualCamera.Lens.FieldOfView = Mathf.Lerp(currentFOV, fov, Time.deltaTime * FOVlerpSpeed);
        }

        else
        {
            virtualCamera.Lens.FieldOfView = fov;
        }


        //Shake
        float currentAmplitude = noiseComponent.AmplitudeGain;

        if (Mathf.Abs(currentAmplitude - targetAmplitude) > 0.001f)
        {
            noiseComponent.AmplitudeGain = Mathf.Lerp(noiseComponent.AmplitudeGain, targetAmplitude, Time.deltaTime * noiseTransitionSpeed);
        }

        else
        {
            noiseComponent.AmplitudeGain = targetAmplitude;
        }

        SpeedEffect();
    }

    public void SpeedEffect()
    {
        float speedFactor = Mathf.InverseLerp(0, 40, playerRb.linearVelocity.magnitude);
        float speedFactorFinal = Mathf.Lerp(0, 12, speedFactor);
        speedScreenShaderMat.SetFloat("_Intensity", speedFactorFinal);
    }

    private void OnDisable()
    {
        virtualCamera.Lens.FieldOfView = normalFOV;
        noiseComponent.AmplitudeGain = 0f;
    }
}
