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
    public ItemSlot[] itemSlots { get { return player.GetComponent<Inventory>().invSlots; } private set { } }

    public bool startingReputation;

    [NonSerialized]
    public bool inDialogue;
    [NonSerialized]
    public bool positiveReputation;

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
        inDialogue = false;
        positiveReputation = startingReputation;
    }

    public void StartDialogue(string title, DialogueNode node, bool italicize = false)
    {
        FreezePlayer(true);

        ShowDialogue();

        DialogueTitle.text = title;

        if (italicize)
        {
            DialogueBody.fontStyle = FontStyles.Italic;
        }
        else
        {
            DialogueBody.fontStyle = FontStyles.Normal;
        }

        DialogueLogic(title, node, positiveReputation, italicize);
    }

    private void DialogueLogic(string title, DialogueNode node, bool posReputation, bool italicize)
    {
        if (posReputation)
        {
            if (node.playedPositiveDialogue == false)
            {
                DialogueBody.text = node.positiveReputationDialogue;

                foreach (Transform child in ResponseButtonContainer)
                {
                    Destroy(child.gameObject);
                }

                foreach (DialogueResponse response in node.positiveReputationResponses)
                {
                    Debug.Log("Loading positive response");
                   if( response.NextNode.playedPositiveDialogue == false){
                        node.playedPositiveDialogue = true;

                        if (response.RequiredObjectTag == "" || InventoryContains(response.RequiredObjectTag))
                        {

                            GameObject buttonObj = Instantiate(ResponseButtonPrefab, ResponseButtonContainer);
                            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.ResponseText;

                            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response, title, italicize));
                        }
                    }
                }
            } 
        }
        else
        {
            Debug.Log("Negative reputation detected");
            DialogueBody.text = node.negativeReputationDialogue;
            if (node.playedNegativeDialogue == false)
            {
                DialogueBody.text = node.negativeReputationDialogue;

                foreach (Transform child in ResponseButtonContainer)
                {
                    Destroy(child.gameObject);
                }

                foreach (DialogueResponse response in node.negativeReputationResponses)
                {
                    Debug.Log("Loading negative response");
                    if (response.NextNode.playedNegativeDialogue == false)
                    {
                        node.playedNegativeDialogue = true;

                        if (response.RequiredObjectTag == "" || InventoryContains(response.RequiredObjectTag))
                        {

                            GameObject buttonObj = Instantiate(ResponseButtonPrefab, ResponseButtonContainer);
                            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.ResponseText;

                            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response, title, italicize));
                        }
                    }
                }
            }
        }
    }

    private bool InventoryContains(string tag)
    {
        foreach (ItemSlot slot in itemSlots) while (slot != null)
            {

                if (slot.itemInSlot.id == tag) return true;

            }
        return false;
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void ShowDialogue()
    {
        DialogueParent.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void FreezePlayer(bool doFreeze)
    {
        Debug.Log("Player frozen: " + doFreeze);
        inDialogue = doFreeze;
        player.GetComponent<MouseLook>().CanLookAround = !doFreeze;
        Camera.main.GetComponent<MouseLook>().CanLookAround = !doFreeze;
    }
}
