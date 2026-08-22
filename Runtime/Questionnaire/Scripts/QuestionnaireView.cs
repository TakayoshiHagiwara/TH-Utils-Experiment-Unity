// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (NITech)
// Created: 2026/8/22
// Summary: Abstract class for questionnaire
// -----------------------------------------------------------------------

using TMPro;
using UnityEngine;

namespace TH.Utils.Experiment
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class QuestionnaireView : MonoBehaviour
    {
        [SerializeField] private string _questionId = "Q1";
        [SerializeField, TextArea(2, 5)] private string _questionText = "";
        [SerializeField] private TMP_Text _questionTextLabel;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;

        public string QuestionId => _questionId;
        public string QuestionText => _questionText;
        public CanvasGroup CanvasGroup => _canvasGroup;
        public RectTransform RectTransform => _rectTransform;

        public abstract string QuestionnaireType { get; }
        public abstract float Score { get; }
        public abstract string ResponseText { get; }

        /// <summary>
        /// Initializes the common questionnaire state.
        /// </summary>
        public virtual void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = (RectTransform)transform;
            _questionTextLabel.text = _questionText;
        }

        /// <summary>
        /// Updates questionnaire selection input.
        /// </summary>
        /// <param name="direction">Horizontal direction. -1 for left, 1 for right, or 0 for neutral.</param>
        /// <param name="deltaTime">Unscaled frame delta time.</param>
        public abstract void UpdateInput(int direction, float deltaTime);

#if UNITY_EDITOR
        /// <summary>
        /// Updates the question text preview in the Editor.
        /// </summary>
        protected virtual void OnValidate()
        {
            if (_questionTextLabel != null) _questionTextLabel.text = _questionText;
        }
#endif
    }
}