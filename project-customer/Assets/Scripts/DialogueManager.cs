
using System;
using TMPro;
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

    [SerializeField]
    private UnityEngine.UI.Image portrait;


    [SerializeField]
    private GameObject dad0;
    [SerializeField]
    private GameObject dad1;
    [SerializeField]
    private GameObject dad3;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip buttonClick;

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
        portrait.color = Color.clear;
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
            print("audio button playinh");
            audioSource.PlayOneShot(buttonClick);
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
            DialogueBody.text = node.positiveReputationDialogue;
            portrait.sprite = actor.positiveFace;
            portrait.color = Color.white;

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
                        Debug.Log("loading unseen convo");
                        GameObject buttonObj = Instantiate(ResponseButtonPrefab, ResponseButtonContainer);
                        buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.ResponseText;

                        buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectResponseActor(response, actor, italicize));
                    }

                    node.playedPositiveDialogue = true;
                }
            }

        }
        else
        {
            Debug.Log("Do negative behaviour logic");

            DialogueBody.text = node.negativeReputationDialogue;
            portrait.sprite = actor.negativeFace;
            portrait.color = Color.white;

            foreach (Transform child in ResponseButtonContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (DialogueResponse response in node.negativeReputationResponses)
            {
                if (response.NextNode.playedNegativeDialogue == false)
                {
                    node.playedNegativeDialogue = true;

                    print(response.RequiredObjectTag);
                        
                    if (response.RequiredObjectTag == "" || InventoryContains(response.RequiredObjectTag))
                    {
                        Debug.Log("loading unseen negative convo");

                        GameObject buttonObj = Instantiate(ResponseButtonPrefab, ResponseButtonContainer);
                        buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.ResponseText;

                        buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectResponseActor(response, actor, italicize));
                    }

                    node.playedNegativeDialogue = true;
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

        if(dad0.activeSelf)
        {
            GameManager.Instance.Fade();
        }
        else if(dad3.activeSelf)
        {
            GameManager.Instance.EndGame(true);
        }

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
        player.GetComponent<SimplePhysicsControls>().canMove = !doFreeze;
        Camera.main.GetComponent<MouseLook>().CanLookAround = !doFreeze;
    }
}
