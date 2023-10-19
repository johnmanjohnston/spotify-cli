def log(*argv):
    with open("log.txt", "a") as f:
        for arg in argv:
            f.write(arg)
        f.write("\n")
    f.close()

from datetime import datetime
with open("log.txt", "a") as f:
    f.write(f"NEW LOG SESSION - {datetime.now()}")
    f.write("\n")
    f.close()