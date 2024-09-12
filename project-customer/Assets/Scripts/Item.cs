using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string Name;
    public int id;
    public string itemText;
    public bool Inspect;
    public GameObject inspectModel;
    public InteractionType interactionType;

    public enum InteractionType
    {
        Text,
        Rotate
    }
    public void Interact()
    {
        switch (interactionType)
        {
            case InteractionType.Text:
                DialogueManager.Instance.StartDialogue(Name, itemText.RootNode, true);
                break;
            case InteractionType.Rotate:
                //add trigger for item rotation here
               
                break;
        }
    }
}
