using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Interactions : MonoBehaviour {

	public GameObject startText;
	public Animation blurImgAnim;

	void Start ()
	{
		
	}
	
	private void Update()
	{
		if(Input.GetKeyUp(KeyCode.E))
		{
			GameStart();
		}
	}

	public void GameStart()
	{
		startText.SetActive(false);
		blurImgAnim.Play("BlurAway");
	}
}
