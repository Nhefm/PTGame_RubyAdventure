using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling sharedInstance;
    [SerializeField] GameObject objectToPool;
    [SerializeField] int poolSize;
    List<GameObject> pool;

    private void Awake() {
        sharedInstance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        pool = new List<GameObject>();
        
        for(int i = 0; i < poolSize; i++)
        {
            GameObject newObj = Instantiate(objectToPool);
            newObj.SetActive(false);
            pool.Add(newObj);
        }
    }

    public GameObject getActiveObject()
    {
        foreach(GameObject gameObject in pool)
        {
            if(!gameObject.activeInHierarchy)
            {
                return gameObject;
            }
        }

        return null;
    }
}
