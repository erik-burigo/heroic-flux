using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for IconicTextBox.xaml
    /// </summary>
    public partial class IconicTextBox: UserControl
    {
        public IconicTextBox()
        {
            InitializeComponent();
        }

        public Thickness InnerMargin { get { return TheBox.Padding; } set { TheBox.Padding = value; } }

        public new FontStretch FontStretch { get { return TheBox.FontStretch; } set { TheBox.FontStretch = value; } }

        public FontStyle FontSyle { get { return TheBox.FontStyle; } set { TheBox.FontStyle = value; } }

        public new FontWeight FontWeight { get { return TheBox.FontWeight; } set { TheBox.FontWeight = value; } }

        public Double LineHeight { get { return TheBox.LineHeight; } set { TheBox.LineHeight = value; } }

        public String Text { get { return _rawText; } set { _rawText = value; } }

        public TextAlignment TextAlignment { get { return TheBox.TextAlignment; } set { TheBox.TextAlignment = value; } }

        public TextDecorationCollection TextDecorations { get { return TheBox.TextDecorations; } set { TheBox.TextDecorations = value; } }
     
        public void SetText(String rawText)
        {
            Text = rawText;
            TheBox.BeginInit();
            TheBox.Inlines.Clear();
            TheBox.Inlines.AddRange(GetInlines(rawText));
            TheBox.EndInit();
        }

        private void ExtractText(String raw, out String prefix, out String key, out String suffix)
        {
            var i = 0;
            var test = "";
            while (i<raw.Length)
            {
                test += raw[i];
                if (InlineSymbol.IsTailMapped(test, out key))
                {
                    var splitPoint1 = test.Length-key.Length;
                    var splitPoint2 = test.Length;

                    prefix = raw.Substring(0, splitPoint1);
                    suffix = raw.Substring(splitPoint2);
                    return;
                }
                i++;
            }

            prefix = raw;
            key = null;
            suffix = null;
        }

        private IEnumerable<Inline> GetInlines(String raw)
        {
            if (String.IsNullOrEmpty(raw))
                yield break;

            String prefix;
            String key;
            String suffix;

            ExtractText(raw, out prefix, out key, out suffix);

            foreach (var inlinePrefix in Process(prefix))
            {
                yield return inlinePrefix;
            }

            if (key != null)
            {
                foreach (var symbol in InlineSymbol.GetSymbolStreak(key, FontSize))
                {
                    yield return symbol.Inline;
                }                
            }

            if (suffix != null)
            {
                foreach (var item in GetInlines(suffix))
                {
                    yield return item;
                }
            }
        }

        private IEnumerable<Inline> Process(string raw)
        {
            if (String.IsNullOrEmpty(raw))
                yield break;

            var i = 0;
            var test = "";
            while (i<raw.Length)
            {
                test += raw[i];

                // 012345 6 7890 1 234567 prefix * bold * suffix split1 split2
                var s1s = -1;
                var s1e = -1;
                var s2s = -1;
                var s2e = -1;
                FontStyle? style = null;
                FontWeight? weight = null;

                if (i+1<raw.Length && raw[i]=='*' && raw[i+1]=='_')
                {
                    i++;
                    test+=raw[i];
                }

                if (test.Contains("_*") && test.Contains("*_") && test.IndexOf("_*")<test.IndexOf("*_"))
                {
                    s1s = test.IndexOf("_*");
                    s1e = s1s+1;
                    s2s = test.IndexOf("*_");
                    s2e = s2s+1;
                    style = FontStyles.Italic;
                    weight = FontWeights.Bold;
                }
                else if (test.Contains("*") && test.IndexOf("*") != test.LastIndexOf("*"))
                {
                    s1s = s1e = test.IndexOf("*");
                    s2s = s2e = test.LastIndexOf("*");
                    weight = FontWeights.Bold;
                }
                else if (test.Contains("_") && test.IndexOf("_") != test.LastIndexOf("_"))
                {
                    s1s = s1e = test.IndexOf("_");
                    s2s = s2e = test.LastIndexOf("_");
                    style = FontStyles.Italic;
                }

                if (s1s != -1)
                {
                    var prefix = raw.Substring(0, s1s);
                    var middle = raw.Substring(s1e+1, s2s-s1e-1);
                    var suffix = raw.Substring(s2e+1);
                    yield return new Run(prefix);

                    var middleRun = new Run(middle);
                    if (style.HasValue)
                        middleRun.FontStyle = style.Value;
                    if (weight.HasValue)
                        middleRun.FontWeight = weight.Value;
                    yield return middleRun;

                    if (String.IsNullOrEmpty(suffix))
                        yield break;

                    raw = suffix;
                    test = "";
                    i = -1;
                }
                i++;
            }
            yield return new Run(test);
        }

        private String _rawText;
    }
}