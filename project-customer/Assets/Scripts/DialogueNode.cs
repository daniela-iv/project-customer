using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class DialogueNode : iFollowReputation
{
    //public string DialogueText;
    //public List<DialogueResponse> Responses;
    [NonSerialized]
    public string speakerName;

    public string positiveReputationDialogue;
    public List<DialogueResponse> positiveReputationResponses;
    [NonSerialized]
    public bool playedPositiveDialogue;

    public string negativeReputationDialogue;
    public List<DialogueResponse> negativeReputationResponses;
    [NonSerialized]
    public bool playedNegativeDialogue;

    public void Awake()
    {
        playedNegativeDialogue = false;
        playedPositiveDialogue = false;
    }

    public void SetReputation(bool reputationValue)
    {
        DialogueManager.Instance.SetReputation(speakerName, reputationValue);
        Debug.Log("Positive reputation has changed to " + reputationValue);
    }

   public bool IsLastNode()
    {
        if (DialogueManager.Instance.GetReputation(speakerName))
        {
            if (positiveReputationDialogue.Count() <= 1) { return true; }
        }
        else
        {
            if (negativeReputationDialogue.Count() <= 1) { return true; }
        }
        return false;
    }
}
