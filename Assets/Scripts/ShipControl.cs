using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShipControl : MainObject
{ 
    public enum Control
    {
        ButtonControl,
        MouseControl
    }

    [SerializeField]
    [Range(0, 10)] private float _rotateSpeed;
    [SerializeField]
    [Range(0, 150)] private float _speed;
    [SerializeField]
    AudioClip _sound;

    public Control control = Control.MouseControl;
    
    private float _shootTimer;

    private float _blinkTimer;

   

    private Rigidbody2D _rb;

    private Transform _spawnPoint;   
    
   

    AudioSource _source;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spawnPoint = transform.GetChild(0).transform;
        _objectPool = ObjectPool.SharedInstance;
       
        
    }
    private void Start()
    {
        StartCoroutine("Blink");
        _source = GetComponent<AudioSource>();
    }
    void Update()
    {       
       
        FireClass();
        Mirror();
    }
    private void FixedUpdate()
    {
        MoveClass();
    }
    private void MoveClass()
    {
        if (GameManager.FindObjectOfType<GameManager>()._isButtonControl == true)
        {
            control = Control.ButtonControl;
        }
        else control = Control.MouseControl;
        #region Buttons_Contol
        switch (control)
        {
            case Control.ButtonControl:
                if (Input.GetButton("Boost1"))
                    Boost();                
                if (Input.GetButtonDown("Boost1"))
                    _source.Play();
                else if(Input.GetButtonUp("Boost1"))
                    _source.Stop();
                if (Input.GetButton("Right"))
                    ButtonRotateShip(-_rotateSpeed);                    
                if (Input.GetButton("Left"))
                    ButtonRotateShip(_rotateSpeed);
                break;
        #endregion
        #region Mouse_Control
            case Control.MouseControl:
                if (Input.GetButton("Boost2"))
                    Boost();
                if (Input.GetButtonDown("Boost2"))
                    _source.Play();
                else if (Input.GetButtonUp("Boost2"))
                    _source.Stop();
                MouseRotateShip();
                break;
        }
        #endregion
    }
    private void FireClass()
    {
        _shootTimer += Time.deltaTime;
        #region Buttons_Contol
        switch (control)
        {
            case Control.ButtonControl:
                if (Input.GetKeyDown(KeyCode.Space) && _shootTimer > 0.33f)
                {
                    _source.PlayOneShot(_sound);
                    _shootTimer = 0;
                    SpawnBullet(0, _spawnPoint, 0, new Color(0, 255, 0, 255));
                }
                break;
        #endregion
        #region Mouse_Control
            case Control.MouseControl:
                if (Input.GetButtonDown("Fire1") && _shootTimer > 0.33f)
                {
                    _source.PlayOneShot(_sound);
                    _shootTimer = 0;
                    SpawnBullet(0, _spawnPoint, 0, new Color(0, 255, 0, 255));
                }
                break;
        }
        #endregion
    }
    private void ButtonRotateShip(float rotateSpeed)
    {
        transform.Rotate(0, 0, rotateSpeed / 1.5f);
    }
    private void MouseRotateShip()
    {
        var currentPosition = transform.position;
        var moveTowards = Input.mousePosition;
        moveTowards = Camera.main.ScreenToWorldPoint(moveTowards);
        Vector2 movement = moveTowards - currentPosition;
        movement.Normalize();
        float targetAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), _rotateSpeed/2f * Time.deltaTime);
    }
    private void Boost()
    {
       
        _rb.velocity = _rb.transform.right * _speed * Time.deltaTime;
    }
   
    

    IEnumerator Blink()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        
        while (true)
        {           
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.25f);
            _blinkTimer++;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.25f);
            if (_blinkTimer >= 6)
            {
                gameObject.GetComponent<Collider2D>().isTrigger = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                _blinkTimer = 0;
                break;
            }
        }
    }
}
       
    




