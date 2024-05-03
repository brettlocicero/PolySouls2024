using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] Animator anim;
	[SerializeField] float speed = 5f;
	
	CharacterController cc;
	bool isLockedOn = false;
	
	void Start() 
	{
		cc = GetComponent<CharacterController>();
	}
	
	void Update() 
	{
		MovementHandler();
		LockOnHandler();
	}
	
	void MovementHandler() 
	{
		Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		anim.SetFloat("DirectionX", input.x);
		anim.SetFloat("DirectionZ", input.z);
		
		cc.Move(input.normalized * Time.deltaTime * speed);
	}
	
	void LockOnHandler() 
	{
		if (Input.GetKeyDown(KeyCode.LeftControl)) 
			isLockedOn = !isLockedOn;
	}
}
