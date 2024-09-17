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
    public string id;
    public Dialogue ItemDialogue;
    public string ItemNote;
    public GameObject inspectModel;

    public InteractionType interactionType;

    public enum InteractionType
    {
        Text,
        Rotate,
        Switch,
        exit
    }
    public void Interact(GameObject inspect, GameObject inspectObjPos)
    {
        switch (interactionType)
        {
            case InteractionType.Text:
                Debug.Log("item with text interact");
                DialogueManager.Instance.StartDialogue(Name, ItemDialogue.RootNode, true);
                break;

            case InteractionType.Rotate:
                Debug.Log("its trying rotate");
                if (true)
                {
                    Debug.Log("item with rotate interact");
                    inspect.SetActive(true);
                    Instantiate(inspectModel, inspectObjPos.transform.position, new Quaternion(0, 0, 0, 0), inspectObjPos.transform);
                    Camera.main.GetComponent<MouseLook>().CanLookAround = false;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().CanLookAround = false;
                }
                break;
            case InteractionType.Switch:
                Debug.Log("item with switch interact");
                GameManager.Instance.SecondStage();
                break;
            case InteractionType.exit:
                Debug.Log("item with exit interact");
                GameManager.Instance.EndGame(false);
                break;
        }
    }
}
