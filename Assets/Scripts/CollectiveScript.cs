using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiveScript : MonoBehaviour
{
    public float maxDestroyTime;

    void Start()
    {
        maxDestroyTime = GameManager.Instance.maxBoxDestroyTime;

        StartCoroutine(DestroyAfterTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(transform.CompareTag("UpgradeBox"))
            {
                UpgradeBox();
            }
            else if (transform.CompareTag("HealthBox"))
            {
                HealthBox();
            }

            Destroy(gameObject);
        }
    }

    #region
    private void UpgradeBox()
    {
        //GameManager.Instance.UpgradeWeapon();
    }

    private void HealthBox()
    {
        GameManager.Instance.PlayerHealth.HealthPoints = GameManager.Instance.maxHealthPlayer;
        GameManager.Instance.PlayerHealth.SetHealthSlider(GameManager.Instance.PlayerHealth.HealthPoints);
    }
    #endregion

    private IEnumerator DestroyAfterTime()
    {
        float t = 0f;

        while(t <= maxDestroyTime)
        {
            t += Time.unscaledDeltaTime;

            yield return 0;
        }

        Destroy(gameObject);
    }
}
