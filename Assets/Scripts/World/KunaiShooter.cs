using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiShooter : MonoBehaviour
{
    public Transform shootPoint;
    private bool _canShoot = true;
    public float shootInterval = 2f;

    void Update()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (_canShoot)
        {
            Shoot();
            _canShoot = false;
            yield return new WaitForSeconds(shootInterval);
            _canShoot = true;
        }
    }

    private void Shoot()
    {
        GameObject kunai = KunaiPool.Instance.GetKunai();
        kunai.transform.position = shootPoint.position;
    }
}
