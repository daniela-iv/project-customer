using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialPage;

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
        tutorialPage.SetActive(false);  
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
