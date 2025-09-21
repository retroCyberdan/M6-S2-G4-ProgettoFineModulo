using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretsController : MonoBehaviour
{
    private enum TOWER_TYPE { STANDARD, RIFLE }

    [Header("Turrets Settings")]
    [SerializeField] private TOWER_TYPE _towerType;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _barrelSpawnPoint;
    [SerializeField] private float _distance = 10f;
    [SerializeField] private float _detectionRange = 6f;
    [SerializeField] private float _fireRate = 1.5f;
    [SerializeField] private float _automaticNextFire;
    //[SerializeField] private float _lifeSpawn = 3f;
    [SerializeField] private GameObject _bulletPrefab;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // gestisco la rotazione della torretta (head) e sparo al player
        _distance = Vector3.Distance(_player.position, transform.position);
        if (_distance <= _detectionRange)
        {
            _head.LookAt(_player);
            if(Time.time >= _automaticNextFire)
            {
                _automaticNextFire = Time.time + 1f / _fireRate;
                ShootByType();
            }
        }
    }

    void ShootAtPlayer() // <- generazione del bullet
    {
        GameObject bulletClone = Instantiate(_bulletPrefab, _barrelSpawnPoint.position, _head.rotation);
        //GameObject bulletClone = BulletPool.BulletPooling.GetBullet();
        bulletClone.GetComponent<Rigidbody>().AddForce(_head.forward * 10f, ForceMode.Impulse);
        //Destroy(bulletClone, _lifeSpawn);
    }

    void ShootByType() // <- gestione dello sparo secondo il tipo selezionato
    {
        switch (_towerType)
        {
            case TOWER_TYPE.STANDARD:
                ShootAtPlayer();
                break;

            case TOWER_TYPE.RIFLE:
                StartCoroutine(RifleShoot());
                break;
        }
    }

    IEnumerator RifleShoot() // <- coroutine per gestire lo sparo a raffica
    {
        int shots = 3;
        float delay = 0.15f;

        for (int i = 0; i < shots; i++)
        {
            ShootAtPlayer();
            yield return new WaitForSeconds(delay);
        }
    }
}
