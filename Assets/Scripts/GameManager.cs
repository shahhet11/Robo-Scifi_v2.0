using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    #region CLASS INSTANCE
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public GameObject MainPlayer;
    public GameObject Torch;
    public Renderer MainPlayerSkin;
    public Material[] MainPlayerMaterial;
    public int CurrentLevel;
    public PlayerHealth PlayerHealth;
    public CameraShaker CameraShaker;
    public Weapon PlayerWeapon;
    public UIManager UIManager;
    [Header("ANIMATIONS")]
    public Animation LightFade;
    public Animation GroundBlack;

    public bool allowSlowTime;

    public int maxHealthPlayer;
    public int maxHealthAI;
    public float maxBoxDestroyTime;
    public float timeBetweenAttackAI;

    public float delayToOver;

    public int killCounter;
    public int minUpgradeKills;
    public int minHealthKills;
    public int minBlackoutkills;

    [Header("WAVE SETTINGS")]
    public List<Transform> WavePosList;
    public int[] WaveKills;
    public int currentWave;
    public int minWaveKills;
    public float waveTime;
    public bool isDelayWave;

    private float storeTimeScale;
    private bool doSlow;

    [Header("PLAYER SETTINGS")]
    #region
    public PlayerMove PlayerMove;
    public ParticleSystem ChargeParticles;
    public ParticleSystem AftershockParticles;
    #endregion

    [Header("AI SETTINGS")]
    #region AI

    public float NormalAISpeed;
    public float RedAISpeed;


    public List<GameObject> AiPlayers;
    public List<Transform> AiSpawnPositions;
    public List<int> AI_PerLevel;
    public int AI_countThisLevel;
    public float NextAiSpawnAfter;
    public int maxAIcount;
    public int AIWithBulletNum;

    
    public int AiCount;

    public float AiTimer;

    #endregion

    #region
    [Header("DAMAGE(IN PERCENT)")]
    public List<int> DamageList;
    public List<int> DamageListAI;
    #endregion

    #region
    [Header("PAUSE MENU")]
    public bool isPaused;
    public GameObject PauseMenu;
    #endregion

    #region
    [Header("NEXT LEVEL MENU")]
    public bool isNextLevelMenu;
    public GameObject NextLevelMenu;
    bool TimerOn;
    public TextMeshProUGUI EnergyText;
    public TextMeshProUGUI gameTimerText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI deathsText;
    public TextMeshProUGUI kill_Death_RatioText;
    public TextMeshProUGUI playerXPText;
    public TextMeshProUGUI PlayerEnergyText;
    public TextMeshProUGUI PlayerRewardText;
    float gameTimer = 0f;
    int kills = 0;
    int deaths = 0;
    int KDratio = 0;
    int XP = 0;
    int Level = 0;
    int Reward = 0;
    #endregion

    #region
    [Header("Primary Weapon Inventory")]
    public GameObject WeaponDisplayFrame;
    public Image[] highlightImages;
    public RawImage displayImage; // The RawImage to display textures
    public Texture[] textureArray; // Array of textures to cycle through
    public AmmoCustomization ammoCustomization;
    public int currentWeaponIndex = 0; // Index of the currently displayed texture
    public string[] WeaponNames;
    public Text currentWeaponName;
    public AudioSource ClickSoundSource;
    [Header("Secondary Weapon Inventory")]
    public Image[] highlightImagesSecondary;
    public RawImage displayImageSecondary; // The RawImage to display textures
    public Texture[] textureArraySecondary; // Array of textures to cycle through
    public int currentSecondaryWeaponIndex = 0; // Index of the currently displayed texture
    public string[] SecondaryWeaponNames;
    public Text currentSecondaryWeaponName;
    public Color32[] secondaryWeaponColor;
    #endregion
    public GameObject DeathScreen;
    bool isDead = false;
    void StatisticalData()
    {
        //Level
        EnergyText.text = "Success! \n Proceed to Level: " + PlayerPrefs.GetInt("CurrentLevel").ToString();
        //Kills
        killsText.text = "Kills: "+AI_countThisLevel.ToString();
        //Death
        deathsText.text = "Deaths: " + deaths.ToString();

        //KDRatio
        if(deaths > 0)
        {

        KDratio = AI_countThisLevel / deaths;
        }
        else
        {
            KDratio = AI_countThisLevel;
        }
        kill_Death_RatioText.text = "Kill / Death ratio:" + KDratio.ToString();

        //PlayerXP

        //PlayerEnergyText

        //Reward
        int reward = AI_countThisLevel * 25;
        PlayerRewardText.text = "Player Rewards: "+(reward).ToString();
        PlayerPrefs.SetInt("PlayerRewards", PlayerPrefs.GetInt("PlayerRewards") + 1);
        PlayerPrefs.SetInt("TotalGold", reward);


    }
    // Total Kills, Total Deaths, Total Score, 



    void ShowPreviousTexture()
    {
        PlayerWeapon.WeaponCollection[currentWeaponIndex].SetActive(false);
        PlayerWeapon.AmmoCollection[currentWeaponIndex].SetActive(false);
        currentWeaponIndex = (currentWeaponIndex - 1 + textureArray.Length) % textureArray.Length;
        UpdateDisplayedTexture();
    }

    void ShowNextTexture()
    {
        PlayerWeapon.WeaponCollection[currentWeaponIndex].SetActive(false);
        PlayerWeapon.AmmoCollection[currentWeaponIndex].SetActive(false);
        currentWeaponIndex = (currentWeaponIndex + 1) % textureArray.Length;
        UpdateDisplayedTexture();
    }

    void UpdateDisplayedTexture()
    {
        if(!WeaponDisplayFrame.activeSelf) WeaponDisplayFrame.SetActive(true);
        ClickSoundSource.Play();
        Debug.Log("currentWeaponIndex" + currentWeaponIndex);
        displayImage.texture = textureArray[currentWeaponIndex];
        ammoCustomization.DisplayAmmo();



        for (int i = 0; i < highlightImages.Length; i++)
        {
            if (i == currentWeaponIndex)
            {
                highlightImages[i].color = Color.green;
                currentWeaponName.text = WeaponNames[i];
                PlayerWeapon.WeaponCollection[i].SetActive(true);
                PlayerWeapon.AmmoCollection[i].SetActive(true);
            }
            else
            {
                highlightImages[i].color = Color.red;
            }
        }
    }
    void SwitchSecondaryWeapon()
    {
        PlayerWeapon.SecondaryWeaponCollection[currentSecondaryWeaponIndex].SetActive(false);
        currentSecondaryWeaponIndex = (currentSecondaryWeaponIndex + 1) % textureArraySecondary.Length;
        displayImageSecondary.texture = textureArraySecondary[currentSecondaryWeaponIndex];
        for (int i = 0; i < textureArraySecondary.Length; i++)
        {
            if (i == currentSecondaryWeaponIndex)
            {
                currentSecondaryWeaponName.text = SecondaryWeaponNames[i];
                currentSecondaryWeaponName.color = secondaryWeaponColor[i];
                PlayerWeapon.SecondaryWeaponCollection[i].SetActive(true);
            }
        }
    }
    void Start()
    {
        //GamePlayRequirements();
        //SpawnAIPlayer(1);
    }
    void TimePerMatchTaken()
    {
        if(TimerOn == true)
        {
        gameTimer += Time.deltaTime;

        int seconds = (int)(gameTimer % 60);
        int minutes = (int)(gameTimer/60) % 60;
        int hours = (int)(gameTimer / 3600) % 24;

        string timerString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
        gameTimerText.text = timerString;
        }
    }
    private void Update()
    {
    
            
            if (!isDead && AiTimer > NextAiSpawnAfter && AiCount <= AI_PerLevel[CurrentLevel])
            {
                SpawnAIPlayer(1);
                
                AiTimer = 0f;
            }
            if(AI_countThisLevel == AI_PerLevel[CurrentLevel])
            {
                AI_countThisLevel = 0;
                TimerOn = false;
                gameTimer = 0;
                isNextLevelMenu = true;
                if (isNextLevelMenu)
                {
                    //OnLevelComplete();
                }
            }
                AiTimer += Time.deltaTime;


                TimePerMatchTaken();

        if (allowSlowTime)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Time.timeScale = 0.1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;

                PlayerMove.isHoldingShift = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;

                PlayerMove.isHoldingShift = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 1f)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;

            PauseMenu.SetActive(!PauseMenu.activeSelf);
        }
        if (Input.GetMouseButtonDown(0))
        {
            WeaponDisplayFrame.SetActive(false);
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput > 0) // Scrolling up
        {
            ShowPreviousTexture();
        }
        else if (scrollInput < 0) // Scrolling down
        {
            ShowNextTexture();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Switch Secondary guns physgun and Gravitygun
            SwitchSecondaryWeapon();
        }

    }

    private void SpawnAIPlayer(int count)
    {
        if((killCounter > 0 && AIWithBulletNum > 0) && (killCounter % AIWithBulletNum == 0))     //Spawn AI With Normal Bullet
        {
            for (int i = 0; i < count; i++)
            {
                int r_pos = Random.Range(0, AiSpawnPositions.Count);

                Instantiate(AiPlayers[0], AiSpawnPositions[r_pos].position, AiSpawnPositions[r_pos].rotation);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                int r_pl = Random.Range(1, AiPlayers.Count);
                int r_pos = Random.Range(0, AiSpawnPositions.Count);
                if(AiPlayers.Count > 0)
                    Instantiate(AiPlayers[r_pl], AiSpawnPositions[r_pos].position, AiSpawnPositions[r_pos].rotation);
            }
        }
        //Debug.Log("Went iN"+ AiCount);
        AiCount += count;
        
    }

    public void InitiateUpgrade()
    {
        maxAIcount += 2;

        //StartCoroutine(SlowMotion(1, 2f, 0.5f));
    }

    public void PlayerDeath()
    {
        StartCoroutine(SlowMotion(0, 5f, 0.2f));
    }

    public IEnumerator SlowMotion(int task, float delay, float slowness)
    {
        Time.timeScale = slowness;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        float slowEndTime = Time.realtimeSinceStartup + delay;

        while (Time.realtimeSinceStartup < slowEndTime)
        {
            yield return 0;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        if(task == 0)
        {
            //SceneManager.LoadScene(0);
            onResetLevel();
        }
        else if(task == 1)
        {
            //UpgradeWeapon();
        }
    }
   
    public void UpgradeWeapon()
    {
        int w = PlayerWeapon.currentWeaponId;
        PlayerWeapon.WeaponCollection[w].SetActive(false);

        w++;

        if(w >= PlayerWeapon.WeaponCollection.Length)
        {
            w = 0;
        }

        PlayerWeapon.WeaponCollection[w].SetActive(true);
        PlayerWeapon.currentWeaponId = w;
    }
    #region LIGHT ANIMATION
    public void TurnLight(bool isActive)
    {
        if (isActive == true)
        {
            StartCoroutine(FadeLightToDefault());
        }
        else
        {
            StartCoroutine(FadeLightAnimation());
        }
    }

    private IEnumerator FadeLightAnimation()
    {
        LightFade.Play("LightFade");
        GroundBlack.Play("GroundBlack");

        yield return new WaitUntil(() => LightFade.IsPlaying("LightFade") == false && GroundBlack.IsPlaying("GroundBlack") == false);

        Torch.SetActive(true);
    }

    private IEnumerator FadeLightToDefault()
    {
        LightFade.Play("LightDefault");

        yield return new WaitUntil(() => LightFade.IsPlaying("LightDefault") == false);

        Torch.SetActive(false);
    }
    #endregion
    #region AI WAVES
    public void NextWave(int id)
    {
        Transform[] w = WavePosList[id].GetComponentsInChildren<Transform>();

        AiSpawnPositions.Clear();

        foreach (Transform item in w)
        {
            if (item != WavePosList[id])
            {
                AiSpawnPositions.Add(item);
            }
        }
    }

    public void ChangeWave()
    {
        StartCoroutine(WaveDelayStart());
    }

    private IEnumerator WaveDelayStart()
    {
        isDelayWave = true;

        yield return new WaitForSecondsRealtime(waveTime);

        isDelayWave = false;
        currentWave++;
        NextWave(currentWave);
    }

    #endregion

    #region GAME OVER
    private IEnumerator DelayGameOver()
    {
        yield return new WaitForSecondsRealtime(delayToOver);

        //UIManager.GameOverScreen.SetActive(true);
    }

    public void GameOver()
    {
        StartCoroutine(DelayGameOver());
    }
    #endregion
    public void OnNextButtonClick()
    {
        CurrentLevel += 1;
        AiCount = 0;
        AI_countThisLevel = 0;
        deaths = 0;

        NextLevelMenu.SetActive(false);
    }
    void OnLevelComplete()
    {

        isNextLevelMenu = false;
        
        int thisLevel = CurrentLevel + 1;
        PlayerPrefs.SetInt("Level"+thisLevel.ToString(), 1);
        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
        NextLevelMenu.SetActive(true);
        StatisticalData();
    }
    void onResetLevel()
    {
        // You died
        // reset inspector values
        deaths += 1;
        PlayerPrefs.SetInt("TotalDeaths", deaths);
        isDead = true;
        DeathScreen.SetActive(true);
        AiCount = 0;
        AI_countThisLevel = 0;
        AiTimer = 0f;

    }
    public void onResumeGame()
    {
        isDead = false;
        DeathScreen.SetActive(false);

    }

    public void OnPauseButtonClick()
    {
        if (!isPaused)
        {
            //PauseText.SetActive(true);
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            //PauseText.SetActive(false);
            PauseMenu.SetActive(false);

            Time.timeScale = 1;
            isPaused = false;
        }

    }

    public void ExitHome()
    {
        PlayerPrefs.SetInt("ExitHome",1);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

    }

    void GamePlayRequirements()
    {

        //ClickSoundSource.Play();
        TimerOn = true;
        for (int i = 0; i < MainPlayerMaterial.Length; i++)
        {
            if(PlayerPrefs.GetInt("Tank"+i) == 2)
            {
                MainPlayerSkin.material = MainPlayerMaterial[i];

            }
        }
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel");
        Debug.Log("CurrentLevel"+ PlayerPrefs.GetInt("CurrentLevel"));

    }
}
