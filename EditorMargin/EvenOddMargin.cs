using System;
using AppKit;
using CoreGraphics;
using Microsoft.VisualStudio.Text.Editor;

namespace EditorMargin
{
    class EvenOddMargin : NSView, ICocoaTextViewMargin
    {
        static nfloat MarginWidth = 6.0f;

        private ICocoaTextView textView;

        public EvenOddMargin(ICocoaTextView textView)
        {
            this.textView = textView;
            this.textView.LayoutChanged += OnTextViewLayoutChanged;
        }

        private void OnTextViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            // Every time textview re-renders UI we want to update
            // maybe we could optimise here to check if line numbers or scrolling changed.
            NeedsDisplay = true;
        }

        // We could return here NSView that is implemented in different class
        // but for such simple view, we just implement it in margin itself.
        public NSView VisualElement => this;

        public override CGSize IntrinsicContentSize => new CGSize(MarginWidth, NoIntrinsicMetric);

        // How wide or tall(depending on orientation) this margin should be
        public double MarginSize => MarginWidth;

        // You can make it dependable on user preferences to not show/enable this margin
        public bool Enabled => true;

        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            // This is only relevant if we have Margins nested otherwise just do this.
            if (marginName == nameof(EvenOddMargin))
                return this;
            return null;
        }

        // Since ITextView editor comes from Windows world, where Y=0 is on top of screen
        // we have to set IsFlipped => true, so our NSView behaves same way, which makes it
        // easier to reason about things.
        public override bool IsFlipped => true;

        public override void DrawRect(CGRect dirtyRect)
        {
            // We have NSView here of which height is same as TextView height
            // and width is 6px. What we do here is use CoreGraphics to render
            // 6px wide lines(full width of Margin) from even lines top to bottom
            var context = NSGraphicsContext.CurrentContext.CGContext;
            context.SetStrokeColor(NSColor.Red.CGColor);
            context.SetLineWidth(MarginWidth);
            foreach (var line in textView.TextViewLines)
            {
                // == 1 is even, because lines numbers here are 0 based
                var even = line.Start.GetContainingLine().LineNumber % 2 == 1;
                var top = line.Top - textView.ViewportTop;
                var bottom = line.Bottom - textView.ViewportTop;
                if (even)
                    context.AddLines(new[] { new CGPoint(MarginWidth / 2, top), new CGPoint(MarginWidth / 2, bottom) });
            }
            context.StrokePath();
        }
    }
}
