using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryItem : MonoBehaviour, Icollectible
{
    public void CollectItem()
    {
        gameObject.SetActive(false);
        Debug.Log("Cherry Item collected");

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
