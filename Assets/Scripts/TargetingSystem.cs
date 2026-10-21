using Unity.VisualScripting;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [Range(0.05f, 0.5f)]
    [SerializeField] private float screenLockRadius;
    [SerializeField] private float maxRange;

    [Header("Target")]
    [SerializeField] private Transform currentTarget;
    public Transform target => currentTarget;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask targetLayer;

    void Update()
    {
        Transform candidateTarget = FindTarget();


        if (candidateTarget != null)
        {
            if (currentTarget != candidateTarget)
            {
                currentTarget = candidateTarget;
            }
        }

        else
        {
            if (currentTarget != null)
            {
                ResetTarget();
            }
        }
    }

    public Transform FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxRange, targetLayer);

        Transform bestTarget = null;
        float minDistanceToCenter = float.MaxValue;

        float minDimension = Mathf.Min(Screen.width, Screen.height);
        float maxPixelRadius = screenLockRadius * minDimension;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        foreach (Collider col in colliders)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(col.transform.position);

            if (screenPos.z > 0)
            {
                Vector2 targetPixelPos = new Vector2(screenPos.x, screenPos.y);
                float distanceInPixels = Vector2.Distance(targetPixelPos, screenCenter);

                if (distanceInPixels <= maxPixelRadius)
                {
                    if (distanceInPixels < minDistanceToCenter)
                    {
                        minDistanceToCenter = distanceInPixels;
                        bestTarget = col.transform;
                    }
                }
            }
        }

        return bestTarget;
    }

    public void ResetTarget()
    {
        currentTarget = null;
    }

    private void OnGUI()
    {
        if (currentTarget == null) return;

        Vector3 vPos = mainCamera.WorldToViewportPoint(currentTarget.position);

        if (vPos.z > 0)
        {
            // Convertimos la posición 3D a coordenadas 2D de pantalla para OnGUI
            Vector2 screenPos = new Vector2(vPos.x * Screen.width, (1f - vPos.y) * Screen.height);

            Color originalColor = GUI.color;
            GUI.color = Color.red;
            float boxSize = 70f;
            Rect boxRect = new Rect(screenPos.x - (boxSize / 2f), screenPos.y - (boxSize / 2f), boxSize, boxSize);
            GUI.Box(boxRect, GUIContent.none);
            GUI.color = originalColor;
        }
    }
}
