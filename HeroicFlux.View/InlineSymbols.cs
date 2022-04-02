using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroicFlux.View
{
    public class InlineSymbol: ICloneable
    {
        static InlineSymbol()
        {
            InitializeMap();
        }

        public Image Image
        {
            get
            {
                var containter = Inline as InlineUIContainer;
                if (containter == null)
                    return null;
                var image = containter.Child as Image;
                return image;
            }
        }

        public Inline Inline { get; private set; }

        public Boolean IsImage { get { return Image != null; } }

        public Boolean IsSymbol { get { return SymbolProperties != null; } }

        public Uri Uri { get; private set; }

        private static InlineSymbol NonBreakingNull
        {
            get
            {
                return CreateFromCharacter(new RunProperties('\u2060'.ToString(), new FontFamily()), 0);
            }
        }

        private RunProperties SymbolProperties { get; set; }

        public static InlineSymbol CreateFromCharacter(RunProperties symbolProperties, Double sizeOffset)
        {
            var symbol = new InlineSymbol();
            symbol.SizeOffset = sizeOffset;
            symbol.SymbolProperties = symbolProperties;
            symbol.Inline = symbolProperties.CreateRun();

            return symbol;
        }

        public static InlineSymbol CreateFromIconFile(String fileName, Double sizeOffset)
        {
            if (!fileName.EndsWith(".png"))
                fileName = fileName+".png";
            return CreateFromImage(new Uri("pack://application:,,,/Icons/"+fileName), sizeOffset);
        }

        public static InlineSymbol CreateFromImage(Uri uri, Double sizeOffset)
        {
            var symbol = new InlineSymbol();

            symbol.SizeOffset = sizeOffset;
            symbol.Uri = uri;

            var image = GetImage(uri);
            InlineUIContainer inlineImage = new InlineUIContainer(image);
            symbol.Inline = inlineImage;

            return symbol;
        }

        public static IEnumerable<InlineSymbol> GetSymbolStreak(String key, Double fontSize)
        {
            if (Map.ContainsKey(key))
            {         
                foreach (var inline in Map[key])
                {
                    var copy = inline.Copy();
                    copy.SetFontSize(fontSize);
                    yield return copy;
                }
            }
        }

        public static Boolean IsTailMapped(String text, out String key)
        {
            key = "";
            for (var n =Math.Min(text.Length, MaxKeyLength); n>0; n--)
            {
                var tail = text.Substring(text.Length-n);
                if (Map.ContainsKey(tail))
                {
                    key = tail;
                    return true;
                }
            }
            return false;
        }

        public object Clone()
        {
            return Copy();
        }

        public InlineSymbol Copy()
        {
            if (IsImage)
                return CreateFromImage(Uri, SizeOffset);

            if (IsSymbol)
                return CreateFromCharacter(SymbolProperties.Copy(), SizeOffset);

            return null;
        }

        public void SetFontSize(Double fontSize)
        {
            fontSize+=SizeOffset;

            if (IsImage)
            {
                var bitmap = Image.Source as BitmapImage;
                if (bitmap == null)
                    return;

                var proportions = bitmap.Width/bitmap.Height;
                Image.BeginInit();
                Image.Height = fontSize;
                Image.Width = fontSize*proportions;
                Image.EndInit();
            }
            if (IsSymbol)
            {
                var run = Inline as Run;
                if (run != null)
                {
                    run.BeginInit();
                    run.FontSize = fontSize;
                    run.EndInit();
                }
            }
        }

        private static void Add(String key, params InlineSymbol[] symbols)
        {
            var list = new List<InlineSymbol>();

            for (var i = 0; i<symbols.Length; i++)
            {
                // if (i>0)                    list.Add(NonBreakingNull);
                list.Add(symbols[i]);
            }

            Map.Add(key, list);
        }

        private static Image GetImage(Uri uri)
        {
            Image image = new Image();
            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.EndInit();

            image.Source = bitmap;
            return image;
        }

        private static void InitializeMap()
        {
            if (Map !=null)
                return;
            var segoeUi = new FontFamily("Segoe UI");
            var segoeUiSymbol = new FontFamily("Segoe UI Symbol");
            var wingdings = new FontFamily("Wingdings");
            var wingdings2 = new FontFamily("Wingdings 2");
            var ocrAExtended= new FontFamily("OCR A Extended");
            var variShapesEmpty = new FontFamily("VariShapes");
            var variShapesSolid = new FontFamily("VariShapes Solid");
            var d4EDings = new FontFamily("dPoly 4EDings");
            var mixedSilhouettes4 = new FontFamily("Mixed Silhouettes Free vol 4");
            var sosa = new FontFamily("Sosa");
            var descent = new FontFamily("DescentSymbols");

            var purpleFrame = new ImageBrush() { ImageSource =new BitmapImage(new Uri("pack://application:,,,/Icons/FramePurple.png")) };

            Map = new Dictionary<String, List<InlineSymbol>>();
            if (true)
            {            
                /* Colored */
                Add("\n\n", CreateFromCharacter(new RunProperties("\n"+'\u00A0'+"\n", segoeUi), -4));
                Add(">>", CreateFromCharacter(new RunProperties("", wingdings), 0));
                Add("<P>", CreateFromIconFile("EssencePhysical", 0));
                Add("<C>", CreateFromIconFile("EssenceCold", 0));
                Add("<F>", CreateFromIconFile("EssenceFire", 0));
                Add("<L>", CreateFromIconFile("EssenceLightning", 0));
                Add("<?>", CreateFromIconFile("EssenceUndecided", 0));
                Add("[o°]", CreateFromCharacter(new RunProperties("X", sosa), 2));
                Add("[flip]", CreateFromCharacter(new RunProperties("9", variShapesSolid), 0));
                Add("[d-]", CreateFromCharacter(new RunProperties("N", variShapesSolid) { Fore=Brushes.Red }, -2));
                Add("[d6]", CreateFromCharacter(new RunProperties("N", variShapesSolid) { Fore=Brushes.Black }, -2));
                Add("[d+]", CreateFromCharacter(new RunProperties("N", variShapesEmpty) { Fore=Brushes.Black }, -2));
                Add("[1]", CreateFromCharacter(new RunProperties("!", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[2]", CreateFromCharacter(new RunProperties("@", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[3]", CreateFromCharacter(new RunProperties("#", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[4]", CreateFromCharacter(new RunProperties("$", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[5]", CreateFromCharacter(new RunProperties("%", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[6]", CreateFromCharacter(new RunProperties("^", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[energy]", CreateFromCharacter(new RunProperties("", wingdings) { Fore=Brushes.Blue }, 0));
                Add("[damage]", CreateFromCharacter(new RunProperties("❤", segoeUiSymbol) { Fore= Brushes.Red }, 0));
                Add("[life]", CreateFromCharacter(new RunProperties("❤", segoeUiSymbol) { Fore= Brushes.Red }, 0));
                Add("[weapon]", CreateFromIconFile("Sword", 0));
                Add("[defensive]", CreateFromIconFile("Shield", 0));
                Add("[jewel]", CreateFromIconFile("Jewel", 0));
                Add("[standard]", CreateFromCharacter(new RunProperties("", descent) { Fore=Brushes.White, Back=Brushes.Black }, 0));
                Add("[move]", CreateFromCharacter(new RunProperties("", descent) { Fore=Brushes.White, Back=Brushes.Black }, 0));                
                Add("[quick]", CreateFromCharacter(new RunProperties("", descent) { Fore=Brushes.White, Back=Brushes.Black }, 0));
                Add("[hand]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[2hands]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[melee]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[ranged]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[armor]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[jewelSlot]", CreateFromCharacter(new RunProperties("", descent), 0));
            }
            else
            {
                /* B&W (where possible)*/
                Add("\n\n", CreateFromCharacter(new RunProperties("\n"+'\u00A0'+"\n", segoeUi), -4));
                Add(">>", CreateFromCharacter(new RunProperties("", wingdings), 0));
                Add("<P>", CreateFromIconFile("EssencePhysical", 0));
                Add("<C>", CreateFromIconFile("EssenceCold", 0));
                Add("<F>", CreateFromIconFile("EssenceFire", 0));
                Add("<L>", CreateFromIconFile("EssenceLightning", 0));
                Add("<?>", CreateFromIconFile("EssenceUndecided", 0));
                Add("[o°]", CreateFromCharacter(new RunProperties("X", sosa), 2));
                Add("[flip]", CreateFromCharacter(new RunProperties("9", variShapesSolid), 0));
                Add("[d-]", CreateFromCharacter(new RunProperties("N", variShapesSolid) { Fore=Brushes.Red }, -2));
                Add("[d6]", CreateFromCharacter(new RunProperties("N", variShapesSolid) { Fore=Brushes.Black }, -2));
                Add("[d+]", CreateFromCharacter(new RunProperties("N", variShapesEmpty) { Fore=Brushes.Black }, -2));
                Add("[1]", CreateFromCharacter(new RunProperties("!", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[2]", CreateFromCharacter(new RunProperties("@", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[3]", CreateFromCharacter(new RunProperties("#", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[4]", CreateFromCharacter(new RunProperties("$", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[5]", CreateFromCharacter(new RunProperties("%", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[6]", CreateFromCharacter(new RunProperties("^", d4EDings) { Fore=Brushes.Black }, 0));
                Add("[energy]", CreateFromCharacter(new RunProperties("", wingdings), 0));
                Add("[damage]", CreateFromCharacter(new RunProperties("❤", segoeUiSymbol), 0));
                Add("[life]", CreateFromCharacter(new RunProperties("❤", segoeUiSymbol), 0));
                Add("[weapon]", CreateFromIconFile("Sword", 0));
                Add("[defensive]", CreateFromIconFile("Shield", 0));
                Add("[jewel]", CreateFromIconFile("Jewel", 0));
                Add("[standard]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[move]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[quick]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[hand]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[2hands]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[melee]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[ranged]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[armor]", CreateFromCharacter(new RunProperties("", descent), 0));
                Add("[jewelSlot]", CreateFromCharacter(new RunProperties("", descent), 0));
            }

            MaxKeyLength = 0;
            foreach (var key in Map.Keys)
            {
                if (key.Length>MaxKeyLength)
                    MaxKeyLength = key.Length;
            }
        }

        public Double SizeOffset;
        private static Dictionary<String, List<InlineSymbol>> Map;
        private static int MaxKeyLength;

        public class RunProperties
        {
            public RunProperties()
            {
            }

            public RunProperties(String text, FontFamily family)
                : this()
            {
                Text = text;
                FontFamily = family;
            }

            public RunProperties Copy()
            {
                return new RunProperties
                {
                    Text = Text,
                    Fore = Fore,
                    Back = Back,
                    FontFamily = FontFamily
                };
            }

            public Run CreateRun()
            {
                Run run =new Run();
                run.Text = Text;

                run.FontFamily = FontFamily;                
                if (Fore != null)
                    run.Foreground = Fore;

                if (Back != null)
                    run.Background = Back;
                return run;
            }

            public Brush Back;
            public FontFamily FontFamily;
            public Brush Fore;
            public String Text;
        }
    }
}