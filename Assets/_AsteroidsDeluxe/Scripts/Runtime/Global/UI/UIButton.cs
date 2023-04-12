using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using NaughtyAttributes;
using ElRaccoone.Tweens;

namespace AsteroidsDeluxe {
	public class UIButton : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
		public enum TransitionType {
			None,
			Color,
			Sprite
		}
		public enum TargetGraphicType {
			Image,
			Text
		}
		public enum StateType {
			Idle,
			Highlighted,
			Pressed,
			Selected,
			SelectedHighlighted,
			SelectedPressed,
			Disabled
		}

		[Serializable]
		public class Element {
			public Graphic TargetGraphic { get => targetGraphic; }
			public TargetGraphicType GraphicType { get => graphicType; }
			public TransitionType Transition { get => transition; }
			[Serializable]
			public class CustomColorBlock {
				public Color NormalColor;
				public Color HighlightedColor;
				public Color PressedColor;
				public Color SelectedColor;
				public Color SelectedHighlightedColor;
				public Color SelectedPressedColor;
				public Color DisabledColor;
				public float ColorMultiplier;
				public float FadeDuration;
			}
			public CustomColorBlock ColorTransitions { get => colorTransitions; }
			[Serializable]
			public class CustomSpriteState {
				public Sprite NormalSprite;
				public Sprite HighlightedSprite;
				public Sprite PressedSprite;
				public Sprite SelectedSprite;
				public Sprite SelectedHighlightedSprite;
				public Sprite SelectedPressedSprite;
				public Sprite DisabledSprite;
			}
			public CustomSpriteState SpriteTransitions { get => spriteTransitions; }

			[SerializeField] private TransitionType transition = TransitionType.None;
			[HideIf("transition", TransitionType.None)]
			[AllowNesting]
			[SerializeField] private Graphic targetGraphic;
			[HideIf("transition", TransitionType.None)]
			[AllowNesting]
			[SerializeField] private TargetGraphicType graphicType = TargetGraphicType.Image;
			[ShowIf("Ed_ShowColorTransitions")]
			[AllowNesting]
			public CustomColorBlock colorTransitions = new CustomColorBlock() {
				NormalColor = new Color(1f, 1f, 1f),
				HighlightedColor = new Color(.8f, .8f, .8f),
				PressedColor = new Color(.65f, .65f, .65f),
				SelectedColor = new Color(1f, 1f, 1f),
				SelectedHighlightedColor = new Color(1f, 1f, 1f),
				SelectedPressedColor = new Color(1f, 1f, 1f),
				DisabledColor = new Color(1f, 1f, 1f, 0.3f),
				ColorMultiplier = 1f,
				FadeDuration = .1f
			};
			[ShowIf("Ed_ShowSpriteTransitions")]
			[AllowNesting]
			public CustomSpriteState spriteTransitions;

			private bool Ed_ShowColorTransitions() {
				return transition == TransitionType.None || transition == TransitionType.Sprite ? false : true;
			}
			private bool Ed_ShowSpriteTransitions() {
				return transition == TransitionType.None || transition == TransitionType.Color || graphicType == TargetGraphicType.Text ? false : true;
			}
		}

		#region Public/Private Variables
		public StateType CurrentState { get => currentState; }
		public bool IsSelectable { get => isSelectable; }
		public Animator Animator { get => animator; }

		//
		[SerializeField] private bool startDisabled = false;
		[SerializeField] private bool isSelectable = false;
		[ShowIf("isSelectable")]
		[SerializeField] private bool startSelected = false;
		[SerializeField] private bool isAnimation = false;

		//
		[HideIf("isAnimation")]
		[SerializeField] private Element[] elements;

		//
		[ShowIf("isAnimation")]
		[Required]
		[SerializeField] private Animator animator;
		[ShowIf("isAnimation")]
		[SerializeField] private AnimationTriggers animationTransitions;

		//
		[Foldout("Events")]
		public UnityEvent PointerEnterEvent;
		[Foldout("Events")]
		public UnityEvent PointerExitEvent;
		[Foldout("Events")]
		public UnityEvent PointerDownEvent;
		[Foldout("Events")]
		public UnityEvent PointerUpEvent;
		[Foldout("Events")]
		public UnityEvent PointerClickEvent;
		#endregion

		#region Runtime Variables
		public static UIButton CurrentHoveredButton;
		[Foldout("Runtime Debug")]
		[SerializeField] private StateType currentState;
		[Foldout("Runtime Debug")]
		[SerializeField] private bool isSelected;
		#endregion

		#region Native Methods
		protected override void Awake() {
			InitializeElements();
		}
		#endregion

		#region Callback Methods
		public virtual void OnPointerEnter(PointerEventData eventData) {
			if (currentState == StateType.Disabled) { return; }
			CurrentHoveredButton = this;
			if (isSelected) {
				SetButtonState(StateType.SelectedHighlighted);
			} else {
				SetButtonState(StateType.Highlighted);
			}
			PointerEnterEvent?.Invoke();
		}

		public virtual void OnPointerExit(PointerEventData eventData) {
			if (currentState == StateType.Disabled) { return; }
			CurrentHoveredButton = null;
			if (isSelected) {
				SetButtonState(StateType.Selected);
			} else {
				SetButtonState(StateType.Idle);
			}
			PointerExitEvent?.Invoke();
		}

		public virtual void OnPointerDown(PointerEventData eventData) {
			if (currentState == StateType.Disabled) { return; }
			if (isSelected) {
				SetButtonState(StateType.SelectedPressed);
			} else {
				SetButtonState(StateType.Pressed);
			}
			PointerDownEvent?.Invoke();
		}

		public virtual void OnPointerUp(PointerEventData eventData) {
			if (currentState == StateType.Disabled) { return; }
			if (isSelected) {
				SetButtonState(StateType.Selected);
			} else {
				SetButtonState(StateType.Idle);
			}
			PointerUpEvent?.Invoke();
		}

		public virtual void OnPointerClick(PointerEventData eventData) {
			if (currentState == StateType.Disabled) { return; }
			PointerClickEvent?.Invoke();
		}
		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void SetButtonState(StateType selectionState, bool ignoreDisabled = true) {
			if (!ignoreDisabled && currentState == StateType.Disabled) { return; }
			switch (selectionState) {
				case StateType.Idle:
					if (isSelectable) {
						isSelected = false;
					}
					currentState = StateType.Idle;
					for (int i = 0; i < elements.Length; i++) {
						if (!elements[i].TargetGraphic) { return; }
						switch (elements[i].Transition) {
							case TransitionType.Color:
								if (CurrentHoveredButton == this) {
									currentState = StateType.Highlighted;
									elements[i].TargetGraphic.TweenGraphicColor(elements[i].ColorTransitions.HighlightedColor, elements[i].ColorTransitions.FadeDuration);
								} else {
									elements[i].TargetGraphic.TweenGraphicColor(elements[i].ColorTransitions.NormalColor, elements[i].ColorTransitions.FadeDuration);
								}
								break;
							case TransitionType.Sprite:
								if (elements[i].GraphicType != TargetGraphicType.Image) { return; }
								if (CurrentHoveredButton == this) {
									if (!elements[i].SpriteTransitions.HighlightedSprite) { return; }
									currentState = StateType.Highlighted;
									((Image)elements[i].TargetGraphic).sprite = elements[i].SpriteTransitions.HighlightedSprite;
								} else {
									if (!elements[i].SpriteTransitions.NormalSprite) { return; }
									((Image)elements[i].TargetGraphic).sprite = elements[i].SpriteTransitions.NormalSprite;
								}
								break;
						}
					}
					break;
				case StateType.Highlighted:
					currentState = StateType.Highlighted;
					for (int i = 0; i < elements.Length; i++) {
						if (!elements[i].TargetGraphic) { return; }
						switch (elements[i].Transition) {
							case TransitionType.Color:
								elements[i].TargetGraphic.TweenGraphicColor(elements[i].ColorTransitions.HighlightedColor, elements[i].ColorTransitions.FadeDuration);
								break;
							case TransitionType.Sprite:
								if (elements[i].GraphicType != TargetGraphicType.Image) { return; }
								if (!elements[i].SpriteTransitions.HighlightedSprite) { return; }
								((Image)elements[i].TargetGraphic).sprite = elements[i].SpriteTransitions.HighlightedSprite;
								break;
						}
					}
					break;
				case StateType.Pressed:
					currentState = StateType.Pressed;
					for (int i = 0; i < elements.Length; i++) {
						if (!elements[i].TargetGraphic) { return; }
						switch (elements[i].Transition) {
							case TransitionType.Color:
								elements[i].TargetGraphic.TweenGraphicColor(elements[i].ColorTransitions.PressedColor, elements[i].ColorTransitions.FadeDuration);
								break;
							case TransitionType.Sprite:
								if (elements[i].GraphicType != TargetGraphicType.Image) { return; }
								if (!elements[i].SpriteTransitions.PressedSprite) { return; }
								((Image)elements[i].TargetGraphic).sprite = elements[i].SpriteTransitions.PressedSprite;
								break;
						}
					}
					break;
				case StateType.Selected:
					if (isSelectable) {
						isSelected = true;
						currentState = StateType.Selected;
						for (int i = 0; i < elements.Length; i++) {
							if (!elements[i].TargetGraphic) { return; }
							switch (elements[i].Transition) {
								case TransitionType.Color:
									if (CurrentHoveredButton == this) {
										currentState = StateType.SelectedHighlighted;
										elements[i].TargetGraphic.TweenGraphicColor(elements[i].ColorTransitions.SelectedHighlightedColor, elements[i].ColorTransitions.FadeDuration);
									} else {
										elements[i].TargetGraphic.TweenGraphicColor(elements[i].ColorTransitions.SelectedColor, elements[i].ColorTransitions.FadeDuration);
									}
									break;
								case TransitionType.Sprite:
									if (elements[i].GraphicType != TargetGraphicType.Image) { return; }
									if (CurrentHoveredButton == this) {
										if (!elements[i].SpriteTransitions.SelectedHighlightedSprite) { return; }
										currentState = StateType.SelectedHighlighted;
										((Image)elements[i].TargetGraphic).sprite = elements[i].SpriteTransitions.SelectedHighlightedSprite;
									} else {
										if (!elements[i].SpriteTransitions.SelectedSprite) { return; }
										((Image)elements[i].TargetGraphic).sprite = elements[i].SpriteTransitions.SelectedSprite;
									}
									break;
							}
						}
					}
					break;
				case StateType.SelectedHighlighted:
					currentState = StateType.SelectedHighlighted;
					for (int i = 0; i < elements.Length; i++) {
						if (!elements[i].TargetGraphic) { return; }
						switch (elements[i].Transition) {
							case TransitionType.Color:
								elements[i].TargetGraphic.TweenGraphicColor(elements[i].ColorTransitions.SelectedHighlightedColor, elements[i].ColorTransitions.FadeDuration);
								break;
							case TransitionType.Sprite:
								if (elements[i].GraphicType != TargetGraphicType.Image) { return; }
								if (!elements[i].SpriteTransitions.SelectedHighlightedSprite) { return; }
								((Image)elements[i].TargetGraphic).sprite = elements[i].SpriteTransitions.SelectedHighlightedSprite;
								break;
						}
					}
					break;
				case StateType.SelectedPressed:
					currentState = StateType.SelectedPressed;
					for (int i = 0; i < elements.Length; i++) {
						if (!elements[i].TargetGraphic) { return; }
						switch (elements[i].Transition) {
							case TransitionType.Color:
								elements[i].TargetGraphic.TweenGraphicColor(elements[i].ColorTransitions.SelectedPressedColor, elements[i].ColorTransitions.FadeDuration);
								break;
							case TransitionType.Sprite:
								if (elements[i].GraphicType != TargetGraphicType.Image) { return; }
								if (!elements[i].SpriteTransitions.SelectedPressedSprite) { return; }
								((Image)elements[i].TargetGraphic).sprite = elements[i].SpriteTransitions.SelectedPressedSprite;
								break;
						}
					}
					break;
				case StateType.Disabled:
					if (isSelectable) {
						isSelected = false;
					}
					currentState = StateType.Disabled;
					for (int i = 0; i < elements.Length; i++) {
						if (!elements[i].TargetGraphic) { return; }
						switch (elements[i].Transition) {
							case TransitionType.Color:
								elements[i].TargetGraphic.TweenGraphicColor(elements[i].ColorTransitions.DisabledColor, elements[i].ColorTransitions.FadeDuration);
								break;
							case TransitionType.Sprite:
								if (elements[i].GraphicType != TargetGraphicType.Image) { return; }
								if (!elements[i].SpriteTransitions.PressedSprite) { return; }
								((Image)elements[i].TargetGraphic).sprite = elements[i].SpriteTransitions.DisabledSprite;
								break;
						}
					}
					break;
			}
		}
		#endregion

		#region Private Methods
		private void InitializeElements() {
			if (startDisabled) {
				SetButtonState(StateType.Disabled);
			} else {
				if (startSelected) {
					SetButtonState(StateType.Selected);
				} else {
					SetButtonState(StateType.Idle);
				}
			}
		}

		private Graphic TryGetGraphic() {
			return this.GetComponent<Graphic>();
		}
		#endregion
	}
}