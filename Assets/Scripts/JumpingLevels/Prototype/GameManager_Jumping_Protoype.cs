using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager_Jumping_Prototype : MonoBehaviour
{
    public static GameManager_Jumping_Prototype Instance{get; set;}

    // Inputs
    [Header("Inputs")]
    [SerializeField] string levelType;

    // UI
    [Header("UI")]
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject inGameScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject endScreen;
    [SerializeField] TextMeshProUGUI startScreenHighScore;
    [SerializeField] TextMeshProUGUI inGameScreenScore;
    [SerializeField] TextMeshProUGUI inGameScreenCoins;
    [SerializeField] TextMeshProUGUI pauseScreenScore;
    [SerializeField] TextMeshProUGUI endScreenScore;

    // Bools
    [Header("Bools")]
    public bool startGame = false;
    public bool paused = false;
    public bool playerDead = false;
    public static bool startTutorial = true;
    public bool tutorialStarted = false;

    // Tutorial Essentials
    [Header("Tutorial Essentials")]
    [SerializeField] GameObject tutorialCollider1;
    [SerializeField] GameObject tutorialCollider2;
    [SerializeField] GameObject tutorialCollider3;
    [SerializeField] GameObject tutorialCollider4;
    [SerializeField] GameObject tutorialStone1;
    [SerializeField] GameObject tutorialStone2;
    [SerializeField] GameObject tutorialStone3;
    [SerializeField] GameObject tutorialStone4;

    // Private Variables
    GameObject player;
    GameObject playerBody;
    GameObject playerRagdoll;
    Rigidbody rb;
    Animator animator;
    ForwardMovement_Jumping_Prototype forwardMovement_Jumping_Prototype;
    PlayerMovement_Jumping_Prototype playerMovement_Jumping_Prototype;
    StoneSpawner_Jumping_Prototype stoneSpawner_Jumping_Prototype;
    float score = 0f;
    int displayScore = 0;
    int coins = 0;
    int coinsCurr = 0;
    int count = 0;
    float currTimeScale;
    float highScore;


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
    }

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        playerBody = player.transform.GetChild(0).gameObject;
        playerRagdoll = player.transform.GetChild(1).gameObject;
        rb = player.GetComponent<Rigidbody>();
        animator = playerBody.GetComponent<Animator>();
        forwardMovement_Jumping_Prototype = player.GetComponent<ForwardMovement_Jumping_Prototype>();
        playerMovement_Jumping_Prototype = player.GetComponent<PlayerMovement_Jumping_Prototype>();
        stoneSpawner_Jumping_Prototype = GameObject.Find("StoneSpawner").GetComponent<StoneSpawner_Jumping_Prototype>();

        if (PlayerPrefs.HasKey("HighScore - " + levelType))
        {
            startScreenHighScore.text = "HighScore : " + Mathf.FloorToInt(PlayerPrefs.GetFloat("HighScore - " + levelType, highScore)).ToString();
        }

        if (PlayerPrefs.HasKey("Coins"))
        {
            coins = PlayerPrefs.GetInt("Coins", 0);
        }

        if (PlayerPrefs.GetInt("Tutorial - " + levelType, 1) == 1)
        {
            startTutorial = true;
            stoneSpawner_Jumping_Prototype.spawnPos = 60f;
            stoneSpawner_Jumping_Prototype.StartSpawning();
            playerMovement_Jumping_Prototype.enabled = false;
            playerMovement_Jumping_Prototype.currDir = "Left";
        }
        else
        {
            startTutorial = false;
            playerMovement_Jumping_Prototype.enabled = true;
            playerMovement_Jumping_Prototype.currDir = "Centre";
            tutorialCollider1.SetActive(false);
            tutorialCollider2.SetActive(false);
            tutorialCollider3.SetActive(false);
            tutorialCollider4.SetActive(false);
            tutorialStone1.SetActive(false);
            tutorialStone2.SetActive(false);
            tutorialStone3.SetActive(false);
            tutorialStone4.SetActive(false);
            stoneSpawner_Jumping_Prototype.spawnPos = 13f;
            stoneSpawner_Jumping_Prototype.StartSpawning();
        }
        Debug.Log(startTutorial);
        playerDead = false;
        Time.timeScale = 1.3f;
    }

    void Update(){
        if (!startTutorial && startGame)
        {
            // score += Time.deltaTime * 10f;
            // score = Mathf.FloorToInt(score);
            // inGameScreenScore.text = score.ToString();
            
            score += Time.deltaTime * 10f;
            int newDisplayScore = Mathf.FloorToInt(score);

            if (newDisplayScore > displayScore)
            {
                int delta = newDisplayScore - displayScore;
                // questManager.AddProgress(QuestType.Score, delta);
                displayScore = newDisplayScore;
            }

            inGameScreenScore.text = "Score : " + displayScore.ToString();
            // questManager.AddProgress(QuestType.Score, displayScore);
        }
    }

    public void UpdateCoins(string type, int amount)
    {
        // if (type == "Gain")
        // {
        //     coinsCurr += amount;
        //     inGameScreenCoins.text = coinsCurr.ToString();
        //     questManager.AddProgress(QuestType.Coins, amount);
        // }
        // else
        // {
        //     coinsCurr -= amount;
        //     inGameScreenCoins.text = coinsCurr.ToString();
        //     questManager.AddProgress(QuestType.Coins, amount);
        // }
        coinsCurr += amount;
        inGameScreenCoins.text = "Coins : " + coinsCurr.ToString();
        // questManager.AddProgress(QuestType.Coins, amount);
    }

    public void UpdateTimeSpeed(){
        count++;
        if(count % 15 == 0){
            Time.timeScale += 0.1f * Time.timeScale;
        }

        if(Time.timeScale > 2.5f) Time.timeScale = 2.5f;
        // questManager.AddProgress(QuestType.Score, Mathf.FloorToInt(score));
        animator.speed = 1/(Time.timeScale);
        Debug.Log("Time : " + Time.timeScale);
    }

    public void EndGame(){
        // questManager.AddProgress(QuestType.Score, Mathf.FloorToInt(score));
        playerDead = true;
        Camera.main.transform.SetParent(null);
        coins += coinsCurr;
        PlayerPrefs.SetInt("Coins", coins);
        rb.linearVelocity = new Vector3(0f,0f,0f);
        playerBody.SetActive(false);
        forwardMovement_Jumping_Prototype.enabled = false;
        playerRagdoll.SetActive(true);
        Invoke("StopGame", 1f);
    }

    void StopGame()
    {
        Time.timeScale = 0f;
        endScreen.SetActive(true);
        inGameScreen.SetActive(false);
        endScreenScore.text = "Score : " + Mathf.FloorToInt(score).ToString();
        if (PlayerPrefs.HasKey("HighScore - " + levelType))
        {
            float highScore_local = PlayerPrefs.GetFloat("HighScore - " + levelType, highScore);
            if (highScore_local < score)
            {
                PlayerPrefs.SetFloat("HighScore - " + levelType, score);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("HighScore - " + levelType, score);
        }
        PlayerPrefs.Save();
    }

    public void ToggleStart(bool tutorial)
    {
        startTutorial = !tutorial;
        PlayerPrefs.SetInt("Tutorial - " + levelType, startTutorial ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void StartButton()
    {
        startGame = true;
        startScreen.SetActive(false);
        inGameScreen.SetActive(true);
        animator.SetTrigger("Run");
        // inGameScreenCoins.text = coins.ToString();
    }

    // public void StartWithTutorialButton()
    // {
    //     startGame = true;
    //     startScreen.SetActive(false);
    //     inGameScreen.SetActive(true);
    //     animator.SetTrigger("Run");
    //     startTutorial = true;
    //     PlayerPrefs.SetInt("Tutorial - " + levelType, startTutorial ? 1 : 0);
    //     PlayerPrefs.Save();
    //     if (startTutorial && !tutorialStarted)
    //     {
    //         tutorialStarted = true;
    //     }
    //     playerMovement?.Invoke(false, "Left");
    //     // playerMovement_Jumping.enabled = false;
    //     tutorialCollider1.SetActive(true);
    //     tutorialCollider2.SetActive(true);
    //     tutorialCollider3.SetActive(true);
    //     tutorialCollider4.SetActive(true);
    //     tutorialStone1.SetActive(true);
    //     tutorialStone2.SetActive(true);
    //     tutorialStone3.SetActive(true);
    //     tutorialStone4.SetActive(true);
    //     stoneSpawner?.Invoke(false, 60f);
    //     // stoneSpawner_Jumping.spawnPos = 60f;
    // }

    public void ExitButton()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void RestartButton(){
        startGame = false;
        StopAllCoroutines();
        Time.timeScale = 1f;
        paused = false;
        SceneManager.LoadScene("Jungle - Prototype");
    }

    public void PauseButton()
    {
        paused = true;
        inGameScreen.SetActive(false);
        pauseScreen.SetActive(true);
        pauseScreenScore.text = "Score : " + Mathf.FloorToInt(score).ToString();
        currTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void ContinueButton()
    {
        pauseScreen.SetActive(false);
        inGameScreen.SetActive(true);
        Time.timeScale = currTimeScale;
        paused = false;
    }
}
