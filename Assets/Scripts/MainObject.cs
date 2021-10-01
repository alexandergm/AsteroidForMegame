using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObject : MonoBehaviour
{
    protected ObjectPool _objectPool;
   
        
    public virtual void SpawnObjects(int _index, Transform _transform)
    {
        GameObject Object = _objectPool.GetPooledObject(_index);
        Object.transform.position = _transform.transform.position;
        Object.transform.rotation = _transform.transform.rotation;
        Object.SetActive(true);
    }
    public virtual void SpawnBullet(int _index, Transform _transform, int count, Color color)
    {
        GameObject Object = _objectPool.GetPooledObject(_index);
        Object.transform.position = _transform.transform.position;
        Object.transform.rotation = _transform.transform.rotation;
        Object.GetComponent<SpriteRenderer>().color = color;
        Object.SetActive(true);
        Object.GetComponent<Bullet>()._id = count;
    }
    public virtual void SpawnParticle(int _index, Transform _transform)
    {
        GameObject Object = _objectPool.GetPooledObject(_index);
        Object.transform.position = _transform.transform.position;
        Object.transform.rotation = _transform.transform.rotation;        
        Object.SetActive(true);
       
    }


    public virtual void Mirror()
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
    }
   

}
