// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (NITech)
// Created: 2026/8/22
// Summary: Manager for input.
// -----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;

namespace TH.Utils.Experiment
{
    public class ControllerInputManager : MonoBehaviour
    {
        [SerializeField] private InputActionReference _navigateAction;
        [SerializeField] private InputActionReference _nextAction;
        [SerializeField] private InputActionReference _previousAction;

        [SerializeField, Range(0.1f, 1f)] private float _axisThreshold = 0.5f;

        private bool _requireNeutral;

        /// <summary>
        /// Enables the questionnaire input actions.
        /// </summary>
        private void OnEnable()
        {
            _navigateAction.action.Enable();
            _nextAction.action.Enable();
            _previousAction.action.Enable();
        }

        /// <summary>
        /// Disables the questionnaire input actions.
        /// </summary>
        private void OnDisable()
        {
            _navigateAction.action.Disable();
            _nextAction.action.Disable();
            _previousAction.action.Disable();
        }

        /// <summary>
        /// Returns the current horizontal navigation direction.
        /// </summary>
        /// <returns>-1 for left, 1 for right, or 0 for neutral.</returns>
        public int ReadHorizontalDirection()
        {
            float horizontal = _navigateAction.action.ReadValue<Vector2>().x;
            int direction = horizontal > _axisThreshold ? 1 : horizontal < -_axisThreshold ? -1 : 0;

            if (!_requireNeutral) return direction;

            if (direction == 0) _requireNeutral = false;
            return 0;
        }

        /// <summary>
        /// Returns whether the next action was triggered during the current frame.
        /// </summary>
        public bool IsPressNext() => _nextAction.action.triggered;

        /// <summary>
        /// Returns whether the previous action was triggered during the current frame.
        /// </summary>
        public bool IsPressPrevious() => _previousAction.action.triggered;

        /// <summary>
        /// Prevents navigation until the horizontal input returns to neutral.
        /// </summary>
        public void RequireNeutral() => _requireNeutral = true;
    }
}