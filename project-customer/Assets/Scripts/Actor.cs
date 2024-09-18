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
    public UnityEngine.UI.Image positiveFace;
    public UnityEngine.UI.Image negativeFace;

    public bool reputation;

    private void Awake()
    {
        reputation = false;
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

    void SpeakWith()
    {
        DialogueManager.Instance.StartDialogue(this, Dialogue.RootNode, italicize);
    }
    public void SetDialogue()
    {

    }
    public void SetReputation(bool reputationValue)
    {
        reputation = reputationValue;
        Debug.Log("Reputation has changed to " + reputationValue);
    }
    public void LookingAt()
    {
        isLookingAt = true;
    }
    public void NotLookingAt()
    {
        isLookingAt= false;
    }
}
