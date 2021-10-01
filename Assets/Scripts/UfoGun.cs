using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoGun : MainObject
{
    private Transform _target;

    private int _shoot;
    private float _timer;
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _shoot = Random.Range(2, 5);
        _objectPool = ObjectPool.SharedInstance;
    }

    void Update()
    {       
        _timer += Time.deltaTime;        
        Vector3 dir = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (_timer >= _shoot)
        {
            Shoot();
            _timer = 0;
        }
    }
    private void Shoot()
    {
        SpawnBullet(0, gameObject.transform, 1, new Color(255,0,0,255));
        _shoot = Random.Range(2, 5);
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
}
