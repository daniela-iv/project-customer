using System;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
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
    public Transform ImageContainer;
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

    public void StartDialogue(Actor actor, DialogueNode node, bool isItem, bool italicize = false)
    {
        if (node.IsLastPositiveNode() && node.IsLastNegativeNode())
        {
            Debug.Log("Cant start actor dialogue - last nodes");
            HideDialogue();
        }
        else
        {
            StartActorDialogue(actor, node, false);
        }
    }
    public void StartDialogue(Item item, DialogueNode node, bool isItem, bool italicize = false)
    {
        if (node.IsLastPositiveNode() && node.IsLastNegativeNode())
        {
            Debug.Log("Cant start item dialogue - last nodes");
            HideDialogue();
        }
        else
        {
            if (node.playedNegativeDialogue == false)
            {
                StartItemDialogue(item, node, false);
            }
        }
    }

    private void StartItemDialogue(Item item, DialogueNode node, bool italicize = false)
    {

        Debug.Log("Starting item dialogue");

        ShowDialogue();
        DialogueTitle.text = item.Name;

        if (italicize)
        {
            DialogueBody.fontStyle = FontStyles.Italic;
        }
        else
        {
            DialogueBody.fontStyle = FontStyles.Normal;
        }

        DialogueBody.text = node.positiveReputationDialogue;

        foreach (Transform child in ResponseButtonContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (DialogueResponse response in node.positiveReputationResponses)
        {
            Debug.Log("Loading positive response for item");

            if (response.NextNode.playedPositiveDialogue == false)
            {
                node.playedPositiveDialogue = true;

                GameObject buttonObj = Instantiate(ResponseButtonPrefab, ResponseButtonContainer);
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.ResponseText;

                buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectResponseItem(response, item, italicize));

            }
            else
            {
                Debug.Log("Forcing item dialogue to shut down");
                HideDialogue();
            }
        }
    }
    public void SelectResponseItem(DialogueResponse response, Item item, bool italicize = false)
    {
        if (!response.NextNode.IsLastPositiveNode())
        {
            response.NextNode.speakerName = item.Name;
            StartItemDialogue(item, response.NextNode, italicize);
        }
        else
        {
            HideDialogue();
        }
    }


    private void StartActorDialogue(Actor actor, DialogueNode node, bool italicize = false)
    {
        Debug.Log("Starting actor dialogue");

        ShowDialogue();

        //load UI image here based on actor.reputation

        DialogueTitle.text = actor.Name;

        if (italicize)
        {
            DialogueBody.fontStyle = FontStyles.Italic;
        }
        else
        {
            DialogueBody.fontStyle = FontStyles.Normal;
        }


        if (actor.reputation)
        {
            Debug.Log(actor.Name + " Do positive behaviour logic: " +actor.reputation);

            if (node.playedPositiveDialogue == false)
            {
                DialogueBody.text = node.positiveReputationDialogue;

                foreach (Transform child in ResponseButtonContainer)
                {
                    Destroy(child.gameObject);
                }

                Debug.Log("Loading positive responses");

                foreach (DialogueResponse response in node.positiveReputationResponses)
                {
                    if (response.NextNode.playedPositiveDialogue == false)
                    {
                        if (response.RequiredObjectTag == "" || InventoryContains(response.RequiredObjectTag))
                        {
                            Debug.Log("");
                            GameObject buttonObj = Instantiate(ResponseButtonPrefab, ResponseButtonContainer);
                            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.ResponseText;

                            buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectResponseActor(response, actor, italicize));
                        }

                        node.playedPositiveDialogue = true;
                    }
                }
            }

        }
        else
        {
            Debug.Log("Do negative behaviour logic");

            if (node.playedNegativeDialogue == false)
            {
                DialogueBody.text = node.negativeReputationDialogue;

                foreach (Transform child in ResponseButtonContainer)
                {
                    Destroy(child.gameObject);
                }

                foreach (DialogueResponse response in node.negativeReputationResponses)
                {
                    if (response.NextNode.playedNegativeDialogue == false)
                    {
                        if (response.RequiredObjectTag == "" || InventoryContains(response.RequiredObjectTag))
                        {
                            Debug.Log("Making buttons for negative");
                            GameObject buttonObj = Instantiate(ResponseButtonPrefab, ResponseButtonContainer);
                            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.ResponseText;

                            buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectResponseActor(response, actor, italicize));
                        }

                        node.playedNegativeDialogue = true;
                    }
                }
            }

        }
    }

    private void SelectResponseActor(DialogueResponse response, Actor actor, bool italicize = false)
    {
        bool isLastNode;

        if (actor.reputation)
        {
            isLastNode = response.NextNode.IsLastPositiveNode();
        }
        else
        {
            isLastNode= response.NextNode.IsLastNegativeNode();
        }

        if (response.NextNode.changeReputation)
        {
            Debug.Log(actor.Name + "    is changing reputation to    " + response.NextNode.changeReputation);
            actor.SetReputation(response.NextNode.changeReputationTo);
        }

        if (!isLastNode)
        {
            response.NextNode.speakerName = actor.Name;
            StartActorDialogue(actor, response.NextNode, italicize);
        }
        else
        {
            HideDialogue();
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

    public void HideDialogue()
    {
        DialogueParent.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        FreezePlayer(false);
    }
    private void ShowDialogue()
    {
        FreezePlayer(true);
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


    /*
        public void StartDialogue(string title, DialogueNode node, bool isItem, bool italicize = false)
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

        private bool ShowDialogueCheck(string title, DialogueNode node, bool posReputation, bool isItem, bool italicize = false)
        {
            if (isItem) return true;

            if (posReputation)
            {
                foreach (DialogueResponse response in node.positiveReputationResponses)
                {
                    if (response.NextNode.playedPositiveDialogue == false && !response.NextNode.IsLastNode())
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

            public void SelectResponse(DialogueResponse response, string title, bool italicize = false)
        {
            if (!response.NextNode.IsLastNode())
            {
                response.NextNode.speakerName = title;
                if (response.NextNode.changeReputation)
                {
                   if(!IsItem(title)) response.NextNode.SetReputation(response.NextNode.changeReputationTo);
                }
                StartDialogue(title, response.NextNode,italicize);
            }
            else
            {
                HideDialogue();
            }
        }
    */

}
