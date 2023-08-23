using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TankManager : MonoBehaviour
{
    public ScrollSnapRect scrollSnap;
    public HomeScreen HomeScreen;
    public WeaponLoadout WeaponLoadout;

    public Transform[] TankWeaponParents;

    public int[] priceArr;
    
    [Header("Buttons")]
    public GameObject buyBtn;
    public GameObject equipBtn;
    public Text eqText;

    //Summary Default, Red, Yellow, Grey, Black
    void Start()
    {
        
    }

    public void LockAll(int pageCount)
    {
        for (int i = 0; i < pageCount; i++)
        {
            PlayerPrefs.SetInt("Tank"+i, 0);
        }
    }

    public void CheckTank(int id)
    {
        //0 = not purchased
        //1 = purchased
        //2 = equipped

        int status = PlayerPrefs.GetInt("Tank"+id);

        if(status == 0)
        {
            ManageButtons(true, false);
            eqText.text = "EQUIP";
        }
        else if(status == 1)
        {
            ManageButtons(false, true);
            equipBtn.GetComponent<Button>().interactable = true;
            eqText.text = "EQUIP";
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
        }
        else
        {
            Debug.Log("more money required");
        }

        Equip();
    }

    public void Equip()
    {
        UnEquipLast();

        PlayerPrefs.SetInt("Tank"+scrollSnap._currentPage, 2);

        CheckTank(scrollSnap._currentPage);
    }

    public void UnEquipLast()
    {
        for (int i = 0; i < scrollSnap._pageCount; i++)
        {
            if(PlayerPrefs.GetInt("Tank"+i) == 2)
                PlayerPrefs.SetInt("Tank"+i, 1);
        }
    }


}
