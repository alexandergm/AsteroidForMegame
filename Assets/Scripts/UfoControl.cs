using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoControl : MonoBehaviour
{
    Rigidbody2D _rb;
    int _speed = 90 ;
    int _axis ;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();       
        
    }
    void FixedUpdate()
    {        
        _rb.velocity = _axis * _rb.transform.right * _speed * Time.deltaTime;
    }
    public void ChangeAxis(int count)
    {        
        _axis = count;
    }
   
   


}
