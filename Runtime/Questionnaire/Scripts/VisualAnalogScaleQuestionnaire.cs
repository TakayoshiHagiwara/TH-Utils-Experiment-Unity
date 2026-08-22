// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (NITech)
// Created: 2026/8/22
// Summary: Control visual analog scale questionnaire.
// -----------------------------------------------------------------------

using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TH.Utils.Experiment
{
    public class VisualAnalogScaleQuestionnaire : QuestionnaireView
    {
        [Header("Slider")]
        [SerializeField] private Slider _slider;
        [SerializeField] private float _minimumValue = 0f;
        [SerializeField] private float _maximumValue = 100f;
        [SerializeField] private float _initialValue = 50f;
        [SerializeField, Min(0.001f)] private float _step = 1f;

        [Header("Long Press")]
        [SerializeField, Min(0f)] private float _longPressThreshold = 1f;
        [SerializeField, Min(0.01f)] private float _repeatInterval = 0.1f;
        [SerializeField, Min(1f)] private float _longPressMultiplier = 2f;

        [Header("Scale Labels")]
        [SerializeField] private TMP_Text _leftLabel;
        [SerializeField] private TMP_Text _rightLabel;
        [SerializeField] private string _leftLabelText = "Not at all";
        [SerializeField] private string _rightLabelText = "Extremely";

        [Header("Value Display")]
        [SerializeField] private bool _showValue;
        [SerializeField] private TMP_Text _valueLabel;
        [SerializeField, Range(0, 3)] private int _valueDecimalPlaces;

        private int _heldDirection;
        private float _holdTime;
        private float _nextRepeatTime;

        public override string QuestionnaireType => "VAS";
        public override float Score => _slider.value;
        public override string ResponseText => _slider.value.ToString("0.###", CultureInfo.InvariantCulture);

        /// <summary>
        /// Initializes the visual analog scale.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _slider.minValue = _minimumValue;
            _slider.maxValue = _maximumValue;
            _slider.SetValueWithoutNotify(Mathf.Clamp(_initialValue, _minimumValue, _maximumValue));

            _leftLabel.text = _leftLabelText;
            _rightLabel.text = _rightLabelText;

            ResetHoldState();

            if (_valueLabel != null)
            {
                _valueLabel.gameObject.SetActive(_showValue);
                UpdateValueLabel();
            }
        }

        /// <summary>
        /// Updates the visual analog scale input and handles accelerated long-press movement.
        /// </summary>
        /// <param name="direction">Horizontal direction. -1 for left, 1 for right, or 0 for neutral.</param>
        /// <param name="deltaTime">Unscaled frame delta time.</param>
        public override void UpdateInput(int direction, float deltaTime)
        {
            if (direction == 0)
            {
                ResetHoldState();
                return;
            }

            if (direction != _heldDirection)
            {
                _heldDirection = direction;
                _holdTime = 0f;
                _nextRepeatTime = _longPressThreshold;

                ChangeValue(direction, _step);
                return;
            }

            _holdTime += deltaTime;

            while (_holdTime >= _nextRepeatTime)
            {
                ChangeValue(direction, _step * _longPressMultiplier);
                _nextRepeatTime += _repeatInterval;
            }
        }

        /// <summary>
        /// Changes the slider value by the specified amount.
        /// </summary>
        /// <param name="direction">Horizontal movement direction.</param>
        /// <param name="amount">Value change amount.</param>
        private void ChangeValue(int direction, float amount)
        {
            float nextValue = Mathf.Clamp(
                _slider.value + amount * direction,
                _slider.minValue,
                _slider.maxValue);

            if (Mathf.Approximately(nextValue, _slider.value)) return;

            _slider.SetValueWithoutNotify(nextValue);
            UpdateValueLabel();
        }

        /// <summary>
        /// Resets the current long-press input state.
        /// </summary>
        private void ResetHoldState()
        {
            _heldDirection = 0;
            _holdTime = 0f;
            _nextRepeatTime = 0f;
        }

        /// <summary>
        /// Updates the optional numeric value display.
        /// </summary>
        private void UpdateValueLabel()
        {
            if (!_showValue || _valueLabel == null) return;

            switch (_valueDecimalPlaces)
            {
                case 0:
                    _valueLabel.SetText("{0:0}", _slider.value);
                    break;
                case 1:
                    _valueLabel.SetText("{0:0.0}", _slider.value);
                    break;
                case 2:
                    _valueLabel.SetText("{0:0.00}", _slider.value);
                    break;
                default:
                    _valueLabel.SetText("{0:0.000}", _slider.value);
                    break;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Updates the visual analog scale preview in the Unity Editor.
        /// </summary>
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_leftLabel != null) _leftLabel.text = _leftLabelText;
            if (_rightLabel != null) _rightLabel.text = _rightLabelText;
            if (_valueLabel != null) _valueLabel.gameObject.SetActive(_showValue);
        }
#endif
    }
}