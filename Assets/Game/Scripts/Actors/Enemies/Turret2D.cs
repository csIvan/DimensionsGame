using UnityEngine;
using System.Collections;

public class Turret2D : MonoBehaviour {

    [SerializeField] private float fireRate = 1.0f;  
    [SerializeField] private GameObject CannonBallPrefab;
    [SerializeField] private Transform TurretBarrel;
    private bool IsShooting;

    private AudioSource CannonShotSFX;


    // --------------------------------------------------------------------
    private void Awake() {
        CannonShotSFX = GetComponent<AudioSource>();
    }


    // --------------------------------------------------------------------
    private void OnEnable() {
        IsShooting = true;
        StartCoroutine(Shoot());
    }


    // --------------------------------------------------------------------
    private IEnumerator Shoot() {
        yield return new WaitForSeconds(1.0f);

        while (IsShooting) {
            SpawnCannonBall();
            yield return new WaitForSeconds(fireRate);
        }
    }


    // --------------------------------------------------------------------
    private void SpawnCannonBall() {
        CannonShotSFX.Play();

        GameObject cannonBall = Instantiate(CannonBallPrefab, TurretBarrel.position, Quaternion.identity);
        Vector3 cannonBallDirection = TurretBarrel.position - transform.position;

        if (cannonBall.TryGetComponent(out CannonBall2D ball)) {
            ball.Initialize(cannonBallDirection);
        }
    }
}