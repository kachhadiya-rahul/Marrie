using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemItem : MonoBehaviour, Icollectible
{
    public void CollectItem()
    {
        gameObject.SetActive(false);
        //Debug.Log("Gem Item collected");
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
