using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AsteroidsDeluxe {
	public class PregameHighScoreListElement : MonoBehaviour {
		#region Public/Private Variables
		[SerializeField] private TMP_Text rankText;
		[SerializeField] private TMP_Text scoreText;
		[SerializeField] private TMP_Text initialsText;
		#endregion

		#region Runtime Variables

		#endregion

		#region Native Methods

		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void PopulateElement(HighScoreEntryData data) {
			this.gameObject.name = data.Initials;
			rankText.text = data.Rank.ToString() + ".";
			scoreText.text = data.Score.ToString();
			initialsText.text = data.Initials;
		}
		#endregion

		#region Private Methods

		#endregion
	}
}