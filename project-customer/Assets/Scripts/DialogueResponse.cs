using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialogueResponse
{
    public string ResponseText;
    public string RequiredObjectTag;
    public DialogueNode NextNode;
}
