using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    string Name;
    public int id;
    public string itemText;

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Inventory>().PickUp(this);
        Destroy(gameObject);
    }
}
