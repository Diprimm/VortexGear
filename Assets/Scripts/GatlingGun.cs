using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GatlingGun : MonoBehaviour
{
    [Header("Config")]
    
    public Transform[] gatlingSpawnPoint;    
    public float bulletSpeed;
    [SerializeField] private float _delay = 0.2f;  
    public float maxOverheat;
    public float overheatCooldown;
    [SerializeField] private float overheatCoolRate;

    [Header("References")]
    public TargetingSystem targetingSystem;
    private Transform gatlingTarget;
    public GameObject bullet;

    [Header("Read Only")]
    [SerializeField] private bool isOverheated;
    [SerializeField] private float delay;
    [SerializeField] private bool isFiring;
    [SerializeField] private float overheat;

    private void Update()
    {
        if (!isOverheated)
        {
            overheat -= Time.deltaTime * overheatCoolRate;
            overheat = Mathf.Clamp(overheat, 0, maxOverheat);
        }

        if (targetingSystem.target != null)
        {
            gatlingTarget = targetingSystem.target;
        }

        else
        {
            gatlingTarget = null;
        }

        if (gatlingTarget != null && !isOverheated)
        {
            if (isFiring)
            {
                FireGatling();
            }
        }
    }

    public void OnFireInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isFiring = true;
        }
        else if (context.canceled)
        {
            isFiring = false;
        }
    }

    public void FireGatling()
    {
        delay -= Time.deltaTime;

        if (delay <= 0)
        {
            foreach (Transform spawnPoint in gatlingSpawnPoint)
            {
                GameObject newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
                newBullet.transform.LookAt(gatlingTarget);
                newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * bulletSpeed * 10);
                overheat++;
            }

            delay = _delay;
        }

        if (overheat >= maxOverheat)
        {
            Overheat();
        }
    }

    private void Overheat()
    {
        if (isOverheated) return;

        isOverheated = true;

        StartCoroutine(OverheatRoutine());
    }

    private IEnumerator OverheatRoutine()
    {
        yield return new WaitForSeconds(overheatCooldown);
        isOverheated = false;
        overheat = 0;
    }
}