//------------------------------------------------------------------------------
// <copyright file="EditorClassifierClassificationDefinition.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace IniColorizer
{
    /// <summary>
    /// Classification type definition export for EditorClassifier
    /// </summary>
    internal static class ClassificationDefinition
    {
        // This disables "The field is never used" compiler's warning. Justification: the field is used by MEF.
#pragma warning disable 169

        /// <summary>
        /// Defines the "SectionNameTypeClassifier" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name(ClassificationTypeNames.Section)]
        private static ClassificationTypeDefinition typeDefinition1;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(ClassificationTypeNames.Key)]
        private static ClassificationTypeDefinition typeDefinition2;

#pragma warning restore 169
    }
}
