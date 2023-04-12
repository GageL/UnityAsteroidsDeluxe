using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace AsteroidsDeluxe {
	public class GameStateManager : MonoBehaviour {
		#region Public/Private Variables
		public static GameStateManager Instance { get; private set; }
		public GameplayMilestoneData GameplayMilestoneData { get { return gameplayMilestoneData; } }
		public Transform RuntimeFolder { get { return runtimeFolder; } }
		[SerializeField] private Transform runtimeFolder;
		[SerializeField] private PlayerActor playerActorPrefab;
		[SerializeField] private GameplayMilestoneData gameplayMilestoneData;
		[SerializeField] private float gameStartDelay = 2.5f;
		[SerializeField] private int startingLives = 3;
		[SerializeField] private float respawnDelay = 2f;
		[SerializeField] private AudioClip gameStartClip;
		[SerializeField] private AudioClip gameEndClip;
		#endregion

		#region Runtime Variables
		public PlayerActor PlayerInstance { get { return playerInstance; } }
		public bool IsPlaying { get { return isPlaying; } }
		[Foldout("Runtime Debug")]
		[SerializeField] private PlayerActor playerInstance;
		[Foldout("Runtime Debug")]
		[SerializeField] private bool isPlaying;
		[Foldout("Runtime Debug")]
		[SerializeField] private int currentLives;
		[Foldout("Runtime Debug")]
		[SerializeField] private int currentScore;
		#endregion

		#region Native Methods
		private void Awake() {
			Instance = this;
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void StartGame() {
			isPlaying = true;
			IngameGameplayMenu.Instance.Show();
			currentLives = startingLives;
			StartCoroutine(StartGameProcess());
			GlobalControlsMenu.Instance.DisableGlobals();
		}

		public void KillPlayer() {
			DestroyPlayerInstance();
			if (currentLives == 0) {
				EndGame();
				return;
			}
			currentLives--;
			StartCoroutine(TakeLifeProcess());
		}

		public void AddScore(int value) {
			currentScore += value;
			IngameGameplayMenu.Instance.UpdateScore(currentScore);
		}

		public void EndGame() {
			isPlaying = false;
			AudioManager.Instance.PlayOneShot(gameEndClip);
			ActorWaveManager.Instance.EndGame();
			IngameGameplayMenu.Instance.EndGame();
		}
		#endregion

		#region Private Methods
		private IEnumerator StartGameProcess() {
			DestroyPlayerInstance();
			ActorWaveManager.Instance.ClearWaveActors();
			IngameGameplayMenu.Instance.ShowGame(currentLives);
			yield return new WaitForSeconds(gameStartDelay);
			currentLives--;
			SpawnPlayer();
			currentScore = 0;
			AudioManager.Instance.PlayOneShot(gameStartClip);
			ActorWaveManager.Instance.StartWaves();
			IngameGameplayMenu.Instance.SpawnPlayer();
			IngameGameplayMenu.Instance.StartGame();
		}

		private IEnumerator TakeLifeProcess() {
			yield return new WaitForSeconds(respawnDelay);
			SpawnPlayer();
		}

		private void SpawnPlayer() {
			playerInstance = Instantiate(playerActorPrefab, Vector3.forward * 5, Quaternion.identity, runtimeFolder);
			playerInstance.PlayerActorInput.SetControl(true);
			playerInstance.PlayerActorCombat.SetFire(true);
			IngameGameplayMenu.Instance.SpawnPlayer();
		}

		private void DestroyPlayerInstance() {
			if (playerInstance != null) {
				Destroy(playerInstance.gameObject);
				playerInstance = null;
			}
		}
		#endregion
	}
}