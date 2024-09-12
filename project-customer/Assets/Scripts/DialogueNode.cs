using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public string DialogueText;
    public List<DialogueResponse> Responses;
    internal bool IsLastNode()
    {
        return Responses.Count <= 0;
    }
}
