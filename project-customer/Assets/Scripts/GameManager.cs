using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private FadeScript fade;

    [SerializeField]
    private GameObject dad1;
    [SerializeField]
    private GameObject dad2;
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


    [SerializeField]
    private float dadTimer;


    private GameState gameState = GameState.firstStage;

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
        fade.fadeIn();

        gameState = GameState.secondStage;
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        float timer = dadTimer;
        while (timer > 0f)
        {
            timer -= 1f;
            print(timer);
            yield return new WaitForSecondsRealtime(1f);
        }
        EndGame(true);
        StopCoroutine("Timer");

    }

    public void changeCharacters()
    {
        dad1.SetActive(false);
        mom1.SetActive(false);
        son1.SetActive(false);
        dad2.SetActive(true);
        mom2.SetActive(true);
        son2.SetActive(true);
    }

    public void EndGame(bool dadKickedOut)
    {
        Time.timeScale = 0;
        if (dadKickedOut)
        {
            end3.SetActive(true);
        }
        else
        {
            if (false) //rep kid and mom 1 or mom 1
            {
                end1.SetActive(true);
            }
            else if (false) //rep kid 1
            {
                end2.SetActive(true);
            }
            else // rep kid and mom 0
            {
                end3.SetActive(true);
            }
        }
    }
}
