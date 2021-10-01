using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidControl : MainObject
{
    private float _speed;
    private int _size = 0;

    GameManager _gameManager;
    void Start()
    {
        _objectPool = ObjectPool.SharedInstance;        
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    void Update()
    {
        transform.position += transform.right * _speed * Time.deltaTime;
        Mirror();
        Physics2D.IgnoreLayerCollision(6, 6, true);
    }

    public void DestroyAsteroid()
    {
        AsteroidSpeed();
        if (_size == 0)
        {            
            Dublicate(2);
            _gameManager.ChangeScore(20);
        }
        if (_size == 1)
        {            
            Dublicate(3);
            _gameManager.ChangeScore(50);
        }
    }
    private void Dublicate(int size)
    {
        SpawnObjects(size, transform.GetChild(0).transform);
        SpawnObjects(size, transform.GetChild(1).transform);
        gameObject.SetActive(false);
    }
    public override void SpawnObjects(int _index, Transform _transform)
    {
        GameObject Object = _objectPool.GetPooledObject(_index);
        Object.transform.position = _transform.transform.position;
        Object.transform.rotation = _transform.transform.rotation;
        Object.GetComponent<AsteroidControl>().Speed(_speed);
        Object.SetActive(true);
        Object.GetComponent<AsteroidControl>()._size = 1;
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && gameObject.tag == "Asteroid")
        {
            SpawnParticle(6, col.transform);
            _gameManager.DestroyPlayer(col, 4);
            gameObject.SetActive(false);
        }    
        if (col.gameObject.tag == "Ufo" && gameObject.tag == "Asteroid")
        {
            gameObject.SetActive(false);
            col.gameObject.SetActive(false);
            _gameManager.Kill(4);
            _gameManager.StartCoroutine("SpawnUfo");
            _gameManager.PlayEffect(2);
        }
        if (col.gameObject.tag == "Player" && gameObject.tag == "AsteroidMed")
        {
            SpawnParticle(6, col.transform);
            _gameManager.DestroyPlayer(col, 2);
            gameObject.SetActive(false);
        }
        if (col.gameObject.tag == "Ufo" && gameObject.tag == "AsteroidMed")
            {
                gameObject.SetActive(false);
                col.gameObject.SetActive(false);
                _gameManager.Kill(2);
               _gameManager.StartCoroutine("SpawnUfo");
               _gameManager.PlayEffect(2);
            }
        if (col.gameObject.tag == "Player" && gameObject.tag == "AsteroidDestroy")
        {
            SpawnParticle(6, col.transform);
            _gameManager.DestroyPlayer(col, 1);
            gameObject.SetActive(false);
        }
        if (col.gameObject.tag == "Ufo" && gameObject.tag == "AsteroidDestroy")
        {
            _gameManager.Kill(1);
            gameObject.SetActive(false);
            col.gameObject.SetActive(false);
            _gameManager.StartCoroutine("SpawnUfo");
            _gameManager.PlayEffect(2);
        }
    }
    public void Speed(float count)
    {
        _speed = count;
    }
    public void AsteroidSpeed()
    {
        _speed = Random.Range(1, 4);
    }   

    
}
