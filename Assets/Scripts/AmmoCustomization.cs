using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCustomization : MonoBehaviour
{
    
    public GameObject[] AmmoIconLayer0;
    public Text ammoMagsTxt;
    public Text ammoRoundsTxt;
    public GameObject[] Ammos;
    private Coroutine currentCoroutine;
    public float transitionDuration = 1.0f;
    public float fadeDuration = 1.0f;
    private Color initialColor;
    public Color targetColor;
    [System.Serializable]
    public class AmmoObject
    {
        public string WeaponName;
        public int WeaponID;
        public int ammoCurrent;
        public int ammoRounds;
        public int ammoMags;
        public bool isReloading;
    }

    public List<AmmoObject> weaponAmmoList = new List<AmmoObject>();
  

    private void Start()
    {
        initialColor = ammoRoundsTxt.color;
    }
    public void Reload()
    {
        // animation or something buffer time.
        int bulletsToReload = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoRounds - weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent;
        weaponAmmoList[GameManager.Instance.currentWeaponIndex].isReloading = true;

        // Check if there are enough bullets in totalAmmo to reload.
        if (bulletsToReload <= weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags)
        {
            StartCoroutine(LerpAmmo(weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent + bulletsToReload, weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags - bulletsToReload));
            weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent += bulletsToReload;
            weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags -= bulletsToReload;
        }
        else
        {
            StartCoroutine(LerpAmmo(weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent + weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags, 0));
            weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent += weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags;
            weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags = 0;
            
        }
        currentCoroutine = StartCoroutine(ActivateAmmosWithDelay(weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent));
        ammoRoundsTxt.text = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent.ToString();
        ammoMagsTxt.text = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags.ToString();
    }
    private IEnumerator LerpAmmo(int targetCurrentAmmo, int targetTotalAmmo)
    {
        float duration = 1.0f; // Duration of the lerp effect
        float startTime = Time.time;
        int startCurrentAmmo = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent;
        int startTotalAmmo = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent = Mathf.RoundToInt(Mathf.Lerp(startCurrentAmmo, targetCurrentAmmo, t));
            weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags = Mathf.RoundToInt(Mathf.Lerp(startTotalAmmo, targetTotalAmmo, t));

            ammoRoundsTxt.text = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent.ToString("00");
            ammoMagsTxt.text = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags.ToString("00");


            yield return null;
        }

        weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent = targetCurrentAmmo;
        weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags = targetTotalAmmo;


        ammoRoundsTxt.text = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent.ToString("00");
        ammoMagsTxt.text = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags.ToString("00");
        weaponAmmoList[GameManager.Instance.currentWeaponIndex].isReloading = false;
    }
    public IEnumerator ActivateAmmosWithDelay(int currentAmmo)
    {
        float activationDelay = 0.05f;
        for (int i = 0; i < weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoRounds; i++)
        {
            Ammos[i].SetActive(false);
        }
        for (int i = 0; i < currentAmmo; i++)
        {
            Ammos[i].SetActive(true);
            yield return new WaitForSeconds(activationDelay);
        }
    }
    public void InterpolateAmmoColor(int alpha)
    {
        //StartCoroutine(InterpolateColor(alpha));
        StartCoroutine(FadeInOut());
    }
    private IEnumerator FadeInOut()
    {
        float elapsedTime = 0f;
        Color targetColor = ammoRoundsTxt.color;
        targetColor.a = 0; // Set the target alpha to 0 for fade out.

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            ammoRoundsTxt.color = Color.Lerp(initialColor, targetColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is the target color.
        ammoRoundsTxt.color = targetColor;

        // Pause briefly before fading back in.
        yield return new WaitForSeconds(0.2f);

        // Swap the target color for fade in.
        targetColor = initialColor;
        elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            ammoRoundsTxt.color = Color.Lerp(targetColor, initialColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is the initial color.
        ammoRoundsTxt.color = initialColor;
    }
    private IEnumerator InterpolateColor(int alpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            Color lerpedColor = Color.Lerp(initialColor, targetColor, t);
            lerpedColor.a =  ammoRoundsTxt.color.a; // Keep the alpha value unchanged
            ammoRoundsTxt.color = lerpedColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is the target color
        ammoRoundsTxt.color = targetColor;
    }
    public void ShootAmmo()
    {
        if (weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent > 0)
        {
            weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent--;
            ammoRoundsTxt.text = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent.ToString("00");
            ammoMagsTxt.text = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags.ToString("00");
            Ammos[weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent].SetActive(false);
            for (int i = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent; i < 20; i++)
            {
                if (Ammos[i].activeInHierarchy)
                {
                    Ammos[i].SetActive(false);
                }
            }
        }
    }
    public void AmmoUpdate()
    {
        if (weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent > 0)
        {
            ShootAmmo();
        }
         if (weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent == 0 && weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags > 0)
        {
            Reload();
        }
        else
        {
            // Handle no ammo situation
        }
    }

    public void DisplayAmmo()
    {
        if (weaponAmmoList[GameManager.Instance.currentWeaponIndex].isReloading)
            weaponAmmoList[GameManager.Instance.currentWeaponIndex].isReloading = false;
        ammoRoundsTxt.text = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent.ToString();
        ammoMagsTxt.text = weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoMags.ToString();

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(ActivateAmmosWithDelay(weaponAmmoList[GameManager.Instance.currentWeaponIndex].ammoCurrent));
    }

    public void onAmmoHitCamShake()
    {
        GameManager.Instance.CameraShaker.Shake();
    }

}
