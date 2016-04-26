using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace IniColorizer
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = ClassificationTypeNames.Key)]
    [Name("KeyValueFormat")]
    [UserVisible(true)] // This should be visible to the end user
    [Order(Before = Priority.Default)] // Set the priority to be after the default classifiers
    internal sealed class KeyValueClassifierFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueClassifierFormat"/> class.
        /// </summary>
        public KeyValueClassifierFormat()
        {
            this.DisplayName = "KeyValueClassifier"; // Human readable version of the name
            this.ForegroundColor = Colors.Red;
            this.IsBold = true;
        }
    }
}
