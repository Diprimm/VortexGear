using Unity.VisualScripting;
using UnityEngine;

public class EngineEffect : MonoBehaviour
{
    public Transform coneTransform;
    public Rigidbody rb;

    private void Awake()
    {
        coneTransform = gameObject.transform;
    }
    private void Update()
    {
        float magnitude = Mathf.InverseLerp(0, 45, rb.linearVelocity.magnitude);
        coneTransform.localScale = new Vector3(coneTransform.localScale.x, magnitude, coneTransform.localScale.z);
    }

}
