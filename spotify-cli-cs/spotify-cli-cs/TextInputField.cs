namespace spotify_cli_cs.Components
{
    public class TextInputField : TUIBaseComponent
    {
        public TextInputField(int x = 0, int y = 0) : base(x, y) { }

        private string? content; // content of the input field of this instance
        public override void HandleKeyInput(ConsoleKey key)
        {
            System.Diagnostics.Debug.WriteLine("incoming key: " + key.ToString());   

            this.UpdateLabel();
        }

        public void UpdateLabel()
        {
        }

        public override void OnBlur()
        {
            throw new NotImplementedException();
        }

        public override void OnFocus()
        {
            throw new NotImplementedException();
        }
    }
}