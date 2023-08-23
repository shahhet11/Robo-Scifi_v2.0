using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    // Things to Do
    // Hint Text
    // Loading Transition Animation
    // Loading Content Animation
    AsyncOperation async;

    public Canvas LoadingScreenUI;
    public Text loadingText;
    public Image sliderBar;
    float progress = 0.01f;
    public Text ChangeHints;
    public List<string> Hints = new List<string>();
    public AudioClip Loadingfx;
    public AudioSource LoadingSource;
    public Animation TextFade;
    public Animation BarFade;
    string SceneName;
    public static LoadingScene instance = null;
    private static readonly object padlock = new object();

   
    void Start()
    {
        instance = this;
        //Scene scene = new Scene();
        //Scene scene = SceneManager.GetActiveScene();
        //SceneName = scene.ToString();
        Hints.Add("Hint : Getting Killed Easily? No Problem...Buy new Weapons from Armory");
        Hints.Add("Hint : You can always heal by collecting Medipacks");
        Hints.Add("Hint : Press LShift to access Jetpack");
        Hints.Add("Hint : Press LCtrl for Slow-Motion");
        Hints.Add("Hint : Press Right click to access Phys-Gun to throw barrels");
        //Hide Slider Progress Bar in start


    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
    void PlayLoadinSound()
    {
        LoadingSource.clip = Loadingfx;
        LoadingSource.Play();
    }
    public void LoadGame()
    {
        LoadingScreenUI.enabled = true;
        PlayLoadinSound();
        NewHints();
       
            StartCoroutine(LoadNewScene());
        //SceneManager.LoadScene("gameScene");
    }
    public void UnloadGame()
    {
        GameObject.Find("buttonClick").GetComponent<AudioSource>().Play();
        
        Time.timeScale = 1;
        LoadingScreenUI.enabled = true;
        PlayLoadinSound();
        NewHints();
        //SceneManager.LoadScene("UIScene");
        StartCoroutine("LoadOldScene");

    }
    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {
        
        
         async = SceneManager.LoadSceneAsync("GamePlay-Scene");
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            
            //float progress = Mathf.Clamp01(async.progress / 0.09f);
            sliderBar.fillAmount = progress;
            loadingText.text = Mathf.Round(progress * 100f)+ "%";
            
            progress = progress + 0.001f;// 0.001f takes Time But It Displays 3 Texts  && 0.009f Loads Faster But WOnt Display Texts
            
            Transitions();
            if (Mathf.Round(progress * 100f) == 100f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;

        }

    }
    IEnumerator LoadOldScene()
    {
        if (GameObject.Find("player"))
        {
        GameObject.Find("player").SetActive(false);
        }
        async = SceneManager.LoadSceneAsync("UIScene");
        async.allowSceneActivation = false;

        while (!async.isDone)
        {

            //float progress = Mathf.Clamp01(async.progress / 0.09f);
            sliderBar.fillAmount = progress;
            loadingText.text = Mathf.Round(progress * 100f) + "%";

            progress = progress + 0.0045f;// 0.001f takes Time But It Displays 3 Texts  && 0.009f Loads Faster But WOnt Display Texts

            Transitions();
            if (Mathf.Round(progress * 100f) == 100f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;

        }

    }
    public void Transitions()
    {
        if (Mathf.Round(progress * 100f) == 35f)
        {
            loadingText.color = Color.yellow;
            sliderBar.color = Color.yellow;
            //NewHints();
        }
        if (Mathf.Round(progress * 100f) == 75f)
        {
            loadingText.color = Color.green;
            sliderBar.color = Color.green;
            //NewHints();
        }
    }


    public void NewHints()
    {
        
            
        TextFade.Play();
        BarFade.Play();
        Invoke("ChangeHintText",1f);
        InvokeRepeating("ChangeHintText",1.1f,4f);
        
    }
    void ChangeHintText()
    {
        int j = Random.Range(0, 5);
        ChangeHints.text = Hints[j];
    }
}

