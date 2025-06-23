using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class GameManager_Jumping : MonoBehaviour
{
    public static GameManager_Jumping Instance { get; set; }

    // Inputs
    [Header("Inputs")]
    [SerializeField] string levelType;
    [SerializeField] Transform startScreenCamPos;
    [SerializeField] Transform charScreenCamPos;
    [SerializeField] Transform gameCamPos;
    [SerializeField] GameObject extraLight;
    [SerializeField] GameObject river;

    // UI
    [Header("UI")]
    [SerializeField] GameObject commonUIScreen;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject inGameScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject missionScreen;
    [SerializeField] GameObject characterScreen;
    [SerializeField] GameObject settingsScreen;
    [SerializeField] GameObject exploreScreen;
    [SerializeField] GameObject shopScreen;
    [SerializeField] GameObject purchaseConfirmScreen;
    [SerializeField] GameObject shopCharacterScreen;
    [SerializeField] GameObject shopClothesScreen;
    [SerializeField] GameObject shopAccessoriesScreen;
    [SerializeField] List<GameObject> powerUps = new List<GameObject>();
    [SerializeField] TextMeshProUGUI startScreenHighScore;
    [SerializeField] TextMeshProUGUI topPanelCoins;
    [SerializeField] TextMeshProUGUI inGameScreenScore;
    [SerializeField] TextMeshProUGUI inGameScreenCoins;
    [SerializeField] TextMeshProUGUI inGameScreenHighScore;
    [SerializeField] TextMeshProUGUI pauseScreenScore;
    [SerializeField] TextMeshProUGUI endScreenScore;
    [SerializeField] TextMeshProUGUI endScreenCoins;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject buyButton;
    [SerializeField] GameObject selectButton;
    [SerializeField] GameObject selectedText;
    [SerializeField] GameObject pauseButton;

    // Bools
    [Header("Bools")]
    public bool startGame = false;
    public bool paused = false;
    public bool playerDead = false;
    public static bool startTutorial = true;
    public bool tutorialStarted = false;
    public bool isInverted = false;
    public bool doubled = false;
    public bool obstacleRush = false;
    public bool magicStoneSpawned = false;
    bool onceDead = true;

    // Tutorial Essentials
    [Header("Tutorial Essentials")]
    [SerializeField] List<GameObject> tutorialColliders = new List<GameObject>();
    [SerializeField] List<GameObject> tutorialStones = new List<GameObject>();

    // Private Variables
    GameObject player;
    GameObject playerBody;
    GameObject playerRagdoll;
    [SerializeField] List<string> characters = new List<string>();
    [SerializeField] List<string> clothes = new List<string>();
    [SerializeField] List<string> accessory = new List<string>();
    Rigidbody rb;
    Animator animator;
    ForwardMovement_Jumping forwardMovement_Jumping;
    float score = 0f;
    int displayScore = 0;
    int coins = 0;
    int coinsCurr = 0;
    int shopBuyAmount = 0;
    string purchaseType;
    string shopBuyName;
    public Characters currChar;
    public ShopCharacter currShopChar;
    public ShopClothes currShopCloth;
    public ShopAccessories currShopAccessory;
    string coinType;
    int count = 0;
    float currTimeScale;
    float highScore;
    private bool isTransitioning = false;

    // Events
    public static event Action<QuestType, int> questUpdater;
    public static event Action questUpdaterPlayerDied;
    public static event Action<bool, string> playerMovement;
    public static event Action<GameObject, Animator> updatePlayerMove;
    public static event Action<bool, float> stoneSpawning;
    public static event Action shopPopulate;
    public static event Action<int> buyCheck;
    public static event Action<Characters> charManager;
    public static event Action<Characters> charCheck;
    public static event Action characterSave;

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

    void Start()
    {
        if(!AnalyticsEvents.gameStarted) StartCoroutine(StartFirebaseEvent());
        player = GameObject.FindGameObjectWithTag("Player");
        playerBody = player.transform.GetChild(player.transform.childCount - 2).gameObject;
        playerRagdoll = player.transform.GetChild(player.transform.childCount - 1).gameObject;
        rb = player.GetComponent<Rigidbody>();
        animator = playerBody.GetComponent<Animator>();
        forwardMovement_Jumping = player.GetComponent<ForwardMovement_Jumping>();

        if (PlayerPrefs.HasKey("HighScore - " + levelType))
        {
            startScreenHighScore.text = Mathf.FloorToInt(PlayerPrefs.GetFloat("HighScore - " + levelType, highScore)).ToString();
            inGameScreenHighScore.text = Mathf.FloorToInt(PlayerPrefs.GetFloat("HighScore - " + levelType, highScore)).ToString();
        }

        if (PlayerPrefs.HasKey("Coins"))
        {
            coins = PlayerPrefs.GetInt("Coins", 0);
            topPanelCoins.text = coins.ToString();
        }

        if (PlayerPrefs.GetInt("Tutorial - " + levelType, 1) == 1)
        {
            startTutorial = true;
            playerMovement?.Invoke(false, "Left");
            stoneSpawning?.Invoke(true, 60f);
        }
        else
        {
            startTutorial = false;
            playerMovement?.Invoke(true, "Centre");
            // playerMovement_Jumping.enabled = true;
            // playerMovement_Jumping.currDir = "Centre";
            foreach (GameObject obj in tutorialColliders)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in tutorialStones)
            {
                obj.SetActive(false);
            }
            // stoneSpawner_Jumping.spawnPos = 13f;
            stoneSpawning?.Invoke(true, 13f);
        }
        // stoneSpawner_Jumping.StartSpawning();

        // PlayerPrefs.DeleteKey("CharacterList");
        // PlayerPrefs.DeleteKey("ClothesList");
        // PlayerPrefs.DeleteKey("AccessoryList");

        // characters.Add("Bear");
        // PlayerPrefs.SetString("CharactersList", string.Join(",", characters));

        string data = PlayerPrefs.GetString("CharactersList");
        if (data != "") characters = new List<string>(data.Split(','));
        string clothData = PlayerPrefs.GetString("ClothesList");
        if (clothData != "") clothes = new List<string>(clothData.Split(','));
        string accessoryData = PlayerPrefs.GetString("AccessoryList");
        if (accessoryData != "") accessory = new List<string>(accessoryData.Split(','));

        shopPopulate?.Invoke();
        Debug.Log(startTutorial);
        playerDead = false;
        Time.timeScale = 1.3f;
    }

    IEnumerator StartFirebaseEvent()
    {
        yield return new WaitUntil(() => FirebaseInit.isFirebaseReady);
        AnalyticsEvents.GameStartEvent();
    }

    void Update()
    {
        if (!startTutorial && startGame)
        {
            // score += Time.deltaTime * 10f;
            // score = Mathf.FloorToInt(score);
            // inGameScreenScore.text = score.ToString();
            if (doubled)
            {
                score += Time.deltaTime * 10f * 2;
            }
            else
            {
                score += Time.deltaTime * 10f;
            }
            int newDisplayScore = Mathf.FloorToInt(score);

            if (newDisplayScore > displayScore)
            {
                int delta = newDisplayScore - displayScore;
                questUpdater?.Invoke(QuestType.Score, delta);
                // questManager.AddProgress(QuestType.Score, delta);
                displayScore = newDisplayScore;
            }

            inGameScreenScore.text = displayScore.ToString();
            // questManager.AddProgress(QuestType.Score, displayScore);
        }
    }

    public void UpdateChild(GameObject a, GameObject b)
    {
        playerBody = a;
        playerRagdoll = b;
        animator = playerBody.GetComponent<Animator>();
        updatePlayerMove?.Invoke(playerBody, animator);
    }

    public void UpdateCoins(int amount)
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
        inGameScreenCoins.text = coinsCurr.ToString();
        questUpdater?.Invoke(QuestType.Coins, amount);
        // questManager.AddProgress(QuestType.Coins, amount);
    }

    public void UpdateTimeSpeed()
    {
        count++;
        if (count % 15 == 0)
        {
            Time.timeScale += 0.1f * Time.timeScale;
        }

        if (Time.timeScale > 2.5f) Time.timeScale = 2.5f;
        // questManager.AddProgress(QuestType.Score, Mathf.FloorToInt(score));
        animator.speed = 1 / (Time.timeScale);
        Debug.Log("Time : " + Time.timeScale);
    }

    public void EndGame()
    {
        if (onceDead)
        {
            AnalyticsEvents.LevelCompleteEvent(levelType);
            onceDead = false;
            // questManager.AddProgress(QuestType.Score, Mathf.FloorToInt(score));
            playerDead = true;
            Camera.main.transform.SetParent(null);
            coins += coinsCurr;
            Debug.LogError(coins);
            PlayerPrefs.SetInt("Coins", coins);
            questUpdaterPlayerDied?.Invoke();
            characterSave?.Invoke();
            rb.linearVelocity = new Vector3(0f, 0f, 0f);
            playerBody.SetActive(false);
            forwardMovement_Jumping.enabled = false;
            playerRagdoll.SetActive(true);
            Invoke("StopGame", 1f);
        }
    }

    void StopGame()
    {
        Time.timeScale = 0f;
        endScreen.SetActive(true);
        inGameScreen.SetActive(false);
        endScreenScore.text = Mathf.FloorToInt(score).ToString();
        endScreenCoins.text = coinsCurr.ToString();
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
        AnalyticsEvents.LevelStartEvent(levelType);
        // AnalyticsEvents.LevelNameEvent();
        StartCoroutine(StartingAnim());
        startScreen.SetActive(false);
        commonUIScreen.SetActive(false);
        inGameScreen.SetActive(true);
        // inGameScreenCoins.text = coins.ToString();
        StartCoroutine(DisappearPowerUp());
    }

    IEnumerator StartingAnim()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        animator.SetTrigger("StartJump");

        DOTween.Kill(Camera.main.transform);

        Sequence camSeq = DOTween.Sequence();
        camSeq.Append(Camera.main.transform.DOMove(gameCamPos.position, 3f).SetEase(Ease.InOutSine));
        camSeq.Join(Camera.main.transform.DORotate(gameCamPos.eulerAngles, 3f).SetEase(Ease.InOutSine));
        camSeq.OnComplete(() => isTransitioning = false);

        player.transform.DOMoveX(0f, 3f).SetEase(Ease.InOutSine);
        playerBody.transform.DORotate(Vector3.zero, 3f).SetEase(Ease.InOutSine);

        if (extraLight != null) extraLight.SetActive(false);

        yield return new WaitForSeconds(3f);

        rb.useGravity = true;
        startGame = true;
        river.transform.SetParent(player.transform);
        river.transform.SetSiblingIndex(0);
        animator.SetTrigger("Run");
    }

    IEnumerator DisappearPowerUp()
    {
        int countdown = 5;
        while (countdown > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            countdown--;
        }

        foreach (GameObject powerUp in powerUps)
        {
            powerUp.SetActive(false);
        }
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
    //     stoneSpawning?.Invoke(false, 60f);
    //     // stoneSpawner_Jumping.spawnPos = 60f;
    // }

    public void MissionsButton()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        missionScreen.SetActive(true);

        RectTransform child0 = missionScreen.transform.GetChild(0).GetComponent<RectTransform>();
        RectTransform child1 = missionScreen.transform.GetChild(1).GetComponent<RectTransform>();

        child0.localScale = Vector3.zero;
        child1.localScale = Vector3.zero;

        DOTween.Kill(child0);
        DOTween.Kill(child1);

        Sequence seq = DOTween.Sequence();
        seq.Append(child0.DOScale(1.08972f, 0.8f).SetEase(Ease.OutBack));
        seq.Join(child1.DOScale(1f, 0.8f).SetEase(Ease.OutBack));
        seq.OnComplete(() => isTransitioning = false);

        startScreen.SetActive(false);
        settingsScreen.SetActive(false);
    }

    public void BackButton(string screen)
    {
        if (isTransitioning) return;
        isTransitioning = true;

        switch (screen)
        {
            case "Mission":
                RectTransform child0 = missionScreen.transform.GetChild(0).GetComponent<RectTransform>();
                RectTransform child1 = missionScreen.transform.GetChild(1).GetComponent<RectTransform>();
                DOTween.Kill(child0);
                DOTween.Kill(child1);

                Sequence seq = DOTween.Sequence();
                seq.Append(child0.DOScale(0f, 0.3f));
                seq.Join(child1.DOScale(0f, 0.3f));
                seq.AppendInterval(0.2f);
                seq.OnComplete(() =>
                {
                    missionScreen.SetActive(false);
                    isTransitioning = false;
                });
                startScreen.SetActive(true);
                break;

            case "Character":
                string currshopBuyName = "Bear";
                if (PlayerPrefs.HasKey("CurrentCharacter"))
                    currshopBuyName = PlayerPrefs.GetString("CurrentCharacter");

                if (currChar.characterName != currshopBuyName)
                    charCheck?.Invoke(CharacterManager.OnFindCharacter?.Invoke(currshopBuyName));

                RectTransform charchild1 = characterScreen.transform.GetChild(1).GetComponent<RectTransform>();
                RectTransform charchild2 = characterScreen.transform.GetChild(2).GetComponent<RectTransform>();
                DOTween.Kill(charchild1);
                DOTween.Kill(charchild2);
                DOTween.Kill(Camera.main.transform);

                Sequence charseq = DOTween.Sequence();
                charseq.Append(charchild1.DOAnchorPosY(-Screen.height, 1f));
                charseq.Join(charchild2.DOAnchorPosY(-Screen.height, 1f));
                charseq.Join(Camera.main.transform.DOMove(startScreenCamPos.position, 1f));
                charseq.Join(Camera.main.transform.DORotate(startScreenCamPos.eulerAngles, 1f));
                charseq.AppendCallback(() =>
                {
                    characterScreen.SetActive(false);
                    isTransitioning = false;
                });
                startScreen.SetActive(true);
                break;

            case "Explore":
                RectTransform rectTransformExplore = exploreScreen.transform.GetChild(0).GetComponent<RectTransform>();
                DOTween.Kill(rectTransformExplore);

                DOTween.Sequence()
                .Append(rectTransformExplore.DOAnchorPos(new Vector2(1000, -130f), 0.5f))
                .AppendCallback(() =>
                {
                    exploreScreen.SetActive(false);
                    isTransitioning = false;
                });
                startScreen.SetActive(true);
                break;

            case "Shop":
                purchaseConfirmScreen.SetActive(false);
                RectTransform rectTransform1 = shopScreen.transform.GetChild(0).GetComponent<RectTransform>();
                RectTransform rectTransform2 = shopScreen.transform.GetChild(1).GetComponent<RectTransform>();
                RectTransform rectTransform3 = shopScreen.transform.GetChild(2).GetComponent<RectTransform>();
                DOTween.Kill(rectTransform1);
                DOTween.Kill(rectTransform2);
                DOTween.Kill(rectTransform3);
                DOTween.Kill(Camera.main.transform);

                if (!paused)
                {
                    DOTween.Sequence()
                    .Append(rectTransform1.DOAnchorPos(new Vector2(1000, -154.9013f), 0.5f))
                    .Join(rectTransform2.DOAnchorPos(new Vector2(1000, -154.9013f), 0.5f))
                    .Join(rectTransform3.DOAnchorPos(new Vector2(1000, 54.48608f), 0.5f))
                    .Join(Camera.main.transform.DOMove(startScreenCamPos.position, 1f))
                    .Join(Camera.main.transform.DORotate(startScreenCamPos.eulerAngles, 1f))
                    .AppendCallback(() =>
                    {
                        shopScreen.SetActive(false);
                        isTransitioning = false;
                    });
                    startScreen.SetActive(true);
                }
                else
                {
                    DOTween.Sequence()
                    .SetUpdate(true)
                    .Append(rectTransform1.DOAnchorPos(new Vector2(1000, -154.9013f), 0.5f).SetUpdate(true))
                    .Join(rectTransform2.DOAnchorPos(new Vector2(1000, -154.9013f), 0.5f).SetUpdate(true))
                    .Join(rectTransform3.DOAnchorPos(new Vector2(1000, 54.48608f), 0.5f).SetUpdate(true))
                    .AppendCallback(() =>
                    {
                        shopScreen.SetActive(false);
                        isTransitioning = false;
                    });
                }
                break;

            case "Settings":
                startButton.SetActive(true);
                isTransitioning = false;
                break;

            default:
                isTransitioning = false;
                break;
        }

        RectTransform settingschild = settingsScreen.transform.GetChild(0).GetComponent<RectTransform>();
        DOTween.Kill(settingschild);
        settingschild.DOScale(0f, 0.3f).SetUpdate(true).OnComplete(() =>
        {
            settingsScreen.SetActive(false);
        });

        startButton.SetActive(true);
        if (paused) startScreen.SetActive(false);
    }

    public void MeButton()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        characterScreen.SetActive(true);
        startScreen.SetActive(false);
        settingsScreen.SetActive(false);

        DOTween.Kill(Camera.main.transform);

        Sequence camSeq = DOTween.Sequence();
        camSeq.Join(Camera.main.transform.DOMove(charScreenCamPos.position, 1f).SetEase(Ease.InOutSine));
        camSeq.Join(Camera.main.transform.DORotate(charScreenCamPos.eulerAngles, 1f).SetEase(Ease.InOutSine));

        RectTransform rectTransform1 = characterScreen.transform.GetChild(1).GetComponent<RectTransform>();
        DOTween.Kill(rectTransform1);
        RectTransform rectTransform2 = characterScreen.transform.GetChild(2).GetComponent<RectTransform>();
        DOTween.Kill(rectTransform2);

        rectTransform1.anchoredPosition = new Vector2(0, -Screen.height);
        rectTransform2.anchoredPosition = new Vector2(0, -Screen.height);

        Sequence charSeq = DOTween.Sequence();
        charSeq.Append(rectTransform1.DOAnchorPos(new Vector2(0, 0), 1f).SetEase(Ease.OutExpo));
        charSeq.Join(rectTransform2.DOAnchorPos(new Vector2(0, 50), 1f).SetEase(Ease.OutExpo));
        charSeq.OnComplete(() => isTransitioning = false);
    }

    public void ExploreButton()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        settingsScreen.SetActive(false);
        exploreScreen.SetActive(true);

        RectTransform rectTransform1 = exploreScreen.transform.GetChild(0).GetComponent<RectTransform>();
        DOTween.Kill(rectTransform1);
        rectTransform1.anchoredPosition = new Vector2(1000, -150);

        rectTransform1.DOAnchorPos(new Vector2(0, -136.0349f), 0.5f)
            .OnComplete(() => isTransitioning = false);

        startScreen.SetActive(false);
    }

    public void ShopButton()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        buyCheck?.Invoke(coins);
        shopScreen.SetActive(true);

        RectTransform rectTransform1 = shopScreen.transform.GetChild(0).GetComponent<RectTransform>();
        DOTween.Kill(rectTransform1);
        RectTransform rectTransform2 = shopScreen.transform.GetChild(1).GetComponent<RectTransform>();
        DOTween.Kill(rectTransform2);
        RectTransform rectTransform3 = shopScreen.transform.GetChild(2).GetComponent<RectTransform>();
        DOTween.Kill(rectTransform3);

        rectTransform1.anchoredPosition = new Vector2(1000, -150);
        rectTransform2.anchoredPosition = new Vector2(1000, -150);
        rectTransform3.anchoredPosition = new Vector2(1000, 54.48608f);

        Sequence seq = DOTween.Sequence().SetUpdate(true);

        seq.Append(rectTransform1.DOAnchorPos(new Vector2(0, -154.9013f), 0.5f).SetUpdate(true));
        seq.Join(rectTransform2.DOAnchorPos(new Vector2(0, -154.9013f), 0.5f).SetUpdate(true));
        seq.Join(rectTransform3.DOAnchorPos(new Vector2(0, 54.48608f), 0.5f).SetUpdate(true));
        seq.OnComplete(() => isTransitioning = false);

        missionScreen.SetActive(false);
        characterScreen.SetActive(false);
        exploreScreen.SetActive(false);
        settingsScreen.SetActive(false);
        startScreen.SetActive(false);
    }

    public void ShopInsideButton(string screen)
    {
        switch (screen)
        {
            case "Character":
                shopCharacterScreen.SetActive(true);
                shopClothesScreen.SetActive(false);
                shopAccessoriesScreen.SetActive(false);
                break;

            case "Clothes":
                shopCharacterScreen.SetActive(false);
                shopClothesScreen.SetActive(true);
                shopAccessoriesScreen.SetActive(false);
                break;

            case "Accessories":
                shopCharacterScreen.SetActive(false);
                shopClothesScreen.SetActive(false);
                shopAccessoriesScreen.SetActive(true);
                break;

            default:
                break;
        }
    }

    public void CharacterBuyButton()
    {
        Debug.Log("Clicked");
        if (coins >= currChar.cost)
        {
            Debug.Log("Bought");
            coins -= currChar.cost;
            topPanelCoins.text = coins.ToString();
            PlayerPrefs.SetInt("Coins", coins);
            PlayerPrefs.SetString("CurrentCharacter", currChar.characterName);
            currChar.purchased = true;
            currChar.selected = true;

            buyButton.SetActive(false);
            selectButton.SetActive(false);
            selectedText.SetActive(true);
            charManager?.Invoke(currChar);
        }
    }

    public void SelectButton()
    {
        selectButton.SetActive(false);
        selectedText.SetActive(true);
        currChar.selected = true;
        PlayerPrefs.SetString("CurrentCharacter", currChar.characterName);
        charManager?.Invoke(currChar);
    }

    public void ShopCharacterBuyButton(ShopCharacter shopCharacter)
    {
        if (isTransitioning) return;
        isTransitioning = true;

        purchaseConfirmScreen.SetActive(true);
        RectTransform rectTransform = purchaseConfirmScreen.transform.GetChild(0).GetComponent<RectTransform>();
        DOTween.Kill(rectTransform);
        rectTransform.localScale = Vector3.zero;

        rectTransform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() => isTransitioning = false);

        shopBuyAmount = shopCharacter.cost;
        shopBuyName = shopCharacter.characterName;
        currShopChar = shopCharacter;
        coinType = shopCharacter.costType.ToString();
        purchaseType = "Character";

        purchaseConfirmScreen.transform.GetChild(0).GetChild(0)
            .GetComponent<TextMeshProUGUI>().text =
            $"Are you sure you want to unlock {shopBuyName} for {shopBuyAmount} {coinType}?";
    }

    public void ShopClothesBuyButton(ShopClothes shopClothes)
    {
        if (isTransitioning) return;
        isTransitioning = true;

        purchaseConfirmScreen.SetActive(true);
        RectTransform rectTransform = purchaseConfirmScreen.transform.GetChild(0).GetComponent<RectTransform>();
        DOTween.Kill(rectTransform);
        rectTransform.localScale = Vector3.zero;

        rectTransform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() => isTransitioning = false);

        shopBuyAmount = shopClothes.cost;
        shopBuyName = shopClothes.clothName;
        currShopCloth = shopClothes;
        coinType = shopClothes.costType.ToString();
        purchaseType = "Clothes";

        purchaseConfirmScreen.transform.GetChild(0).GetChild(0)
            .GetComponent<TextMeshProUGUI>().text =
            $"Are you sure you want to unlock {shopBuyName} for {shopBuyAmount} {coinType}?";
    }

    public void ShopAccessoriesBuyButton(ShopAccessories shopAccessories)
    {
        if (isTransitioning) return;
        isTransitioning = true;

        purchaseConfirmScreen.SetActive(true);
        RectTransform rectTransform = purchaseConfirmScreen.transform.GetChild(0).GetComponent<RectTransform>();
        DOTween.Kill(rectTransform);
        rectTransform.localScale = Vector3.zero;

        rectTransform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() => isTransitioning = false);

        shopBuyAmount = shopAccessories.cost;
        shopBuyName = shopAccessories.accessoriesName;
        currShopAccessory = shopAccessories;
        coinType = shopAccessories.costType.ToString();
        purchaseType = "Accessory";

        purchaseConfirmScreen.transform.GetChild(0).GetChild(0)
            .GetComponent<TextMeshProUGUI>().text =
            $"Are you sure you want to unlock {shopBuyName} for {shopBuyAmount} {coinType}?";
    }

    public void ConfirmYesButton()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        coins -= shopBuyAmount;
        topPanelCoins.text = coins.ToString();
        PlayerPrefs.SetInt("Coins", coins);

        RectTransform rect = purchaseConfirmScreen.transform.GetChild(0).GetComponent<RectTransform>();
        DOTween.Kill(rect);
        rect.DOScale(0f, 0.3f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                purchaseConfirmScreen.SetActive(false);
                isTransitioning = false;
            });

        if (purchaseType == "Clothes")
        {
            clothes.Add(shopBuyName);
            PlayerPrefs.SetString("ClothesList", string.Join(",", clothes));
            currShopCloth.purchased = true;
        }
        else if (purchaseType == "Accessory")
        {
            accessory.Add(shopBuyName);
            PlayerPrefs.SetString("AccessoryList", string.Join(",", accessory));
            currShopAccessory.purchased = true;
        }
        else
        {
            characters.Add(shopBuyName);
            PlayerPrefs.SetString("CharactersList", string.Join(",", characters));
            currShopChar.purchased = true;
        }

        buyCheck?.Invoke(coins);
    }

    public void ConfirmNoButton()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        RectTransform rect = purchaseConfirmScreen.transform.GetChild(0).GetComponent<RectTransform>();
        DOTween.Kill(rect);
        rect.DOScale(0f, 0.3f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                purchaseConfirmScreen.SetActive(false);
                isTransitioning = false;
            });
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void RestartButton()
    {
        startGame = false;
        StopAllCoroutines();
        Time.timeScale = 1f;
        paused = false;
        PlayerPrefs.SetInt("Coins", coins);
        SceneManager.LoadScene(levelType);
    }

    public void SettingsButton()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        settingsScreen.SetActive(true);
        RectTransform rectTransform = settingsScreen.transform.GetChild(0).GetComponent<RectTransform>();
        DOTween.Kill(rectTransform);
        rectTransform.localScale = Vector3.zero;

        rectTransform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() => isTransitioning = false);

        startButton.SetActive(false);
    }

    public void PauseButton()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        paused = true;
        inGameScreen.SetActive(false);

        CanvasGroup canvasGroup = pauseScreen.GetComponent<CanvasGroup>();
        DOTween.Kill(canvasGroup);
        canvasGroup.alpha = 0f;
        pauseScreen.SetActive(true);
        canvasGroup.DOFade(1f, 0.3f).SetUpdate(true).OnComplete(() => isTransitioning = false);

        commonUIScreen.SetActive(true);
        pauseButton.SetActive(false);
        pauseScreenScore.text = Mathf.FloorToInt(score).ToString();
        currTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void ContinueButton()
    {
        StartCoroutine(ResumeWithCountdown());
    }

    private IEnumerator ResumeWithCountdown()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        CanvasGroup canvasGroup = pauseScreen.GetComponent<CanvasGroup>();
        DOTween.Kill(canvasGroup);
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0f, 0.3f).SetUpdate(true).OnComplete(() => { pauseScreen.SetActive(false); });

        inGameScreen.SetActive(true);
        commonUIScreen.SetActive(false);
        settingsScreen.SetActive(false);
        countdownText.gameObject.SetActive(true);

        int countdown = 3;
        while (countdown > 0)
        {
            countdownText.text = "Continuing in " + countdown.ToString() + "...";
            yield return new WaitForSecondsRealtime(1f);
            countdown--;
        }

        countdownText.gameObject.SetActive(false);
        pauseButton.SetActive(true);

        Time.timeScale = currTimeScale;
        paused = false;

        isTransitioning = false;
    }
    
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            AnalyticsEvents.GameOverEvent();
        }
    }

    void OnApplicationQuit()
    {
        AnalyticsEvents.GameOverEvent();
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            AnalyticsEvents.GameOverEvent();
        }
    }
}
