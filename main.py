from auth import auth
import spotify
import curses
from curses import wrapper
import time

sp = auth()
#spotify.main(sp)

# display config
D_MARGIN = 2

def main():
    # init
    curses.noecho()
    curses.cbreak()
    stdscr.keypad(True)
    curses.curs_set(0)

    # main
    h, w = stdscr.getmaxyx()
    CURRENTLY_PLAYLING =  f"{spotify.getCurrentSongData(sp)['name']} - {spotify.getCurrentSongData(sp)['main_artist']}"
    stdscr.addstr(h // 2 + (h // 2) - D_MARGIN, w // 2 - len(CURRENTLY_PLAYLING) // 2, CURRENTLY_PLAYLING)
    stdscr.refresh()

    time.sleep(3)

    # clean up
    stdscr.clear()
    curses.nocbreak()
    stdscr.keypad(False)
    curses.echo()

    curses.endwin()

stdscr = curses.initscr()
main()
