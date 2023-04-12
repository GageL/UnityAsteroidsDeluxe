using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace AsteroidsDeluxe {
	public class ApplicationManager : MonoBehaviour {
		#region Public/Private Variables
		public static ApplicationManager Instance { get; private set; }
		public static readonly int MAX_HIGH_SCORES = 3;
		[SerializeField] private AudioClip appStartClip;
		#endregion

		#region Runtime Variables
		public HighScoreStorageData HighScoreStorageData { get { return highScoreStorageData; } }
		[Foldout("Runtime Debug")]
		public HighScoreStorageData highScoreStorageData;
		#endregion

		#region Native Methods
		private void Awake() {
			Instance = this;
			Application.targetFrameRate = 60;
			QualitySettings.vSyncCount = 2;
			CacheHighScoresFromPrefs();
		}

		private void Start() {
			AudioManager.Instance.PlayOneShot(appStartClip);
			PregameStartMenu.Instance.Show();
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		private static void SetDefaultResolution() {
			Screen.SetResolution(1000, 800, false);
		}
		#endregion

		#region Public Methods
		public bool ScoreIsHighScore(int score) {
			return highScoreStorageData.HighScores.Any(s => s.Score < score);
		}

		public void AddHighScore(HighScoreEntryData data, Action OnScoreAdded) {
			if (highScoreStorageData.HighScores.Count == MAX_HIGH_SCORES) {
				Debug.LogError("Cannot add any additional generic scores, cancelling");
				return;
			}
			highScoreStorageData.HighScores.Add(data);
			highScoreStorageData.HighScores = highScoreStorageData.HighScores.OrderBy(s => s.Score).ToList();
			PlayerPrefs.SetString(typeof(HighScoreStorageData).Name, JsonUtility.ToJson(highScoreStorageData));
			OnScoreAdded?.Invoke();
		}

		public void AddNewHighScore(HighScoreEntryData data, Action OnScoreAdded) {
			if (highScoreStorageData.HighScores.Count < MAX_HIGH_SCORES) {
				AddHighScore(data, OnScoreAdded);
				Debug.LogError("Cannot replace existing high scores when the max scores isnt met, adding score");
				return;
			}
			int _indexOfScore = -1;
			for (int i = 0; i < highScoreStorageData.HighScores.Count; i++) {
				if (data.Score > highScoreStorageData.HighScores[i].Score) {
					_indexOfScore = i;
					break;
				}
			}
			if (_indexOfScore != -1) {
				highScoreStorageData.HighScores[_indexOfScore] = data;
				highScoreStorageData.HighScores = highScoreStorageData.HighScores.OrderBy(s => s.Score).ToList();
				PlayerPrefs.SetString(typeof(HighScoreStorageData).Name, JsonUtility.ToJson(highScoreStorageData));
				OnScoreAdded?.Invoke();
			}
		}
		#endregion

		#region Private Methods
		private void CacheHighScoresFromPrefs() {
			if (!PlayerPrefs.HasKey(typeof(HighScoreStorageData).Name)) {
				PlayerPrefs.SetString(typeof(HighScoreStorageData).Name, JsonUtility.ToJson(new HighScoreStorageData() {
					HighScores = new List<HighScoreEntryData>() {
						new HighScoreEntryData() { Rank = 1, Score = 348246, Initials = "gdl" },
						new HighScoreEntryData() { Rank = 2, Score = 85193, Initials = "sam" },
						new HighScoreEntryData() { Rank = 3, Score = 10713, Initials = "tal" }
					}
				}));
			}
			if (PlayerPrefs.HasKey(typeof(HighScoreStorageData).Name)) {
				highScoreStorageData = JsonUtility.FromJson<HighScoreStorageData>(PlayerPrefs.GetString(typeof(HighScoreStorageData).Name));
			}
		}
		#endregion
	}
}