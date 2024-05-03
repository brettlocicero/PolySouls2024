using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraState : MonoBehaviour
{
	[SerializeField] GameObject lockOnCam;
	[SerializeField] GameObject freeCam;
	
	public void LockOn () 
	{
		lockOnCam.SetActive(true);
		freeCam.SetActive(false);
	}
	
	public void FreeCam () 
	{
		lockOnCam.SetActive(false);
		freeCam.SetActive(true);
	}
}
