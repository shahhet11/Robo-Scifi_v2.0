using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Image healthSlider;
    public Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetHealthSlider(float hp)
    {
        int maxHealth = GameManager.Instance.maxHealthPlayer;
        float total = ((float)(hp * 1f) / 100);
        if (total < 0f)
            total = 0f;

        healthSlider.fillAmount = total;
        healthText.text = total.ToString("00");
    }
}