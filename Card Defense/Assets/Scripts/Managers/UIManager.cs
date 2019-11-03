using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public ViewCollection views;
	private View currentView;

	private void Awake()
	{
		CodeControl.Message.AddListener<ChangeViewRequestEvent>(OnChangeViewRequested);
	}

	private void Start()
	{
		AppStartSetup();
	}

	private void AppStartSetup()
	{
		foreach (GameObject view in views.Collection)
		{
			if (view == null) continue;
			view.SetActive(true);
			view.SetActive(false);
		}
		views[View.MainMenu].SetActive(true);
		currentView = View.MainMenu;
	}

	private void OnChangeViewRequested(ChangeViewRequestEvent obj)
	{
		ChangeView(obj.previousView, obj.requestedView, obj.togglePreviousOff);
	}

	private void ChangeView(View previousView, View requestedView, bool togglePreviousOff)
	{
		views[previousView].SetActive(!togglePreviousOff);
		View viewToLeaveUp = requestedView == View.Back ? currentView : requestedView;
		views[viewToLeaveUp].SetActive(true);
		if (togglePreviousOff) currentView = viewToLeaveUp;
	}
}
