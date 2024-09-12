using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
   public static DialogueManager Instance { get; private set; }

    public GameObject DialogueParent;
    public TextMeshProUGUI DialogueTitle, DialogueBody;
    public GameObject ResponseButtonPrefab;
    public Transform ResponseButtonContainer;
    public GameObject player;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        HideDialogue();
    }

    public void FreezePlayer(bool doFreeze)
    {
        Debug.Log("Player frozen: " + doFreeze);
        player.GetComponent<MouseLook>().CanLookAround = !doFreeze;
        Camera.main.GetComponent<MouseLook>().CanLookAround = !doFreeze;
    }

    public void StartDialogue(string title, DialogueNode node, bool italicize = false)
    {
        FreezePlayer(true);
        ShowDialogue();
        DialogueTitle.text = title;
        DialogueBody.text = node.DialogueText;

        if (italicize)
        {
            DialogueBody.fontStyle = FontStyles.Italic;
        }
        else
        {
            DialogueBody.fontStyle = FontStyles.Normal;
        }

            foreach (Transform child in ResponseButtonContainer)
            {
                Destroy(child.gameObject);
            }

        foreach (DialogueResponse response in node.Responses)
        {
            GameObject buttonObj = Instantiate(ResponseButtonPrefab, ResponseButtonContainer);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.ResponseText;
            
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response, title, italicize));
        }
    }

    public void SelectResponse(DialogueResponse response, string title, bool italicize = false)
    {
        if (!response.NextNode.IsLastNode())
        {
            StartDialogue(title, response.NextNode,italicize);
        }
        else
        {
            HideDialogue();
            FreezePlayer(false);
        }
    }
    public void HideDialogue()
    {
        DialogueParent.SetActive(false);
    }
    private void ShowDialogue()
    {
        DialogueParent.SetActive(true);
    }

}
