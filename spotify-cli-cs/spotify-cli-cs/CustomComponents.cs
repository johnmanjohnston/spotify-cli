using System.Xml.Schema;

namespace Terminal.Gui
{
    class CustomProgressBar : View
    {
        public float Fraction { get; set; }
        public uint barWidth;

        private char FILLED = '━';
        private char EMPTY = '━';

        private Label filledLabel;
        private Label unfilledLabel;

        public CustomProgressBar()
        {
            // instansiate labels
            unfilledLabel = new Label()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Text = new String(EMPTY, (int)(barWidth * Fraction)),
            };
            filledLabel = new Label() {
                X = Pos.Center(),
                Y = Pos.Center()+1,
                Text = new String(FILLED, (int)(barWidth * this.Fraction)),
                TextAlignment = TextAlignment.Left,
            };


            // color labels
            filledLabel.ColorScheme = new()
            {
                Normal = Attribute.Make(Color.Blue, Color.Black),
            };

            unfilledLabel.ColorScheme = new()
            {
                Normal = Attribute.Make(Color.Green, Color.Black),
            };
        }

        public void AddToWindow(Window w)
        {
            w.Add(this.unfilledLabel);
            w.Add(this.filledLabel);
        }
        public void DisplayProgress()
        {
            try
            {
                // TODO
                int fillWidth = (int)(barWidth * this.Fraction);

                filledLabel.Text = new String(FILLED, fillWidth);
                unfilledLabel.Text = new String(EMPTY, (int)barWidth);

                filledLabel.Y = unfilledLabel.Y;
                filledLabel.Width = fillWidth;
                unfilledLabel.Width = (int)barWidth;

                filledLabel.X = Pos.Center() - (int)(barWidth / 2);
            } catch (Exception ex) { }
        }
    }  
}