using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HomeScreen : MonoBehaviour
{
    public Text[] Gold;
    public int Goldint = 10000;
    //public Text[] Money;
    int Goldno;
    int Moneyno;
    public GameObject Title;
    public GameObject Dissolve;
    public LoadingScene loadingScene;
    public void RedirectToMain()
    {
        SceneManager.LoadScene(1);
    }

     void Start()
    {
        PlayerPrefs.SetInt("TotalGold", Goldint);
        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.GetInt("SetWalletFirstTime") == 0)
        {
            PlayerPrefs.SetInt("TotalGold", Goldint); 
            PlayerPrefs.DeleteAll();
            Debug.Log("WentIn");
            SetWalletFirstTime();
            PlayerPrefs.SetInt("SetWalletFirstTime", 123124);
        }
        CheckWalletOnStart();
        CheckGameReturn();
    }

    public void TapStart()
    {
        
        Dissolve.SetActive(true);
        Invoke("CloseDissolve", 2f);
        Invoke("CloseTitle", 0.5f);
        //Invoke("LoadGameScene", 2f);
        
    }
    void LoadGameScene()
    {
        loadingScene.LoadGame();
    }
    void CloseDissolve()
    {
        Dissolve.SetActive(false);
    }

    void CloseTitle()
    {

        Title.SetActive(false);
    }

    void SetWalletFirstTime()
    {
        Debug.Log("WentIn11");
        // Defining initial coins value provided to user 
       
    }
    public void CheckWalletOnStart()
    {
        //PlayerPrefs.SetInt("TotalGold", 10000);
        Goldno = PlayerPrefs.GetInt("TotalGold");
        // storing the value of coins in a variable using Player Prefs

        for (int i = 0; i < Gold.Length; i++)
        {
            Gold[i].text = Goldno.ToString();
            
        }

    }
    void CheckGameReturn()
    {
        if(PlayerPrefs.GetInt("ExitHome") == 1)
        {
            PlayerPrefs.SetInt("ExitHome", 0);
            Title.SetActive(false);
        }
        
    }
}
