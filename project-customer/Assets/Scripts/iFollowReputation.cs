using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iFollowReputation : IEnumerable
{
    public string positiveReputationText;
    public List<DialogueResponse> positiveReputationResponses;

    public string negativeReputationText;
    public List<DialogueResponse> negativeReputationResponses;

    public void CheckRep() { }
}
