using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class OpenScript : MonoBehaviour
{
    [SerializeField]
    private GameObject doorClosed;
    [SerializeField]
    private GameObject doorOpened;

    string filepath;
    string audioName;
   public AudioSource source;
    public AudioClip openSound;
    public AudioClip closeSound;
    public bool isOpen;

    public void Open()
    {
        source.PlayOneShot(openSound);
        doorOpened.SetActive(true);
        doorClosed.SetActive(false);
        isOpen = true;
    }
    
    public void Close()
    {
        source.PlayOneShot(closeSound);
        
        doorClosed.SetActive(true);
        doorOpened.SetActive(false);
        isOpen = false;
    }
}
