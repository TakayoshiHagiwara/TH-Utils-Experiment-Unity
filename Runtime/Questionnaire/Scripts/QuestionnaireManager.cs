// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (NITech)
// Created: 2026/8/22
// Summary: Manager for questionnaire.
// -----------------------------------------------------------------------

using UnityEngine;

namespace TH.Utils.Experiment
{
    public class QuestionnaireManager : MonoBehaviour
    {
        [SerializeField] private ControllerInputManager _controllerInputManager;
        [SerializeField] private AnimationManager _animationManager;
        [SerializeField] private QuestionnaireCsvExporter _csvExporter;

        [SerializeField] private Transform _questionnaireRoot;

        [SerializeField, Tooltip("Randomizes questionnaire presentation order when enabled.")]
        private bool _isRandomizeOrder;

        private QuestionnaireView[] _questionnaires;
        private int[] _questionnaireOrder;

        private QuestionnaireView _currentQuestionnaire;
        private int _currentQuestionnaireNum;
        private bool _isCompleted;

        public int CurrentQuestionnaireNum => _currentQuestionnaireNum;
        public int[] QuestionnaireOrder => _questionnaireOrder;

        /// <summary>
        /// Initializes all questionnaires.
        /// </summary>
        private void Start() => Init();

        /// <summary>
        /// Initializes questionnaires, presentation order, and animation state.
        /// </summary>
        private void Init()
        {
            _questionnaires = _questionnaireRoot.GetComponentsInChildren<QuestionnaireView>(true);

            if (_questionnaires.Length == 0)
            {
                Debug.LogWarning("No questionnaires were found.");
                return;
            }

            for (int i = 0; i < _questionnaires.Length; i++)
                _questionnaires[i].Initialize();

            _questionnaireOrder = MakeQuestionnaireOrder(_questionnaires.Length, _isRandomizeOrder);

            _currentQuestionnaireNum = 0;
            _currentQuestionnaire = _questionnaires[_questionnaireOrder[0]];

            _animationManager.Init(_questionnaires);
            _animationManager.ShowFirstQuestionnaire(_questionnaireOrder[0]);

            _controllerInputManager.RequireNeutral();
        }

        /// <summary>
        /// Processes questionnaire input when no transition is active.
        /// </summary>
        private void Update()
        {
            if (_isCompleted || _questionnaires == null || _questionnaires.Length == 0) return;
            if (_animationManager.IsAnimating) return;

            if (_controllerInputManager.IsPressPrevious())
            {
                if (_currentQuestionnaireNum > 0)
                    ChangeQuestionnaire(_currentQuestionnaireNum - 1);

                return;
            }

            if (_controllerInputManager.IsPressNext())
            {
                if (_currentQuestionnaireNum < _questionnaireOrder.Length - 1)
                    ChangeQuestionnaire(_currentQuestionnaireNum + 1);
                else
                    CompleteQuestionnaire();

                return;
            }

            int direction = _controllerInputManager.ReadHorizontalDirection();
            _currentQuestionnaire.UpdateInput(direction, Time.unscaledDeltaTime);
        }

        /// <summary>
        /// Changes the current questionnaire.
        /// </summary>
        /// <param name="nextOrderIndex">Destination index in presentation order.</param>
        private void ChangeQuestionnaire(int nextOrderIndex)
        {
            int currentQuestionnaireIndex = _questionnaireOrder[_currentQuestionnaireNum];
            int nextQuestionnaireIndex = _questionnaireOrder[nextOrderIndex];

            if (!_animationManager.ChangeAnimationState(currentQuestionnaireIndex, nextQuestionnaireIndex))
                return;

            _currentQuestionnaireNum = nextOrderIndex;
            _currentQuestionnaire = _questionnaires[nextQuestionnaireIndex];

            _controllerInputManager.RequireNeutral();
        }

        /// <summary>
        /// Completes the questionnaire and saves all responses.
        /// </summary>
        private void CompleteQuestionnaire()
        {
            _isCompleted = true;

            _currentQuestionnaire.CanvasGroup.interactable = false;
            _currentQuestionnaire.CanvasGroup.blocksRaycasts = false;

            _csvExporter.Save(_questionnaires, _questionnaireOrder);
        }

        /// <summary>
        /// Creates the questionnaire presentation order.
        /// </summary>
        /// <param name="count">Number of questionnaires.</param>
        /// <param name="shuffle">Whether the order should be randomized.</param>
        /// <returns>Questionnaire presentation order.</returns>
        private static int[] MakeQuestionnaireOrder(int count, bool shuffle)
        {
            int[] order = new int[count];

            for (int i = 0; i < count; i++)
                order[i] = i;

            if (shuffle) Shuffle(order);

            return order;
        }

        /// <summary>
        /// Randomizes an array using the Fisher-Yates algorithm.
        /// </summary>
        /// <param name="order">Array to randomize.</param>
        private static void Shuffle(int[] order)
        {
            for (int i = order.Length - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);

                int temporary = order[i];
                order[i] = order[randomIndex];
                order[randomIndex] = temporary;
            }
        }
    }
}