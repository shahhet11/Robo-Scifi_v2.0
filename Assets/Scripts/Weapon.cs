using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
	public int currentWeaponId;
    public GameObject[] AmmoCollection;
    public GameObject[] WeaponCollection;
    public GameObject[] SecondaryWeaponCollection;
    public GameManager GameManager;
	void Start ()
	{
		//currentWeaponId = 0;
		currentWeaponId = GameManager.currentWeaponIndex;//PlayerPrefs.GetInt("selectedWeapon");
        WeaponCollection[currentWeaponId].SetActive(true);
        AmmoCollection[currentWeaponId].SetActive(true);
        SecondaryWeaponCollection[currentWeaponId].SetActive(true);
    }
}
