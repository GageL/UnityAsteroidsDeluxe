using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using NaughtyAttributes;
using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;

namespace AsteroidsDeluxe {
	[DefaultExecutionOrder(-2)]
	[RequireComponent(typeof(CanvasGroup))]
	public class UIFadeControl : MonoBehaviour {
		#region Public/Private Variables
		public Action OnShowStarted;
		public Action OnShowCompleted;
		public Action OnHideStarted;
		public Action OnHideCompleted;

		[Foldout("UIFadeControl")]
		[SerializeField] private CanvasGroup canvasGroup;
		[Foldout("UIFadeControl")]
		[SerializeField] private bool startShown = false;
		[Foldout("UIFadeControl")]
		[SerializeField] private float fadeInSpeed = 0.5f;
		[Foldout("UIFadeControl")]
		[SerializeField] private EaseType fadeInEase = EaseType.QuadInOut;
		[Foldout("UIFadeControl")]
		[SerializeField] private float fadeOutSpeed = 0.5f;
		[Foldout("UIFadeControl")]
		[SerializeField] private EaseType fadeOutEase = EaseType.QuadInOut;
		public float FadeOutSpeed { get { return fadeOutSpeed; } }
		public float FadeInSpeed { get { return fadeInSpeed; } }
		#endregion

		#region Runtime Variables
		[Foldout("Runtime Debug")]
		[SerializeField] protected bool isControlShown = false;
		#endregion

		#region Native Methods
		private void OnValidate() {
			TryGetRef();
		}

		protected virtual void Awake() {
			if (canvasGroup == null) { return; }
			InstantControl(startShown);
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void InstantControl(bool show) {
			if (canvasGroup == null) { return; }
			canvasGroup.alpha = show ? 1 : 0;
			isControlShown = show;
			SetValues(show);
		}

		public void Show(Action OnStart = null, Action OnComplete = null) {
			if (canvasGroup == null) { return; }
			canvasGroup.TweenCanvasGroupAlpha(1, fadeInSpeed).SetEase(fadeInEase).SetOnStart(() => { OnShowStart(); OnStart?.Invoke(); OnShowStarted?.Invoke(); }).SetOnComplete(() => { OnShowComplete(); SetValues(true); isControlShown = true; OnComplete?.Invoke(); OnShowCompleted?.Invoke(); });
		}
		protected virtual void OnShowStart() { }
		protected virtual void OnShowComplete() { }

		public void Hide(Action OnStart = null, Action OnComplete = null) {
			if (canvasGroup == null) { return; }
			canvasGroup.TweenCanvasGroupAlpha(0, fadeOutSpeed).SetEase(fadeOutEase).SetOnStart(() => { OnHideStart(); SetValues(false); OnStart?.Invoke(); OnHideStarted?.Invoke(); }).SetOnComplete(() => { OnHideComplete(); isControlShown = false; OnComplete?.Invoke(); OnHideCompleted?.Invoke(); });
		}
		protected virtual void OnHideStart() { }
		protected virtual void OnHideComplete() { }

		public bool IsShown() {
			return isControlShown;
		}
		#endregion

		#region Private Methods
		private void TryGetRef() {
			if (canvasGroup != null) { return; }
			canvasGroup = this.GetComponent<CanvasGroup>();
		}

		private void SetValues(bool show) {
			if (canvasGroup == null) { return; }
			canvasGroup.interactable = show ? true : false;
			canvasGroup.blocksRaycasts = show ? true : false;
		}
		#endregion
	}
}