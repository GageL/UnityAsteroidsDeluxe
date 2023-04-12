using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsDeluxe {
	public class AudioManager : MonoBehaviour {
		#region Public/Private Variables
		public static AudioManager Instance { get; private set; }
		[SerializeField] private AudioSource oneShotSource;
		#endregion

		#region Runtime Variables

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
		public void PlayOneShot(AudioClip clip, float volume = 0.5f) {
			if (clip == null) { return; }
			oneShotSource.PlayOneShot(clip, volume);
		}

		public void ToggleAudio(bool value) {
			oneShotSource.enabled = value;
		}
		#endregion

		#region Private Methods

		#endregion
	}
}