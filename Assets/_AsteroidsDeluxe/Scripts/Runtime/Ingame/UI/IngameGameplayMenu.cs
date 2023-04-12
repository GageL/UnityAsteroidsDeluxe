using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace AsteroidsDeluxe {
	public class IngameGameplayMenu : UIFadeControl {
		#region Public/Private Variables
		public static IngameGameplayMenu Instance { get; private set; }
		[SerializeField] private GameObject gameOverHolder;
		[SerializeField] private GameObject pressEnterText;
		[SerializeField] private TMP_Text playerNameText;
		[SerializeField] private TMP_Text currentScoreText;
		[SerializeField] private GameObject playerLifeElementPrefab;
		[SerializeField] private Transform currentLivesContainer;
		[SerializeField] private TMP_Text nextMilestoneText;
		[SerializeField] private GameObject clearedWaveText;
		[SerializeField] private GameObject gameCompleteText;
		#endregion

		#region Runtime Variables
		private Coroutine flashPressEnterTextProcess;
		#endregion

		#region Native Methods
		protected override void Awake() {
			base.Awake();
			Instance = this;
			ResetMenu();
		}

		private void Update() {
			if (!IsShown()) { return; }
			if (GameStateManager.Instance.IsPlaying) { return; }
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				ResetMenu();
				GameStateManager.Instance.StartGame();
			}
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void ShowGame(int lives) {
			for (int i = 0; i < lives; i++) {
				Instantiate(playerLifeElementPrefab, currentLivesContainer);
			}
			playerNameText.gameObject.SetActive(true);
			currentScoreText.text = "0";
		}

		public void StartGame() {
			playerNameText.gameObject.SetActive(false);
		}

		public void UpdateScore(int score) {
			currentScoreText.text = score.ToString();
		}

		public void SpawnPlayer() {
			Destroy(currentLivesContainer.GetChild(currentLivesContainer.childCount - 1).gameObject);
		}

		public void StartNextWave() {
			clearedWaveText.SetActive(false);
			GlobalControlsMenu.Instance.DisableGlobals();
		}

		public void WaveCleared() {
			clearedWaveText.SetActive(true);
		}

		public void GameComplete() {
			clearedWaveText.SetActive(false);
			gameCompleteText.SetActive(true);
		}

		public void EndGame() {
			gameOverHolder.SetActive(true);
			flashPressEnterTextProcess = StartCoroutine(FlashPressEnterTextProcess());
			GlobalControlsMenu.Instance.EnableGlobals();
		}
		#endregion

		#region Private Methods
		protected override void OnShowStart() {
			base.OnShowStart();
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
			gameOverHolder.SetActive(false);
			currentScoreText.text = string.Empty;
			nextMilestoneText.text = string.Empty;
			ClearCurrentLivesContainer();
			clearedWaveText.SetActive(false);
			gameCompleteText.SetActive(false);
		}

		private void ClearCurrentLivesContainer() {
			foreach (Transform t in currentLivesContainer.GetComponentsInChildren<Transform>()) {
				if (t != currentLivesContainer) {
					Destroy(t.gameObject);
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