using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    public ItemSlot[] invSlots = new ItemSlot[10];

    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private TextMeshProUGUI textRight;

    [SerializeField]
    private CanvasGroup journal;
    [SerializeField]
    private FadeScript icon;

    private int numOfNotes;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Journal"))
        {
            if (journal.alpha == 1f)
                journal.alpha = 0f;
            else
                journal.alpha = 1f;
        }
    }

    public void PickUp(Item item)
    {
        if (!ItemCheck(item))
        {
            for (int i = 0; i < invSlots.Length; i++)
            {
                if (invSlots[i].itemInSlot == null)
                {
                    invSlots[i].itemInSlot = item;
                    icon.fadeIn();
                    if(numOfNotes >= 5)
                    {
                        textRight.text += "<BR>" + item.ItemNote;
                        print("tetaovhfAEPIHLSBDEFPICA;KN");
                        numOfNotes++;
                    }
                    else if (numOfNotes < 5)
                    { 
                        text.text += "<BR>" + item.ItemNote;
                        numOfNotes++;
                    }
                    break;
                }
            }
        }
    }

    public bool ItemCheck(Item item)
    {
        for (int i = 0; i < invSlots.Length; i++)
        {
            if (invSlots[i].itemInSlot == item)
            {
                return true;
            }
        }
        return false;
    }
}
