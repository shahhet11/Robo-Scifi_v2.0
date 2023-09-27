using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float _initialHealth;
     private float _currentHealth;
    // Start is called before the first frame update
    void Awake()
    {
        _currentHealth = _initialHealth;
    }

    public void ApplyDamage(float Damage)
    {
        if (_currentHealth <= 0) return;

        _currentHealth -= Damage;

        if (_currentHealth <= 0)
        {
            GameManager.Instance.CameraShaker.Shake();
            Destroy(gameObject);
        }
    }
}
