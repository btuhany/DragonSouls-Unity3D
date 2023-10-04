using UnityEngine;
using System.Collections.Generic;

public abstract class ObjectPoolBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private int _size;
    [SerializeField] private int _expandSize;
    private Queue<T> _pooledObjects;
    protected void Initialize()
    {
        _pooledObjects = new Queue<T>();
        for (int i = 0; i < _size; i++)
        {
            T newObj = Instantiate(_prefab, this.transform);
            newObj.gameObject.SetActive(false);
            _pooledObjects.Enqueue(newObj);
        }
    }
    public T GetObject()
    {
        if (_pooledObjects.Count <= 0)
            ExpandPool(_expandSize);
        T newObj = _pooledObjects.Dequeue();
        newObj.gameObject.SetActive(true);
        //newObj.transform.parent = null;
        return newObj;
    }
    public T GetObjectDisabled()
    {
        if (_pooledObjects.Count <= 0)
            ExpandPool(_expandSize);
        T newObj = _pooledObjects.Dequeue();
        //newObj.transform.parent = null;
        return newObj;
    }
    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        _pooledObjects.Enqueue(obj);
    }
    private void ExpandPool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            T newObj = Instantiate(_prefab, this.transform);
            newObj.gameObject.SetActive(false);
            _pooledObjects.Enqueue(newObj);
        }
    }
}
