// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (NITech)
// Created: 2026/8/22
// Summary: Control Likert scale options.
// -----------------------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TH.Utils.Experiment
{
    public class LikertScaleOption : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Image _selectionImage;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Color _selectedColor = Color.red;
        [SerializeField] private Color _defaultColor = Color.white;

        /// <summary>
        /// Sets the displayed label.
        /// </summary>
        /// <param name="label">Label text.</param>
        public void SetLabel(string label) => _label.text = label;

        /// <summary>
        /// Sets the selected visual state without invoking Toggle callbacks.
        /// </summary>
        /// <param name="selected">Whether this option is selected.</param>
        public void SetSelected(bool selected)
        {
            _toggle.SetIsOnWithoutNotify(selected);
            _selectionImage.color = selected ? _selectedColor : _defaultColor;
        }
    }
}