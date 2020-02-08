using System;
// It is important to use this using and not using System.Composition;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace EditorMargin
{
    [Export(typeof(ICocoaTextViewMarginProvider))]
    [Name(nameof(EvenOddMarginProvider))]
    [Order(Before = PredefinedMarginNames.LineNumber)]
    [Order(After = PredefinedMarginNames.Glyph)]
    [MarginContainer(PredefinedMarginNames.Left)]
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    class EvenOddMarginProvider : ICocoaTextViewMarginProvider
    {
        public ICocoaTextViewMargin CreateMargin(ICocoaTextViewHost textViewHost, ICocoaTextViewMargin marginContainer)
        {
            return new EvenOddMargin(textViewHost.TextView);
        }
    }
}
