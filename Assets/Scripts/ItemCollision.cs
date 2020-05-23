using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Icollectible itemToCollect =  collision.gameObject.GetComponent<Icollectible>();
        if (itemToCollect != null)
        {
            itemToCollect.CollectItem();
        }
    }
}
