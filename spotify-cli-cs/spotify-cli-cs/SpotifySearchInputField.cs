namespace spotify_cli_cs.Components
{
    class SpotifySearchInputField : TextInputField
    {
        public override void OnEnter()
        {
            System.Diagnostics.Debug.WriteLine("on enter but on the child class or smth idk");
        }
    }
}