using System;
using UnityEngine;

public class LookingAt : MonoBehaviour
{
    public Camera camera;
    private GameObject lastLookedAt;
    public float LookDistance;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private GameObject inspect;
    [SerializeField]
    private GameObject inspectObjPos;

    private void Awake()
    {
    }
    public void Update()
    {
       CheckLook();
    }

    private void OnEnable()
    {
        
    }
    private void CheckLook()
    {
        if (lastLookedAt)
        {
            lastLookedAt.SendMessage("NotLookingAt", SendMessageOptions.DontRequireReceiver);
        }

        Ray lookRay = new Ray(camera.transform.position, camera.transform.rotation * Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(lookRay, out hit, LookDistance))
        {
            hit.transform.SendMessage("LookingAt", SendMessageOptions.DontRequireReceiver);
            lastLookedAt = hit.transform.gameObject;

            if (hit.transform.gameObject.tag == "Item")
            {
                Debug.Log("look at item");
                if (Input.GetButtonDown("Interact"))
                {
                    Item item = hit.transform.GetComponent<Item>();
                    inventory.GetComponent<Inventory>().PickUp(hit.transform.GetComponent<Item>());

                    if (item.Inspect && inspect.activeSelf == false)
                    {
                        inspect.SetActive(true);
                        Instantiate(item.inspectModel, inspectObjPos.transform.position, new Quaternion(0, 0, 0, 0), inspectObjPos.transform);
                        camera.GetComponent<MouseLook>().CanLookAround = false;
                        GetComponent<MouseLook>().CanLookAround = false;
                    }

                    item.Interact();
                }

                //function to show Examining UI
            }
            else if (hit.transform.gameObject.tag == "Actor")
            {

                //fuction to show Question UI
            }
            //dialogue logic is in actor/item script
        }
    }
}
