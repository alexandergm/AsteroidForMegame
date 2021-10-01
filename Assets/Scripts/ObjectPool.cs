using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{

    public GameObject _objectToPool;
    public int _amountToPool;
    public bool _shouldExpand = true;

    public ObjectPoolItem(GameObject obj, int amt, bool exp = true)
    {
        _objectToPool = obj;
        _amountToPool = Mathf.Max(amt, 2);
        _shouldExpand = exp;
    }
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<ObjectPoolItem> itemsToPool;

    public List<List<GameObject>> _pooledObjectsList;
    public List<GameObject> _pooledObjects;
    private List<int> _positions;

    void Awake()
    {
        SharedInstance = this;

        _pooledObjectsList = new List<List<GameObject>>();
        _pooledObjects = new List<GameObject>();
        _positions = new List<int>();


        for (int i = 0; i < itemsToPool.Count; i++)
        {
            ObjectPoolItemToPooledObject(i);
        }
    }
    public GameObject GetPooledObject(int index)
    {

        int curSize = _pooledObjectsList[index].Count;
        for (int i = _positions[index] + 1; i < _positions[index] + _pooledObjectsList[index].Count; i++)
        {

            if (!_pooledObjectsList[index][i % curSize].activeInHierarchy)
            {
                _positions[index] = i % curSize;
                return _pooledObjectsList[index][i % curSize];
            }
        }

        if (itemsToPool[index]._shouldExpand)
        {

            GameObject obj = (GameObject)Instantiate(itemsToPool[index]._objectToPool);
            obj.SetActive(false);
            obj.transform.parent = this.transform;
            _pooledObjectsList[index].Add(obj);
            return obj;

        }
        return null;
    }

    public List<GameObject> GetAllPooledObjects(int index)
    {
        return _pooledObjectsList[index];
    }
    public int AddObject(GameObject GO, int amt = 3, bool exp = true)
    {
        ObjectPoolItem item = new ObjectPoolItem(GO, amt, exp);
        int currLen = itemsToPool.Count;
        itemsToPool.Add(item);
        ObjectPoolItemToPooledObject(currLen);
        return currLen;
    }
    void ObjectPoolItemToPooledObject(int index)
    {
        ObjectPoolItem item = itemsToPool[index];

        _pooledObjects = new List<GameObject>();
        for (int i = 0; i < item._amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(item._objectToPool);
            obj.SetActive(false);
            obj.transform.parent = this.transform;
            _pooledObjects.Add(obj);
        }
        _pooledObjectsList.Add(_pooledObjects);
        _positions.Add(0);

    }
}