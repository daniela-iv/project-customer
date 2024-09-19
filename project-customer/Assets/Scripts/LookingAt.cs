using System;
using UnityEngine;

public class LookingAt : MonoBehaviour
{
    public Camera camera;
    private GameObject lastLookedAt;
    public float LookDistance;
    [SerializeField]
    private DialogueManager dialogueManager;
    [SerializeField]
    private GameObject inspect;
    [SerializeField]
    private GameObject inspectObjPos;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private GameObject inspectableUI;

    private bool isInspecting;

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

        inspectableUI.SetActive(false);

        Ray lookRay = new Ray(camera.transform.position, camera.transform.rotation * Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(lookRay, out hit, LookDistance))
        {
            hit.transform.SendMessage("LookingAt", SendMessageOptions.DontRequireReceiver);
            lastLookedAt = hit.transform.gameObject;

            if (hit.transform.gameObject.tag == "Item")
            {
                if(!isInspecting)
                    inspectableUI.SetActive(true);

                if (Input.GetButtonDown("Interact"))
                {
                    Item item = hit.transform.GetComponent<Item>();
                    inventory.GetComponent<Inventory>().PickUp(hit.transform.GetComponent<Item>());
                    Debug.Log("trying to interact with item");
                    item.Interact(inspect,inspectObjPos);
                    isInspecting = true;
                }

                //function to show Examining UI
            }
            else if (hit.transform.gameObject.tag == "Actor")
            {
                inspectableUI.SetActive(true);

                //fuction to show Question UI
            }
            if (dialogueManager.inDialogue == true)
            {
                inspectableUI.SetActive(false);
            }
            //dialogue logic is in actor/item script
        }
        else
        {
            isInspecting = false;
        }
    }
}
