//------------------------------------------------------------------------------
// <copyright file="EditorClassifier.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace IniColorizer
{
    /// <summary>
    /// Classifier that classifies all text as an instance of the "EditorClassifier" classification type.
    /// </summary>
    internal class EditorClassifier : IClassifier
    {
        private readonly IClassificationType sectionNameClassifierType;
        private readonly IClassificationType keyValueClassifierType;
        private readonly IClassificationType commentClassifierType;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorClassifier"/> class.
        /// </summary>
        /// <param name="registry">Classification registry.</param>
        internal EditorClassifier(IClassificationTypeRegistryService registry)
        {
            this.sectionNameClassifierType = registry.GetClassificationType(ClassificationTypeNames.Section);
            this.keyValueClassifierType = registry.GetClassificationType(ClassificationTypeNames.Key);

            this.sectionNameClassifierType = registry.GetClassificationType("keyword");
            this.keyValueClassifierType = registry.GetClassificationType("symbol definition");

            this.commentClassifierType = registry.GetClassificationType("comment");
        }

        #region IClassifier

#pragma warning disable 67

        /// <summary>
        /// An event that occurs when the classification of a span of text has changed.
        /// </summary>
        /// <remarks>
        /// This event gets raised if a non-text change would affect the classification in some way,
        /// for example typing /* would cause the classification to change in C# without directly
        /// affecting the span.
        /// </remarks>
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

#pragma warning restore 67

        /// <summary>
        /// Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text.
        /// </summary>
        /// <remarks>
        /// This method scans the given SnapshotSpan for potential matches for this classification.
        /// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </remarks>
        /// <param name="span">The span currently being classified.</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var result = new List<ClassificationSpan>();

            string modified = span.GetText();
            int commentPos = modified.IndexOf(';');

            do
            {
                if (AddIfSection(result, span, modified, commentPos, this.sectionNameClassifierType) == true)
                {
                    break;
                }

                if (AddIfKeyValue(result, span, modified, commentPos, this.keyValueClassifierType) == true)
                {
                    break;
                }

            } while (false);

            if (commentPos != -1)
            {
                int commentStart = span.Start + commentPos;
                int commentLength = span.Length - commentPos;
                result.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(commentStart, commentLength)), commentClassifierType));
            }

            return result;
        }
        
        #endregion

        private bool AddIfKeyValue(List<ClassificationSpan> result, SnapshotSpan span, string modified, int commentPos, IClassificationType type)
        {
            int pos = modified.IndexOf('=');
            if (pos == -1)
            {
                return false;
            }

            if (IsInComment(commentPos, pos) == true)
            {
                return false;
            }

            result.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(span.Start, pos)), type));

            return true;
        }

        private bool AddIfSection(List<ClassificationSpan> result, SnapshotSpan span, string modified, int commentPos, IClassificationType type)
        {
            int startPos, endPos;

            if (RetrieveSection(modified, out startPos, out endPos) == false)
            {
                return false;
            }

            if (IsInComment(commentPos, startPos) == true)
            {
                return false;
            }

            result.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(span.Start, span.Length)), type));

            return true;
        }

        private bool RetrieveSection(string modified, out int startPos, out int endPos)
        {
            endPos = 0;
            startPos = modified.IndexOf('[');

            if (startPos == -1)
            {
                return false;
            }

            endPos = modified.IndexOf(']', startPos);

            if (endPos == -1 || startPos > endPos)
            {
                return false;
            }

            return true;
        }

        private bool IsInComment(int commentPos, int pos)
        {
            return (commentPos != -1 && commentPos < pos);
        }
    }
}
