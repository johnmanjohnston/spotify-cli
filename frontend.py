import textual.widgets as w
from textual.app import App, ComposeResult

import modification

# Custom widgets
class CurrentlyPlaying(w.Static):
    def compose(self) -> ComposeResult:
        yield w.Label("Currently playing is not set", id="currently_playing")

# Main app
class SpotifyCLI(App):
    currentlyPlaying = None

    def on_mount(self) -> None:
        self.currentlyPlaying = self.query_one("#currently_playing", w.Label)

    def compose(self) -> ComposeResult:
        yield CurrentlyPlaying()

    def on_key(self, key):
        val = key.key
        
        if val == "space":
            modification.togglePlayPause()

def init():
    app = SpotifyCLI()
    app.run()