using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponLoadout : MonoBehaviour
{
    public ScrollSnapRect scrollSnap;
    public HomeScreen HomeScreen;
    public TankManager TankManager;

    public int[] priceArr;
    public int currentEqId;

    [Header("Buttons")]
    public GameObject buyBtn;
    public GameObject equipBtn;
    public Text eqText;


    //Summary Shotgun, SwarmMissile, MultipleShotgun, MultipleSwarmMissile, DancingLines
    void Start()
    {
        
    }

    public void LockAll(int pageCount)
    {
        for (int i = 0; i < pageCount; i++)
        {
            PlayerPrefs.SetInt("Weapon"+i, 0);
        }
    }

    public void CheckWeapon(int id)
    {
        //0 = not purchased
        //1 = purchased
        //2 = equipped

        int status = PlayerPrefs.GetInt("Weapon"+id);

        if(status == 0)
        {
            ManageButtons(true, false);
            eqText.text = "GET";
        }
        else if(status == 1)
        {
            ManageButtons(false, true);
            equipBtn.GetComponent<Button>().interactable = true;
            eqText.text = "GET";
        }
        else if(status == 2)
        {
            ManageButtons(false, true);
            equipBtn.GetComponent<Button>().interactable = false;
            eqText.text = "EQUIPPED";
        }
    }

    private void ManageButtons(bool buy, bool equip)
    {
        buyBtn.SetActive(buy);
        equipBtn.SetActive(equip);
    }

    public void Buy()
    {
        int totalMoney = PlayerPrefs.GetInt("TotalGold");
        
        if((totalMoney - priceArr[scrollSnap._currentPage]) >= 0)
        {
            totalMoney -= priceArr[scrollSnap._currentPage];
            PlayerPrefs.SetInt("TotalGold", totalMoney);

            HomeScreen.CheckWalletOnStart();
            Equip();
        }
        else
        {
            Debug.Log("more money required");
        }

    }

    public void Equip()
    {
        UnEquipLast();

        PlayerPrefs.SetInt("Weapon"+scrollSnap._currentPage, 2);
        PlayerPrefs.SetInt("selectedWeapon", scrollSnap._currentPage);

        currentEqId = scrollSnap._currentPage;

        CheckWeapon(scrollSnap._currentPage);

        //TankManager.ActiveWeaponById();
    }

    public void UnEquipLast()
    {
        for (int i = 0; i < scrollSnap._pageCount; i++)
        {
            if(PlayerPrefs.GetInt("Weapon"+i) == 2)
                PlayerPrefs.SetInt("Weapon"+i, 1);
        }
    }
}
