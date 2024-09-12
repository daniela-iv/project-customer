using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    public string Name;
    public int id;
    public Dialogue ItemDialogue;
    public string ItemNote;
    public GameObject inspectModel;

    public InteractionType interactionType;

    public enum InteractionType
    {
        Text,
        Rotate
    }
    public void Interact(GameObject inspect= null, GameObject inspectObjPos = null)
    {
        switch (interactionType)
        {
            case InteractionType.Text:
                Debug.Log("item with text interact");
                DialogueManager.Instance.StartDialogue(Name, ItemDialogue.RootNode, true);
                break;

            case InteractionType.Rotate:

                if (inspect != null && inspectObjPos !=null && inspect.activeSelf == false)
                {
                    Debug.Log("item with rotate interact");
                    inspect.SetActive(true);
                    Instantiate(inspectModel, inspectObjPos.transform.position, new Quaternion(0, 0, 0, 0), inspectObjPos.transform);
                    Camera.main.GetComponent<MouseLook>().CanLookAround = false;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().CanLookAround = false;
                }
                break;
        }
    }
}
