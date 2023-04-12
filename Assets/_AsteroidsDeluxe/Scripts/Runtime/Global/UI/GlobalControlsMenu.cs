using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidsDeluxe {
	public class GlobalControlsMenu : MonoBehaviour {
		#region Public/Private Variables
		public static GlobalControlsMenu Instance { get; private set; }
		[SerializeField] private Toggle enableAudioToggle;
		[SerializeField] private Toggle infiniteModeToggle;
		#endregion

		#region Runtime Variables

		#endregion

		#region Native Methods
		private void Awake() {
			Instance = this;
		}

		private void OnEnable() {
			enableAudioToggle.onValueChanged.AddListener(OnEnableAudioToggleValueChanged);
		}

		void Start() {
			AudioManager.Instance.ToggleAudio(enableAudioToggle.isOn);
		}

		private void OnDestroy() {
			enableAudioToggle.onValueChanged.RemoveListener(OnEnableAudioToggleValueChanged);
		}
		#endregion

		#region Callback Methods
		private void OnEnableAudioToggleValueChanged(bool value) {
			AudioManager.Instance.ToggleAudio(value);
		}
		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void DisableGlobals() {
			infiniteModeToggle.gameObject.SetActive(false);
		}

		public void EnableGlobals() {
			infiniteModeToggle.gameObject.SetActive(true);
		}
		#endregion

		#region Private Methods

		#endregion
	}
}