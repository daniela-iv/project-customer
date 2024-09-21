using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScript : MonoBehaviour
{
    [SerializeField]
    private GameObject doorClosed;
    [SerializeField]
    private GameObject doorOpened;

    public bool isOpen;

    public void Open()
    {
        doorClosed.SetActive(false);
        doorOpened.SetActive(true);
        isOpen = true;
    }
    
    public void Close()
    {
        doorClosed.SetActive(true);
        doorOpened.SetActive(false);
        isOpen = false;
    }
}
