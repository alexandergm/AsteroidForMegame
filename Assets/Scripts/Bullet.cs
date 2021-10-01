using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Bullet : MainObject
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    List<AudioClip> _audio = new List<AudioClip>();
    [NonSerialized]
    public int _id = 0;

    private float _lifeTime;

    GameManager _gameManager;

    AudioSource _source;

    void Start()
    {
        _lifeTime = 0;
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _objectPool = GameObject.FindObjectOfType<ObjectPool>();
    }
    void Update()
    {
        _source = GetComponent<AudioSource>();
        _lifeTime += Time.deltaTime;
        transform.position += transform.right * _speed * Time.deltaTime;
        Mirror();
        if (_id == 1)
        {
            Physics2D.IgnoreLayerCollision(0, 7, true);
            Physics2D.IgnoreLayerCollision(0, 6, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(0, 7, false);
            Physics2D.IgnoreLayerCollision(0, 6, false);
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Asteroid" && _id == 0 || col.gameObject.tag == "AsteroidMed" && _id == 0)
        {
            col.gameObject.GetComponent<AsteroidControl>().DestroyAsteroid();            
            gameObject.SetActive(false);
            _gameManager.PlayEffect(1);
        }

        if (col.gameObject.tag == "AsteroidDestroy" && _id == 0)
        {
            _gameManager.ChangeScore(100);
            _gameManager.Kill(1);
            gameObject.SetActive(false);
            col.gameObject.SetActive(false);
            _gameManager.PlayEffect(0);

        }
        if (col.gameObject.tag == "Ufo" && _id == 0)
        {
            _gameManager.ChangeScore(200);
            col.gameObject.SetActive(false);
            gameObject.SetActive(false);
            _gameManager.StartCoroutine("SpawnUfo");
            _gameManager.PlayEffect(2);
        }
        if (col.gameObject.tag == "Player" && _id == 0)
        {
            gameObject.SetActive(false);
            _gameManager.PlayEffect(2);
        }
        if (col.gameObject.tag == "Player" && _id == 1)
        {           
            SpawnParticle(6, col.transform);
            GameManager.FindObjectOfType<GameManager>().DestroyPlayer(col, 0);
            gameObject.SetActive(false);
            
        }               
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }
    public override void Mirror()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        if (transform.position.x < min.x)
        {
            transform.position = new Vector2(-transform.position.x - 0.1f, transform.position.y);
        }
        if (transform.position.x > max.x)
        {
            transform.position = new Vector2(-transform.position.x + 0.1f, transform.position.y);
        }
        if (transform.position.y < min.y)
        {
            transform.position = new Vector2(transform.position.x, -transform.position.y - 0.1f);
        }
        if (transform.position.y > max.y)
        {
            transform.position = new Vector2(transform.position.x, -transform.position.y + 0.1f);
        }
        if ((_speed * _lifeTime) >= (max.x - min.x))
        {
            _lifeTime = 0;
            gameObject.SetActive(false);
        }
    }
   
}


