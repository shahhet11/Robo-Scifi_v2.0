using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleFrameColorRandomize : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeInterval = 1.0f;
    public float speed = 2.0f;
    Color32 currentcolor;
    float duration = 1.0f;
    void Start()
    {
        StartCoroutine(ColorChangeRoutine());
        //InvokeRepeating("ChangeColor", 0.0f, duration);
      
    }

    private IEnumerator ColorChangeRoutine()
    {
        float timer = 0.0f;
        Color32 color = new Color32(255, (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);

        while (timer < timeInterval)
        {
            timer += Time.fixedDeltaTime * speed;

            if (this.gameObject.GetComponent<RawImage>() != null)
            {
                Debug.Log("WENTIN");
                GetComponent<RawImage>().color = Color32.Lerp(GetComponent<RawImage>().color, color, timer);

            }
            else if (this.gameObject.GetComponent<Text>() != null)
            {
                GetComponent<Text>().color = Color32.Lerp(GetComponent<Text>().color, color, timer);
            }
            

            yield return null;
        }

        // On finish recursively call the same routine
        StartCoroutine(ColorChangeRoutine());
    }




 
 void Update()
    {
        //Color32 color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
        //GetComponent<RawImage>().color = Color.Lerp(GetComponent<RawImage>().color, color, Random.value);
    }

}
