from auth import authenticateUser
import spotify
import curses
from curses import wrapper
import time
from colorama import Fore, Style

sp = authenticateUser()
#spotify.main(sp)

# display config
D_MARGIN = 2
D_HEIGHT: int = -1
D_WIDTH: int = -1

CHAR_PAUSE = "❚❚"
CHAR_PLAY = "▶"

def drawMapLabels():
    h, w = stdscr.getmaxyx()

    for i in range(h):
        stdscr.addstr(i, 0, str(i))

    for i in range(w):
        if (i % 5 == 0):
            stdscr.addstr(0, i, str(i))

def main():
    # init
    curses.noecho()
    curses.cbreak()
    stdscr.keypad(True)
    curses.curs_set(0)

    # main
    D_HEIGHT, D_WIDTH = stdscr.getmaxyx()
    CURRENTLY_PLAYLING =  f"{spotify.getCurrentSongData(sp)['name']} - {spotify.getCurrentSongData(sp)['main_artist']}"
    stdscr.addstr(D_HEIGHT - D_MARGIN - 1, D_WIDTH // 2 - len(CURRENTLY_PLAYLING) // 2, CURRENTLY_PLAYLING)
    stdscr.addstr(D_HEIGHT - D_MARGIN, D_WIDTH // 2 - D_MARGIN // 2, CHAR_PAUSE if spotify.getCurrentSongData(sp)['is_playing'] else CHAR_PLAY)

    sp.pause_playback()
    drawMapLabels()

    spotify.drawCurrentSongCover(sp, stdscr)

    stdscr.refresh()

    time.sleep(3)

    # clean up
    stdscr.clear()
    curses.nocbreak()
    stdscr.keypad(False)
    curses.echo()

    curses.endwin()

"""# initialize curses
stdscr = curses.initscr()

# initialize colors
curses.start_color()
curses.use_default_colors()

for i in range(0, curses.COLORS):
        curses.init_pair(i + 1, i, -1)
# initialization complete
main()
"""