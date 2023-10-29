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

        public Pos xPos;
        public Pos yPos;

        public CustomProgressBar()
        {
            this.xPos = Pos.Percent(4);
            this.yPos = Pos.Percent(90);

            // instansiate labels
            unfilledLabel = new Label()
            {
                X = this.xPos,
                Y = this.yPos,
                Text = new String(EMPTY, (int)(barWidth * Fraction)),
            };
            filledLabel = new Label() {
                X = this.xPos,
                Y = this.yPos,
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
            int fillWidth = (int)(barWidth * this.Fraction);

            filledLabel.Text = new String(FILLED, fillWidth);
            unfilledLabel.Text = new String(EMPTY, (int)barWidth);

            filledLabel.Y = unfilledLabel.Y;
            filledLabel.Width = fillWidth;
            unfilledLabel.Width = (int)barWidth;
        }
    }  
}