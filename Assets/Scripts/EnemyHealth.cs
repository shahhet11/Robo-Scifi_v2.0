using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject blastPrefab;
    public GameObject healthBox;
    public GameObject weaponBox;

    public int HealthPoints;

    [Header("Damage")]
    public int dmg_bullet_percent;

    void Start()
    {
        HealthPoints = GameManager.Instance.maxHealthAI;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.name.Contains("PoliceCar"))
        {
            if (collision.collider.name.Contains("Enemy"))
            {
                Destroy(collision.collider.gameObject);
            }
        }

        if (collision.collider.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);

            int id = collision.gameObject.GetComponent<Damage>().dmgId;
            dmg_bullet_percent = GameManager.Instance.DamageList[id];

            UpdateHealth(dmg_bullet_percent, collision.transform.position);
        }
    }

    public void UpdateHealth(int Damage, Vector3 pos)
    {
        HealthPoints -= Damage;
        GameManager.Instance.CameraShaker.Shake();
        if (HealthPoints <= 0)
        {
            GameManager.Instance.killCounter++;
            GameManager.Instance.minWaveKills++;

            //For upgrade box
            if (GameManager.Instance.killCounter >= GameManager.Instance.minUpgradeKills)
            {
                GameManager.Instance.minUpgradeKills += 10;
                Instantiate(weaponBox, pos, weaponBox.transform.rotation);
            }

            //For Health box
            if (GameManager.Instance.killCounter >= GameManager.Instance.minHealthKills)
            {
                GameManager.Instance.minHealthKills += 10;
                Instantiate(healthBox, pos, healthBox.transform.rotation);
            }

            if (GameManager.Instance.minWaveKills == GameManager.Instance.WaveKills[GameManager.Instance.currentWave])
            {
                GameManager.Instance.minWaveKills = 0;
                GameManager.Instance.AiCount = 0;

                //GameManager.Instance.ChangeWave();
            }

            //For Light blackout
            if (GameManager.Instance.killCounter > GameManager.Instance.minBlackoutkills) //This will only run once
            {
                GameManager.Instance.TurnLight(false);
                GameManager.Instance.minBlackoutkills = int.MaxValue;
            }

            //GameManager.Instance.AiCount--;
            GameManager.Instance.CameraShaker.Shake();
            GameObject blast = Instantiate(blastPrefab, pos, Quaternion.identity);
            Destroy(blast, 1f);
            //GameManager.Instance.CameraShaker.Shake();
           Destroy(gameObject);
        }
    }
}
