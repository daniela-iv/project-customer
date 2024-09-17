using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode : iFollowReputation
{
    public string DialogueText;
    public List<DialogueResponse> Responses;

    public string positiveReputationText;
    public List<DialogueResponse> positiveReputationResponses;

    public string negativeReputationText;
    public List<DialogueResponse> negativeReputationResponses;


    public void SetReputation(bool reputationValue)
    {
        GameObject.FindObjectOfType<DialogueManager>().positiveReputation = reputationValue;
        Debug.Log("Positive reputation has changed to " + reputationValue);
    }

    internal bool IsLastNode()
    {
        return Responses.Count <= 0;
    }
}
