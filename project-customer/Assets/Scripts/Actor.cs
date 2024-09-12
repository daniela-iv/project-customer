using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Actor : MonoBehaviour, iLookReciever
{
    public string Name;
    public Dialogue Dialogue;
    private bool isLookingAt;
    public bool italicize;

    void SpeakWith()
    {
        DialogueManager.Instance.StartDialogue(Name, Dialogue.RootNode, italicize);
    }

    private void Update()
    {
        if (isLookingAt)
        {
            if (Input.GetButtonDown("Interact"))
            {
                SpeakWith();
            }
        }
    }

    public void LookingAt()
    {
        isLookingAt = true;
    }
    public void NotLookingAt()
    {
        isLookingAt= false;
        Console.WriteLine("youre not looking at the npc!!");

    }
}
