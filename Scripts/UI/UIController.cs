using System;
using System.Collections.Generic;
using UnityEngine;

namespace CCore.UI
{
	public class UIController : MonoBehaviourSingleton<UIController>
	{
		private List<UIView> uiViews;

		private bool isSwappingViews = false;

		private bool isSwappingPopups = false;

		private UIView currentView;

		private UIView nextView;

		private UIView currentPopup;

		private UIView nextPopup;

		private void Awake()
		{
			uiViews = new List<UIView>(GetComponentsInChildren<UIView>(true));

			InitializeAllViews();
		}

		private void InitializeAllViews()
		{
			for (int i = uiViews.Count - 1; i >= 0; i--)
			{
				UIView uiView = uiViews[i];

				if (uiView.Layer == UILayer.Child)
				{
					continue;
				}

				uiView.RequestShowViewEvent += OnRequestShowView;

				uiView.RequestHideViewEvent += OnRequestHideView;

				uiView.ShowCompleteEvent += OnShowComplete;

				uiView.HideCompleteEvent += OnHideComplete;
				
				uiView.Initialize();
			}
		}

        private void OnRequestShowView(UIView uiView)
        {
			switch (uiView.Layer)
			{
				case UILayer.Default:
            		SwapView(uiView);
					break;
				
				case UILayer.Popup:
					SwapPopup(uiView);
					break;

				case UILayer.Overlay:
					ShowView(uiView);
					break;
			}
        }

        private void OnRequestHideView(UIView uiView)
        {
            HideView(uiView);
        }

		private void OnShowComplete(UIView uiView)
		{
			currentView = uiView;
		}

		private void OnHideComplete(UIView uiView)
		{
			if (isSwappingViews)
			{
				ShowView(nextView);

				// The current ui view will be set to proper
				// value on show complete
				currentView = null;

				nextView = null;

				isSwappingViews = false;
			}

			if (isSwappingPopups)
			{
				ShowView(nextPopup);

				// The current popup will be set to proper
				// value on show complete
				currentPopup = null;

				nextPopup = null;

				isSwappingPopups = false;
			}
		}
		
        private void ShowView(UIView uiView)
		{
			uiView.OnShow();
		}

		private void HideView(UIView uiView)
		{
			uiView.OnHide();
		}

		private void SwapView(UIView uiView)
		{
			if (currentView == null || currentView == uiView)
			{
				ShowView(uiView);

				return;
			}

			nextView = uiView;

			isSwappingViews = true;

			HideView(currentView);
		}

		private void SwapPopup(UIView uiView)
		{
			if (currentPopup == null || currentPopup == uiView)
			{
				ShowView(uiView);

				return;
			}

			nextPopup = uiView;

			isSwappingPopups = true;

			HideView(currentPopup);
		}

		public T GetView<T>() where T : UIView
		{
			for (int i = 0; i < uiViews.Count; i++)
			{
				Type type = uiViews[i].GetType();

				if (type == typeof(T))
				{
					return uiViews[i] as T;
				}
			}

			Debug.LogError("Unable to find UI View of type: " + typeof(T).Name);

			return null;
		}
	}
}