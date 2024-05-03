using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Runtime")]
	[SerializeField] Transform lookAtTarget;
	[SerializeField] Transform lockOnTargetUI;

	[Header("Settings")]
	[SerializeField] float moveSpeed = 5f;
	[SerializeField] float smoothInputSpeed = 0.2f;
	[SerializeField] float animBlendSpeed = 5f;
	[SerializeField] float rotationSpeed = 50f;
	[SerializeField] CameraState camState;
	[SerializeField] LayerMask hitLayers;
	[SerializeField] Transform hitBoxOrigin;

	[Header("Visuals")]
	[SerializeField] Animator animator;

	Rigidbody rb;
	bool lockedOn = false;
	[SerializeField] bool inAttack = false;

	Vector3 smoothDir = Vector3.zero;
	Vector3 moveVec = Vector3.zero;
	Vector3 angularVelVec = Vector3.zero;
	Vector3 smoothMoveDir = Vector3.zero;
	Vector3 smoothInputVelocity = Vector3.zero;
	Vector3 inputSmoothed = Vector3.zero;
	Vector3 inputSmoothedVelocity = Vector3.zero;

	float attackTime = 0f;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl) && lookAtTarget)
			lockedOn = !lockedOn;

		if (lockedOn)
			ApplyLookAtRotation();

		inAttack = attackTime >= 0f;
		attackTime -= Time.deltaTime;
	}
	
	void FixedUpdate()
	{
		ApplyMovement();
		HandleAnimations();
	}

	void LateUpdate()
	{
		if (lookAtTarget && lockedOn)
		{
			// lockOnTargetUI.position = Camera.main.WorldToScreenPoint(lookAtTarget.position);
			// lockOnTargetUI.gameObject.SetActive(true);			
		}

		else
		{
			// lockOnTargetUI.gameObject.SetActive(false);
			lockedOn = false;
		}
	}

	void ApplyMovement()
	{
		if (lockedOn && lookAtTarget)
			LockedOnMovement();
		else
			FreeRoamMovement();
	}

	void LockedOnMovement()
	{
		moveVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

		moveVec = transform.TransformDirection(moveVec);
		if (!inAttack) rb.MovePosition(transform.position + moveVec * moveSpeed * Time.deltaTime);

		camState.LockOn();
	}

	void FreeRoamMovement()
	{
		moveVec = new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")).normalized;
		moveVec = transform.TransformDirection(moveVec);
		smoothMoveDir = Vector3.SmoothDamp(smoothMoveDir, moveVec, ref smoothInputVelocity, smoothInputSpeed);
		if (!inAttack) rb.MovePosition(transform.position + moveVec * moveSpeed * Time.deltaTime);

		angularVelVec = new Vector3(0f, Input.GetAxisRaw("Horizontal"), 0f).normalized;
		if (!inAttack) rb.angularVelocity = angularVelVec * rotationSpeed;

		camState.FreeCam();
	}

	void ApplyLookAtRotation()
	{
		if (!lookAtTarget) return;
		transform.LookAt(new Vector3(lookAtTarget.position.x, transform.position.y, lookAtTarget.position.z));
	}

	void HandleAnimations()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
		if (!lockedOn && input.z != 0f)
			input.x = 0f;
		if (!lockedOn && Mathf.Abs(input.z - 0f) < 1e-2)
			input *= 0.8f;

		inputSmoothed = Vector3.Lerp(inputSmoothed, input, animBlendSpeed * Time.deltaTime);
		animator.SetFloat("DirectionX", inputSmoothed.x);
		animator.SetFloat("DirectionZ", inputSmoothed.z);
		animator.SetBool("LockedOn", lockedOn);
		animator.SetFloat("Speed", inputSmoothed.magnitude);
	}
}
