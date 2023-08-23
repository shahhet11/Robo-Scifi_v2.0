using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public GameObject blastPrefab;

    public float HealthPoints;
    public Image healthSlider;
    public Text healthText;

    [Header("Damage")]
    public int dmg_bullet;
    public static PlayerHealth ph_instance;
    public bool shieldON;
    // Start is called before the first frame update
    void Start()
    {
        ph_instance = this;
       // HealthPoints = GameManager.Instance.maxHealthPlayer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {

            Destroy(collision.gameObject);

            if (!shieldON)
            {

                int id = collision.gameObject.GetComponent<Damage>().dmgId;
                dmg_bullet = GameManager.Instance.DamageList[id];
                UpdateHealth(dmg_bullet, collision.transform.position);
            }
        }
    }

    public void UpdateHealth(int Damage, Vector3 pos)
    {
        float calculate = (Damage * HealthPoints) / 100;
        HealthPoints -= calculate;
        GameManager.Instance.CameraShaker.Shake();
        GameManager.Instance.PlayerHealth.SetHealthSlider(HealthPoints);

        if (HealthPoints <= 1)
        {
            GameManager.Instance.PlayerDeath();
            GameManager.Instance.CameraShaker.Intensity = 1.5f;
            GameManager.Instance.CameraShaker.Shake();

            GameObject blast = Instantiate(blastPrefab, pos, Quaternion.identity);
            Destroy(blast, 1f);

            gameObject.SetActive(false);
        }
    }

    public void SetHealthSlider(float hp)
    {
        int maxHealth = GameManager.Instance.maxHealthPlayer;
        Debug.Log("");
        float total = ((float)(hp * 1f) / 100);
        if (total < 0f)
            total = 0f;

        healthSlider.fillAmount = total;
        
        healthText.text = HealthPoints.ToString("00");
    }
}
