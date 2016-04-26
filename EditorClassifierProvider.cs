//------------------------------------------------------------------------------
// <copyright file="EditorClassifierProvider.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace IniColorizer
{
    /// <summary>
    /// Classifier provider. It adds the classifier to the set of classifiers.
    /// </summary>
    [Export(typeof(IClassifierProvider))]
    [ContentType("text")] // This classifier applies to all text files.
    internal class EditorClassifierProvider : IClassifierProvider
    {
        // Disable "Field is never assigned to..." compiler's warning. Justification: the field is assigned by MEF.
#pragma warning disable 649

        /// <summary>
        /// Classification registry to be used for getting a reference
        /// to the custom classification type later.
        /// </summary>
        [Import]
        private IClassificationTypeRegistryService classificationRegistry;


        private readonly ITextDocumentFactoryService _textDocumentFactoryService;

        [ImportingConstructor]
        internal EditorClassifierProvider(ITextDocumentFactoryService textDocumentFactoryService)
        {
            _textDocumentFactoryService = textDocumentFactoryService;
        }

#pragma warning restore 649

        #region IClassifierProvider

        /// <summary>
        /// Gets a classifier for the given text buffer.
        /// </summary>
        /// <param name="buffer">The <see cref="ITextBuffer"/> to classify.</param>
        /// <returns>A classifier for the text buffer, or null if the provider cannot do so in its current state.</returns>
        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            if (IsINIFile(buffer) == false)
            {
                return null;
            }

            return buffer.Properties.GetOrCreateSingletonProperty<EditorClassifier>(creator: () => new EditorClassifier(this.classificationRegistry));
        }

        #endregion

        private bool IsINIFile(ITextBuffer buffer)
        {
            if (_textDocumentFactoryService == null)
            {
                return false;
            }

            ITextDocument textDocument;
            if (_textDocumentFactoryService.TryGetTextDocument(buffer, out textDocument) == false)
            {
                return false;
            }

            string ext = System.IO.Path.GetExtension(textDocument.FilePath);

            if (string.IsNullOrEmpty(ext) == true)
            {
                return false;
            }

            return ext.ToUpper() == ".INI";
        }
    }
}
