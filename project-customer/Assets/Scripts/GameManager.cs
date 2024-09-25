using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private FadeScript fade;

    [SerializeField]
    private GameObject dad0;
    public GameObject dad1;
    public GameObject dad2;
    public GameObject dad3;
    [SerializeField]
    private GameObject son1;
    [SerializeField]
    private GameObject son2;
    [SerializeField]
    private GameObject mom1;
    [SerializeField]
    private GameObject mom2;

    [SerializeField]
    private GameObject end1;
    [SerializeField]
    private GameObject end2;
    [SerializeField]
    private GameObject end3;

    public AudioSource audioSource;
    public AudioClip winAudio;
    public AudioClip loseAudio;
    public AudioClip DoorAudio;
    [SerializeField]
    private float dadTimer;

    public AudioSource menuMusic;
    public AudioSource gameMusic;

    public GameObject crosshair;

    public GameState gameState = GameState.firstStage;

    public enum GameState
    {
        firstStage,
        secondStage
    }

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SecondStage()
    {
        Fade();
        print("changed stage");
        gameState = GameState.secondStage;
        StartCoroutine(Timer());
    }

    public void Fade()
    {
        fade.fadeIn();
    }

    private IEnumerator Timer()
    {
        float timer = dadTimer;
        while (timer > 0f)
        {
            timer -= 1f;
            yield return new WaitForSecondsRealtime(1f);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        DialogueManager dialogueManager = DialogueManager.Instance;
        if (dialogueManager.inDialogue)
        {
            dialogueManager.HideDialogue();
        }
        else
        {
            dad2.SetActive(false);
            dad3.SetActive(true);
            Fade();
        }
        StopCoroutine("Timer");
    }

    public void changeCharacters()
    {
        if (dad0.activeSelf)
        {
            dad0.SetActive(false);
            dad1.SetActive(true);
            dad1.GetComponent<Actor>().reputation = dad0.GetComponent<Actor>().reputation;
        }
        else
        {
            dad1.SetActive(false);
            mom1.SetActive(false);
            son1.SetActive(false);
            mom2.GetComponent<Actor>().reputation = mom1.GetComponent<Actor>().reputation;
            dad2.GetComponent<Actor>().reputation = dad1.GetComponent<Actor>().reputation;
            son2.GetComponent<Actor>().reputation = son1.GetComponent<Actor>().reputation;
            dad2.SetActive(true);
            mom2.SetActive(true);
            son2.SetActive(true);
        }
    }

    public void EndGame(bool dadKickedOut)
    {
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (dadKickedOut)
        {
            end3.SetActive(true);
            audioSource.PlayOneShot(DoorAudio);
        }
        else
        {
            bool momRep = mom2.GetComponent<Actor>().reputation;
            bool sonRep = son2.GetComponent<Actor>().reputation;

            print(momRep + " < mom rep ! ending ! son rep > " + sonRep);

            if (momRep && sonRep) //rep kid and mom 1 or mom 1
            {
                audioSource.PlayOneShot(winAudio);
                end1.SetActive(true);
            }
            else if (sonRep) //rep kid 1
            {
                audioSource.PlayOneShot(winAudio);
                end2.SetActive(true);
            }
            else // rep kid and mom 0
            {
                audioSource.PlayOneShot(loseAudio);
                end3.SetActive(true);
            }
        }
    }
}
