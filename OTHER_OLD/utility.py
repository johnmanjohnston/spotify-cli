import pyautogui as pag

def alt_tab():
    """
    `driver.minimize_window()` doesn't let audio play for some reason.
    As an alternative, we just Alt+Tab out of the Chrome instance
    """
    pag.keyDown("alt")
    pag.keyDown("tab")
    pag.keyUp("alt")
    pag.keyUp("tab")

def log(s):
    with open("log.txt", "a") as f:
        f.write(s)
    f.close()