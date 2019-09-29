using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
	public float panSpeed, zoomSpeed, rotateSpeed;
	private Camera mainCamera;
	private Transform rotateReference;
	private Vector3 panReferencePoint;
	private PhysicsRaycaster physicsRaycaster;
	private List<RaycastResult> raycastResults = new List<RaycastResult>();
	private bool rotating, panning;
	private float distance;

	private void Awake()
	{
		mainCamera = Camera.main;
		physicsRaycaster = GetComponent<PhysicsRaycaster>();
		CodeControl.Message.AddListener<CameraZoomChangeRequestEvent>(OnZoomChangeRequested);
		CodeControl.Message.AddListener<CameraPanStartRequestEvent>(OnCameraPanStartRequested);
		CodeControl.Message.AddListener<CameraPanEndRequestEvent>(OnCameraPanEndRequested);
		CodeControl.Message.AddListener<CameraRotateStartRequestEvent>(OnCameraRotateStartRequested);
		CodeControl.Message.AddListener<CameraRotateEndRequestEvent>(OnCameraRotateEnded);
	}

	private void OnCameraRotateStartRequested(CameraRotateStartRequestEvent obj)
	{
		StartRotation();
	}

	private void StartRotation()
	{
		raycastResults.Clear();
		rotateReference = FindReference();
		if (rotateReference == null) return;
		distance = Vector3.Distance(mainCamera.transform.position, rotateReference.position);
		rotating = true;
	}

	private void OnCameraRotateEnded(CameraRotateEndRequestEvent obj)
	{
		rotating = false;
	}

	private void OnCameraPanEndRequested(CameraPanEndRequestEvent obj)
	{
		panning = false;
	}

	private void OnCameraPanStartRequested(CameraPanStartRequestEvent obj)
	{
		panning = true;
		panReferencePoint = Input.mousePosition;
	}

	private void OnZoomChangeRequested(CameraZoomChangeRequestEvent obj)
	{
		mainCamera.fieldOfView += obj.increase ? -zoomSpeed : +zoomSpeed;
	}

	private void Update()
	{
		if (rotating)
		{
			RotateCameraAroundPivot();
		}

		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		if (horizontalInput != 0 || verticalInput != 0)
		{
			PanCamera(horizontalInput, verticalInput);
		}

		if (panning)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - panReferencePoint);
			Vector3 move = new Vector3(-pos.x * panSpeed * Time.deltaTime, -pos.y * panSpeed * Time.deltaTime, 0);
			transform.Translate(move, Space.Self);
		}
	}

	private void RotateCameraAroundPivot()
	{
		if(rotateReference != null)
		{
			transform.RotateAround(rotateReference.position, Vector3.up, ((Input.GetAxisRaw("Mouse X") * Time.deltaTime) * rotateSpeed));
			transform.RotateAround(rotateReference.position, transform.right, -((Input.GetAxisRaw("Mouse Y") * Time.deltaTime) * rotateSpeed));
		}
		else
		{			
			transform.RotateAround(transform.forward, Vector3.up, ((Input.GetAxisRaw("Mouse X") * Time.deltaTime) * rotateSpeed));
			transform.RotateAround(transform.forward, transform.right, -((Input.GetAxisRaw("Mouse Y") * Time.deltaTime) * rotateSpeed));
		}
	}

	public void PanCamera(float x, float y)
	{
		Vector3 pos = mainCamera.transform.position;
		mainCamera.transform.Translate(x * panSpeed * Time.deltaTime, y * panSpeed * Time.deltaTime, 0, Space.Self);
	}

	private Transform FindReference()
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = Input.mousePosition;
		physicsRaycaster.Raycast(pointerEventData, raycastResults);
		if (raycastResults.Count > 0)
		{
			return raycastResults[0].gameObject.transform;
		}
		else
		{
			return null;
		}
	}

	private void DispatchZoomChangedEvent()
	{
		CodeControl.Message.Send(new CameraZoomChangedEvent(mainCamera.fieldOfView));
	}
}
