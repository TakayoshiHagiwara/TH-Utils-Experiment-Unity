// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (NITech)
// Created: 2026/8/22
// Summary: Control Likert scale questionnaire.
// -----------------------------------------------------------------------

using System;
using UnityEngine;

namespace TH.Utils.Experiment
{
    public class LikertScaleQuestionnaire : QuestionnaireView
    {
        [SerializeField, Min(2)] private int _scalePointCount = 7;

        [SerializeField]
        private string[] _scaleLabels =
        {
        "Strongly Disagree",
        "Disagree",
        "Somewhat Disagree",
        "Neutral",
        "Somewhat Agree",
        "Agree",
        "Strongly Agree"
    };

        [SerializeField] private Transform _optionRoot;
        [SerializeField] private LikertScaleOption _optionTemplate;

        private LikertScaleOption[] _options;
        private int _selectedIndex;
        private int _previousDirection;

        public override string QuestionnaireType => "Likert";
        public override float Score => _selectedIndex + 1;
        public override string ResponseText => _scaleLabels[_selectedIndex];

        /// <summary>
        /// Initializes the Likert scale and creates the required options.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _options = new LikertScaleOption[_scalePointCount];
            _options[0] = _optionTemplate;

            for (int i = 1; i < _scalePointCount; i++)
                _options[i] = Instantiate(_optionTemplate, _optionRoot);

            for (int i = 0; i < _scalePointCount; i++)
            {
                _options[i].SetLabel(_scaleLabels[i]);
                _options[i].SetSelected(false);
            }

            _selectedIndex = _scalePointCount / 2;
            _options[_selectedIndex].SetSelected(true);
            _previousDirection = 0;
        }

        /// <summary>
        /// Updates the selected Likert option once for each directional input.
        /// </summary>
        /// <param name="direction">Horizontal direction.</param>
        /// <param name="deltaTime">Unused frame delta time.</param>
        public override void UpdateInput(int direction, float deltaTime)
        {
            if (direction == 0)
            {
                _previousDirection = 0;
                return;
            }

            if (direction == _previousDirection) return;

            _previousDirection = direction;

            int nextIndex = Mathf.Clamp(_selectedIndex + direction, 0, _options.Length - 1);
            if (nextIndex == _selectedIndex) return;

            _options[_selectedIndex].SetSelected(false);
            _selectedIndex = nextIndex;
            _options[_selectedIndex].SetSelected(true);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Keeps the number of scale labels synchronized with the configured scale point count.
        /// </summary>
        protected override void OnValidate()
        {
            base.OnValidate();

            _scalePointCount = Mathf.Max(2, _scalePointCount);
            if (_scaleLabels == null || _scaleLabels.Length != _scalePointCount)
                Array.Resize(ref _scaleLabels, _scalePointCount);
        }
#endif
    }
}