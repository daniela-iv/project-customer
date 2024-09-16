using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private FadeScript fade;

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
    }
}
