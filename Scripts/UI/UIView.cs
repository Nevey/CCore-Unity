using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace CCore.UI
{
	public abstract class UIView : MonoBehaviour
	{
		[SerializeField] private UILayer layer;

		[SerializeField] private bool isAnimated = true;

		[Header("Show animation properties")]
		[SerializeField] private AnimationDirection showAnimationDirection;

		[SerializeField] private Ease showAnimationEase = Ease.OutBack;

		[SerializeField] private float showAnimationTime = 1f;

		[Header("Hide animation properties")]
		[SerializeField] private AnimationDirection hideAnimationDirection;

		[SerializeField] private Ease hideAnimationEase = Ease.InBack;

		[SerializeField] private float hideAnimationTime = 1f;

		private enum AnimationDirection
		{
			top,
			bottom,
			left,
			right,
		}

		private enum WindowState
		{
			Hidden,
			Showing
		}

		private WindowState windowState = WindowState.Hidden;

		private IOnUIViewInitialize[] onUIViewInitialize;

		private IOnUIViewShow[] onUIViewShow;

		private IOnUIViewHide[] onUIViewHide;

		private UIView[] childViews;

		private Rect uiRect;

		private RectTransform rectTransform;

		private Vector2 startPosition;

		private Vector2 showPosition;

		private Vector2 endPosition;

		private Tween moveTween;

		public UILayer Layer { get { return layer; } }

		protected event Action LocalShowCompletedEvent;

		protected event Action LocalHideCompletedEvent;

		public event Action<UIView> RequestShowViewEvent;

		public event Action<UIView> RequestHideViewEvent;

		public event Action<UIView> ShowCompleteEvent;

		public event Action<UIView> HideCompleteEvent;

		private void SetupAnimation()
		{
			// The start position used for the "show" animation
			startPosition = GetDirectionPosition(showAnimationDirection);

			// Middle of the screen
			showPosition = Vector2.zero;

			// The end position used for the "hide" animation
			endPosition = GetDirectionPosition(hideAnimationDirection);
		}

		private void SetupChildViews()
		{
			if (layer != UILayer.Child)
			{
				childViews = GetComponentsInChildren<UIView>(true);

				for (int i = 0; i < childViews.Length; i++)
				{
					if (childViews[i] == this)
					{
						continue;
					}

					childViews[i].Initialize();
				}
			}
		}

		private Vector2 GetDirectionPosition(AnimationDirection direction)
		{
			Vector2 position = Vector2.zero;

			switch (direction)
			{
				case AnimationDirection.top:
					
					position = new Vector2(
						0f,
						uiRect.height
					);

					break;
				
				case AnimationDirection.bottom:

					position = new Vector2(
						0f,
						-uiRect.height
					);

					break;
				
				case AnimationDirection.left:

					position = new Vector2(
						-uiRect.width,
						0f
					);

					break;
				
				case AnimationDirection.right:

					position = new Vector2(
						uiRect.width,
						0f
					);

					break;
			}

			return position;
		}

		/// <summary>
		/// Plays the show animation
		/// </summary>
		private void AnimateIn()
		{
			if (!isAnimated)
			{
				OnShowComplete();

				return;
			}

			StopTween();

			rectTransform.anchoredPosition = startPosition;

			moveTween = rectTransform.DOAnchorPos(showPosition, showAnimationTime);

			moveTween.SetEase(showAnimationEase);

			moveTween.OnComplete(OnShowComplete);

			moveTween.Play();
		}

		/// <summary>
		/// Plays the hide animation
		/// </summary>
		private void AnimateOut()
		{
			if (!isAnimated)
			{
				OnHideComplete();
				
				return;
			}

			StopTween();

			moveTween = rectTransform.DOAnchorPos(endPosition, hideAnimationTime);

			moveTween.SetEase(hideAnimationEase);

			moveTween.OnComplete(OnHideComplete);

			moveTween.Play();
		}

		private void StopTween()
		{
			if (moveTween != null
				&& moveTween.IsPlaying())
			{
				// moveTween.onComplete();

				moveTween.Kill();
			}
		}

		/// <summary>
		/// Use this method to do custom stuff internally
		/// </summary>
		protected virtual void OnShowComplete()
		{
			if (LocalShowCompletedEvent != null)
			{
				LocalShowCompletedEvent();
			}

			if (ShowCompleteEvent != null)
			{
				ShowCompleteEvent(this);
			}

			// Do custom stuff here...
		}

		/// <summary>
		/// Use this method to do custom stuff internally
		/// </summary>
		protected virtual void OnHideComplete()
		{
			gameObject.SetActive(false);

			if (onUIViewHide != null)
			{
				for (int i = 0; i < onUIViewHide.Length; i++)
				{
					onUIViewHide[i].OnUIViewHide();
				}
			}

			if (LocalHideCompletedEvent != null)
			{
				LocalHideCompletedEvent();
			}

			if (HideCompleteEvent != null)
			{
				HideCompleteEvent(this);
			}
			
			// Do custom stuff here...
		}

		protected void Show()
		{
			if (RequestShowViewEvent != null)
			{
				RequestShowViewEvent(this);
			}
		}

		protected void Hide()
		{
			if (RequestHideViewEvent != null)
			{
				RequestHideViewEvent(this);
			}
		}

		public virtual void Initialize()
		{
			// In case this view was enabled in the editor, disable...
			gameObject.SetActive(false);

			if (layer != UILayer.Child)
			{
				onUIViewInitialize = GetComponentsInChildren<IOnUIViewInitialize>(true);

				onUIViewShow = GetComponentsInChildren<IOnUIViewShow>(true);

				onUIViewHide = GetComponentsInChildren<IOnUIViewHide>(true);

				for (int i = 0; i < onUIViewInitialize.Length; i++)
				{
					onUIViewInitialize[i].OnUIViewInitialize();
				}
			}

			rectTransform = GetComponent<RectTransform>();

			uiRect = rectTransform.rect;

			SetupAnimation();

			SetupChildViews();

			name = this.GetType().Name;

			Setup();
		}

		public virtual void OnShow()
		{
			if (windowState == WindowState.Showing)
			{
				return;
			}

			gameObject.SetActive(true);

			if (onUIViewShow != null)
			{
				for (int i = 0; i < onUIViewShow.Length; i++)
				{
					onUIViewShow[i].OnUIViewShow();
				}
			}

			if (childViews != null)
			{
				for (int i = 0; i < childViews.Length; i++)
				{
					if (childViews[i] == this)
					{
						continue;
					}

					childViews[i].OnShow();
				}
			}

			AnimateIn();

			windowState = WindowState.Showing;
		}

		public virtual void OnHide()
		{
			if (windowState == WindowState.Hidden)
			{
				return;
			}

			if (childViews != null)
			{
				for (int i = 0; i < childViews.Length; i++)
				{
					if (childViews[i] == this)
					{
						continue;
					}

					childViews[i].OnHide();
				}
			}

			windowState = WindowState.Hidden;

			AnimateOut();
		}

		protected abstract void Setup();
	}
}
