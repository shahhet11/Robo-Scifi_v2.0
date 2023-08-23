using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {


	public Transform Target;
	private Vector3 offset;
	private Vector3 storePos;


	void Start ()
	{
		
	}
	
	void LateUpdate ()
	{
		if(Target == null)
		{
			Target = GameObject.FindGameObjectWithTag("Player").transform;

			if(Target)
			{
				offset = transform.position - Target.position;
			}
		}
		else
		{
			transform.position = offset + Target.position;
		}
	}
}
