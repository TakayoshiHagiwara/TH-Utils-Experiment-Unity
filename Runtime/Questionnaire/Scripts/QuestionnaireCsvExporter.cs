// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (NITech)
// Created: 2026/8/22
// Summary: Export questionnaire results to the CSV file.
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace TH.Utils.Experiment
{
    public class QuestionnaireCsvExporter : MonoBehaviour
    {
        private const string DefaultFileName = "questionnaire.csv";

        private static readonly UTF8Encoding Utf8WithBom = new UTF8Encoding(true);

        [SerializeField, Tooltip("Folder name relative to Assets in the Editor or XXX_Data/Resources in a build. Leave empty to save directly to the base folder.")]
        private string _outputDirectory = "";

        [SerializeField] private string _fileName = DefaultFileName;

        public string LastSavedPath { get; private set; }

        /// <summary>
        /// Changes the output folder and file name.
        /// </summary>
        /// <param name="outputDirectory">Folder name relative to the default output directory.</param>
        /// <param name="fileName">CSV file name.</param>
        public void SetOutputPath(string outputDirectory, string fileName)
        {
            _outputDirectory = outputDirectory;
            _fileName = fileName;
        }

        /// <summary>
        /// Saves questionnaire results as a CSV file.
        /// </summary>
        /// <param name="questionnaires">All questionnaire instances.</param>
        /// <param name="presentationOrder">Order in which questionnaires were presented.</param>
        /// <returns>True when saving succeeded.</returns>
        public bool Save(QuestionnaireView[] questionnaires, int[] presentationOrder)
        {
            try
            {
                string directory = ResolveOutputDirectory();
                string fileName = string.IsNullOrWhiteSpace(_fileName) ? DefaultFileName : _fileName;

                if (!fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    fileName += ".csv";

                Directory.CreateDirectory(directory);

                string filePath = Path.Combine(directory, fileName);
                StringBuilder builder = new StringBuilder(Mathf.Max(1024, questionnaires.Length * 256));

                builder.AppendLine("presentation_order,question_id,questionnaire_type,question,score,response");

                for (int i = 0; i < presentationOrder.Length; i++)
                {
                    QuestionnaireView questionnaire = questionnaires[presentationOrder[i]];

                    builder.Append(i + 1).Append(',');
                    AppendCsvField(builder, questionnaire.QuestionId);
                    builder.Append(',');
                    AppendCsvField(builder, questionnaire.QuestionnaireType);
                    builder.Append(',');
                    AppendCsvField(builder, questionnaire.QuestionText);
                    builder.Append(',');
                    builder.Append(questionnaire.Score.ToString("0.0", CultureInfo.InvariantCulture));
                    builder.Append(',');
                    AppendCsvField(builder, questionnaire.ResponseText);
                    builder.AppendLine();
                }

                File.WriteAllText(filePath, builder.ToString(), Utf8WithBom);

                LastSavedPath = filePath;
                Debug.Log($"Questionnaire data saved: {filePath}");
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError($"Failed to save questionnaire data: {exception.Message}");
                return false;
            }
        }

        /// <summary>
        /// Resolves the CSV output directory.
        /// </summary>
        /// <returns>Resolved output directory.</returns>
        private string ResolveOutputDirectory()
        {
            string baseDirectory = Application.dataPath;

            return string.IsNullOrWhiteSpace(_outputDirectory)
                ? baseDirectory
                : Path.Combine(baseDirectory, _outputDirectory);
        }

        /// <summary>
        /// Appends a correctly escaped CSV field.
        /// </summary>
        /// <param name="builder">Destination builder.</param>
        /// <param name="value">CSV field value.</param>
        private static void AppendCsvField(StringBuilder builder, string value)
        {
            builder.Append('"');

            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    char character = value[i];
                    if (character == '"') builder.Append('"');
                    builder.Append(character);
                }
            }

            builder.Append('"');
        }
    }
}