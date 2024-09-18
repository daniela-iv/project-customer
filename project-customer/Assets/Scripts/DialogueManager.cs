using System;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

    // true = they like you
    // false =  neutral or hateful
    [NonSerialized]
    public bool childReputation;
    [NonSerialized]
    public bool momReputation;
    [NonSerialized]
    public bool dadReputation;

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

        momReputation = startingReputation;
        dadReputation = startingReputation;
        childReputation = startingReputation;
    }
    public bool GetReputation(string name)
    {
        switch (name)
        {
            case "Mom":
                return momReputation;
            case "Dad":
                return dadReputation;
            case "Child":
                return childReputation;
        }
        Debug.Log("Ya fucked up");
        return true;
    }
    public void SetReputation(string name, bool newReputationValue)
    {
        switch (name)
        {
            case "Mom":
                momReputation = newReputationValue;
                break;
            case "Dad":
                dadReputation = newReputationValue;
                break;
            case "Child":
                childReputation = newReputationValue;
                break;
        }
        Debug.Log("Ya fucked up again");
    }
    public void StartDialogue(string title, DialogueNode node, bool italicize = false)
    {
       
        if (ShowDialogueCheck(title, node, GetReputation(title), italicize))
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

            DialogueLogic(title, node, GetReputation(title), italicize);
        }
    }

    private bool ShowDialogueCheck(string title, DialogueNode node, bool posReputation, bool italicize = false)
    {
        if (posReputation)
        {
            foreach (DialogueResponse response in node.positiveReputationResponses)
            {
                if (response.NextNode.playedPositiveDialogue == false)
                {
                    if (response.RequiredObjectTag == "" || InventoryContains(response.RequiredObjectTag))
                    {
                        return true;
                    }
                }
            }
        }
        else
        {
            foreach (DialogueResponse response in node.negativeReputationResponses)
            {
                if (response.NextNode.playedNegativeDialogue == false)
                {
                    if (response.RequiredObjectTag == "" || InventoryContains(response.RequiredObjectTag))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
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

                            buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectResponse(response, title, italicize));
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

                        print(response.RequiredObjectTag);
                        if (response.RequiredObjectTag == "" || InventoryContains(response.RequiredObjectTag))
                        {

                            GameObject buttonObj = Instantiate(ResponseButtonPrefab, ResponseButtonContainer);
                            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.ResponseText;

                            buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectResponse(response, title, italicize));
                        }
                    }
                }
            }
        }
    }

    private bool InventoryContains(string tag)
    {
        foreach (ItemSlot slot in itemSlots) 
            {
            if (slot.itemInSlot != null)
            {
                Debug.Log("You have an item");
                if (slot.itemInSlot.Name == tag)
                {
                    Debug.Log("You have the correct item");

                    return true;
                }
            }
            }
        return false;
    }

    public void SelectResponse(DialogueResponse response, string title, bool italicize = false)
    {
        if (!response.NextNode.IsLastNode())
        {
            print("test1");
            response.NextNode.speakerName = title;
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
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
    private void ShowDialogue()
    {
        DialogueParent.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }
    public void FreezePlayer(bool doFreeze)
    {
        Debug.Log("Player frozen: " + doFreeze);
        inDialogue = doFreeze;
        player.GetComponent<MouseLook>().CanLookAround = !doFreeze;
        Camera.main.GetComponent<MouseLook>().CanLookAround = !doFreeze;
    }
}
