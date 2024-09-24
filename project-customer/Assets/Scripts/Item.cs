using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string Name;
    public Dialogue ItemDialogue;
    public string ItemNote;
    public GameObject inspectModel;

    public InteractionType interactionType;
    private bool isItem = true;

    [SerializeField]
    private AudioClip clip;

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
                DialogueManager dialogueManager = DialogueManager.Instance;
                GetComponent<AudioSource>().PlayOneShot(clip);
                if (dialogueManager.inDialogue)
                {
                    dialogueManager.HideDialogue();
                }
                else
                {
                    dialogueManager.StartDialogue(this, ItemDialogue.RootNode, true, true);
                }
                break;

            case InteractionType.Rotate:
                Debug.Log("its trying rotate");
                if (!inspect.activeSelf)
                {
                    Debug.Log("item with rotate interact");
                    inspect.SetActive(true);
                    Instantiate(inspectModel, inspectObjPos.transform.position, Quaternion.identity, inspectObjPos.transform);
                    Camera.main.GetComponent<MouseLook>().CanLookAround = false;
                    GetComponent<AudioSource>().PlayOneShot(clip);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().CanLookAround = false;
                }
                break;
            case InteractionType.Switch:
                if (GameManager.Instance.gameState == GameManager.GameState.firstStage)
                {
                    Debug.Log("item with switch interact");
                    GetComponentInChildren<Light>().color = Color.red;
                    GameManager.Instance.SecondStage();
                }
                break;
            case InteractionType.exit:
                Debug.Log("item with exit interact");
                GameManager.Instance.EndGame(false);
                break;
        }
    }
}
