import os

def clear():
    """
    use 'clear" if you're using a REAL operating system
    like Linux. otherwise use 'cls'
    """
    os.system("cls") if os.name == "nt" else os.system("clear")