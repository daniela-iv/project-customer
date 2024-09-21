using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private string inspectTextItem = "(F) Inspect ";
    private string inspectTextDoor = "(F) To open ";
    private string inspectTextNPC = "(F) Talk with ";

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
                if (!isInspecting)
                {
                    inspectableUI.SetActive(true);
                    inspectableUI.GetComponentInChildren<TextMeshProUGUI>().text = inspectTextItem + hit.transform.GetComponent<Item>().Name;
                }

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
                inspectableUI.GetComponentInChildren<TextMeshProUGUI>().text = inspectTextNPC + hit.transform.GetComponent<Actor>().Name;
                //fuction to show Question UI
            }
            else if((hit.transform.gameObject.tag == "Door"))
            {
                OpenScript openScript = hit.transform.GetComponent<OpenScript>();
                if (!isInspecting)
                {
                    inspectableUI.SetActive(true);
                    if(openScript.isOpen)
                    {
                        inspectTextDoor = "(F) To close ";
                    }
                    else
                    {
                        inspectTextDoor = "(F) To open ";
                    }

                    inspectableUI.GetComponentInChildren<TextMeshProUGUI>().text = inspectTextDoor;
                }

                if (Input.GetButtonDown("Interact"))
                {
                    Debug.Log("trying to interact with Door");
                    if (openScript.isOpen)
                    {
                        openScript.Close();
                    }
                    else
                    {
                        openScript.Open();
                    }
                }
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
