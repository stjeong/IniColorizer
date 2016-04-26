//------------------------------------------------------------------------------
// <copyright file="EditorClassifierFormat.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace IniColorizer
{
    /// <summary>
    /// Defines an editor format for the EditorClassifier type that has a purple background
    /// and is underlined.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = ClassificationTypeNames.Section)]
    [Name("SectionNameFormat")]
    [UserVisible(true)] // This should be visible to the end user
    [Order(Before = Priority.Default)] // Set the priority to be after the default classifiers
    internal sealed class SectionNameClassifierFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionNameClassifierFormat"/> class.
        /// </summary>
        public SectionNameClassifierFormat()
        {
            this.DisplayName = "SectionNameClassifier"; // Human readable version of the name
            this.IsBold = true;
            this.ForegroundColor = Colors.Blue;
        }
    }
}
