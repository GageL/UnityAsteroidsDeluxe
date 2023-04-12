using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace AsteroidsDeluxe {
	public class PregameStartMenu : UIFadeControl {
		#region Public/Private Variables
		public static PregameStartMenu Instance { get; private set; }
		[SerializeField] private GameObject pressEnterText;
		[SerializeField] private Transform highScoresContainer;
		[SerializeField] private PregameHighScoreListElement highScoreListElementPrefab;
		#endregion

		#region Runtime Variables
		private Coroutine flashPressEnterTextProcess;
		#endregion

		#region Native Methods
		protected override void Awake() {
			base.Awake();
			Instance = this;
		}

		private void Update() {
			if (!IsShown()) { return; }
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				this.Hide(OnComplete: delegate {
					GameStateManager.Instance.StartGame();
				});
			}
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods
		protected override void OnShowStart() {
			base.OnShowStart();
			InitializeMenu();
		}

		protected override void OnHideComplete() {
			base.OnHideComplete();
			ResetMenu();
		}

		private void ResetMenu() {
			if (flashPressEnterTextProcess != null) {
				StopCoroutine(flashPressEnterTextProcess);
				flashPressEnterTextProcess = null;
			}
		}

		private void InitializeMenu() {
			ClearHighScoresContainer();
			PopulateHighScoresContainer();
			flashPressEnterTextProcess = StartCoroutine(FlashPressEnterTextProcess());
		}

		private void ClearHighScoresContainer() {
			foreach (Transform t in highScoresContainer.GetComponentsInChildren<Transform>()) {
				if (t != highScoresContainer) {
					Destroy(t.gameObject);
				}
			}
		}

		private void PopulateHighScoresContainer() {
			if (ApplicationManager.Instance.highScoreStorageData != null && ApplicationManager.Instance.highScoreStorageData.HighScores.Count > 0) {
				foreach (var score in ApplicationManager.Instance.highScoreStorageData.HighScores) {
					PregameHighScoreListElement _element = Instantiate(highScoreListElementPrefab, highScoresContainer);
					_element.PopulateElement(score);
				}
			}
		}

		private IEnumerator FlashPressEnterTextProcess() {
			while (true) {
				yield return new WaitForSeconds(.7f);
				pressEnterText.SetActive(!pressEnterText.activeSelf);
			}
		}
		#endregion
	}
}