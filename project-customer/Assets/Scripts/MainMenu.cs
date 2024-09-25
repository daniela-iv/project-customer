using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialPage;
    [SerializeField]
    private GameObject tutorial2Page;
    [SerializeField]
    private Actor dad;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        tutorialPage.SetActive(true);
    }

    public void Continu()
    {
        Time.timeScale = 1f;
        tutorial2Page.SetActive(false);  
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        DialogueManager dialogueManager = DialogueManager.Instance;
        if (dialogueManager.inDialogue)
        {
            dialogueManager.HideDialogue();
        }
        else
        {
            dialogueManager.StartDialogue(dad, dad.Dialogue.RootNode, dad.italicize);
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
