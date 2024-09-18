using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public bool changeReputation;
    public bool changeReputationTo;
    public void Awake()
    {
        playedNegativeDialogue = false;
        playedPositiveDialogue = false;
    }

    public bool IsLastPositiveNode()
    {

        if (positiveReputationDialogue.Count() <= 0) { return true; }
        else return false;
    }
    public bool IsLastNegativeNode()
    {

        if (negativeReputationDialogue.Count() <= 0) { return true; }
        else return false;
    }

}
