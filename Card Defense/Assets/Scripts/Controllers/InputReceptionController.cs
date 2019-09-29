using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputReceptionController : MonoBehaviour
{
	private void Update()
	{
		if (Input.mouseScrollDelta.y > 0)
		{
			DispatchCameraZoomRequestEvent(true);
		}
		else if (Input.mouseScrollDelta.y < 0)
		{
			DispatchCameraZoomRequestEvent(false);
		}
		if (Input.GetMouseButtonUp(0))
		{
			CheckForTowerTargeting();			
		}
		if (Input.GetMouseButtonDown(2))
		{
			DispatchCameraPanStartRequestEvent();
		}
		if (Input.GetMouseButtonUp(2))
		{
			DispatchCameraPanEndRequestEvent();
		}
		if (Input.GetMouseButtonDown(1))
		{
			DispatchCameraRotateStartRequestEvent();
		}
		if (Input.GetMouseButtonUp(1))
		{
			DispatchCameraRotateEndRequestEvent();
		}
	}

	private void CheckForTowerTargeting()
	{
		RaycastHit raycastHit;
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit);
		if (raycastHit.collider == null ||
			(raycastHit.collider.GetComponent<TowerController>() == null &&
			raycastHit.collider.GetComponent<TowerTargetController>() == null))
		{
			DispatchStopDisplayingTargetRequestEvent();
		}
	}

	private void DispatchStopDisplayingTargetRequestEvent()
	{
		CodeControl.Message.Send(new StopShowingTargetsRequestEvent());
	}

	private void DispatchCameraZoomRequestEvent(bool increase)
	{
		CodeControl.Message.Send(new CameraZoomChangeRequestEvent(increase));
	}

	private void DispatchCameraRotateStartRequestEvent()
	{
		CodeControl.Message.Send(new CameraRotateStartRequestEvent());
	}

	private void DispatchCameraRotateEndRequestEvent()
	{
		CodeControl.Message.Send(new CameraRotateEndRequestEvent());
	}

	private void DispatchCameraPanStartRequestEvent()
	{
		CodeControl.Message.Send(new CameraPanStartRequestEvent());
	}

	private void DispatchCameraPanEndRequestEvent()
	{
		CodeControl.Message.Send(new CameraPanEndRequestEvent());
	}
}
