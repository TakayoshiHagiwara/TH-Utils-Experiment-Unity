// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (NITech)
// Created: 2026/8/22
// Summary: Manager for state transition animation.
// -----------------------------------------------------------------------

using UnityEngine;

namespace TH.Utils.Experiment
{
    public class AnimationManager : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _animationDuration = 0.33333334f;
        [SerializeField] private float _hiddenLocalZ = 400f;

        private QuestionnaireView[] _questionnaires;

        private int _fromIndex = -1;
        private int _toIndex = -1;
        private float _elapsedTime;
        private bool _isAnimating;

        public bool IsAnimating => _isAnimating;

        /// <summary>
        /// Initializes questionnaire animation states.
        /// </summary>
        /// <param name="questionnaires">Questionnaires controlled by this manager.</param>
        public void Init(QuestionnaireView[] questionnaires)
        {
            _questionnaires = questionnaires;

            for (int i = 0; i < _questionnaires.Length; i++)
            {
                SetQuestionState(i, 0f, _hiddenLocalZ, false);
                _questionnaires[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Starts the initial questionnaire appearance animation.
        /// </summary>
        /// <param name="index">Questionnaire index.</param>
        /// <returns>True when the animation was started.</returns>
        public bool ShowFirstQuestionnaire(int index)
        {
            if (_isAnimating) return false;

            _fromIndex = -1;
            _toIndex = index;
            _elapsedTime = 0f;

            _questionnaires[index].gameObject.SetActive(true);
            SetQuestionState(index, 0f, _hiddenLocalZ, false);

            _isAnimating = true;
            return true;
        }

        /// <summary>
        /// Starts a questionnaire transition.
        /// </summary>
        /// <param name="currentIndex">Current questionnaire index.</param>
        /// <param name="nextIndex">Next questionnaire index.</param>
        /// <returns>True when the transition was accepted.</returns>
        public bool ChangeAnimationState(int currentIndex, int nextIndex)
        {
            if (_isAnimating || currentIndex == nextIndex) return false;

            _fromIndex = currentIndex;
            _toIndex = nextIndex;
            _elapsedTime = 0f;

            _questionnaires[nextIndex].gameObject.SetActive(true);

            SetQuestionState(currentIndex, 1f, 0f, false);
            SetQuestionState(nextIndex, 0f, _hiddenLocalZ, false);

            _isAnimating = true;
            return true;
        }

        /// <summary>
        /// Updates the active questionnaire transition.
        /// </summary>
        private void Update()
        {
            if (!_isAnimating) return;

            _elapsedTime += Time.unscaledDeltaTime;

            float normalizedTime = _animationDuration > 0f
                ? Mathf.Clamp01(_elapsedTime / _animationDuration)
                : 1f;

            float easedTime = Mathf.SmoothStep(0f, 1f, normalizedTime);

            if (_fromIndex >= 0)
                SetQuestionState(_fromIndex, 1f - easedTime, _hiddenLocalZ * easedTime, false);

            SetQuestionState(_toIndex, easedTime, _hiddenLocalZ * (1f - easedTime), false);

            if (normalizedTime >= 1f) CompleteAnimation();
        }

        /// <summary>
        /// Completes the current transition and enables the destination questionnaire.
        /// </summary>
        private void CompleteAnimation()
        {
            if (_fromIndex >= 0)
            {
                SetQuestionState(_fromIndex, 0f, _hiddenLocalZ, false);
                _questionnaires[_fromIndex].gameObject.SetActive(false);
            }

            SetQuestionState(_toIndex, 1f, 0f, true);

            _fromIndex = -1;
            _toIndex = -1;
            _isAnimating = false;
        }

        /// <summary>
        /// Sets the visual and interaction state of a questionnaire.
        /// </summary>
        /// <param name="index">Questionnaire index.</param>
        /// <param name="alpha">Canvas alpha.</param>
        /// <param name="localZ">Local Z position.</param>
        /// <param name="interactable">Whether UI interaction is enabled.</param>
        private void SetQuestionState(int index, float alpha, float localZ, bool interactable)
        {
            QuestionnaireView questionnaire = _questionnaires[index];

            questionnaire.CanvasGroup.alpha = alpha;
            questionnaire.CanvasGroup.interactable = interactable;
            questionnaire.CanvasGroup.blocksRaycasts = interactable;

            Vector3 localPosition = questionnaire.RectTransform.localPosition;
            localPosition.z = localZ;
            questionnaire.RectTransform.localPosition = localPosition;
        }
    }
}