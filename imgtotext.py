import PIL.Image
import numpy as np

CHAR = "██"

img = PIL.Image.open("cover.png")
img.load()
imgData = np.asarray(img, dtype="int32")

RENDER_DIMENSIONS = 16
IMAGE_DIMENSIONS = img.width # height and width are same

def getCol(r,g,b):
    if 0 <= r <= 255 and 0 <= g <= 255 and 0 <= b <= 255:
        color_code = f'\33[38;2;{r};{g};{b}m'
        return color_code
    else:
        return '\33[97m'

def convert():
    RETVAL = ""
    for x in range(RENDER_DIMENSIONS):
        for y in range(RENDER_DIMENSIONS):
            color1 = imgData[x * (IMAGE_DIMENSIONS // RENDER_DIMENSIONS)][y * (IMAGE_DIMENSIONS // RENDER_DIMENSIONS)] # fancy m̶e̶t̶h̶ math

            r = (color1[0])
            g = (color1[1])
            b = (color1[2])

            RETVAL += (getCol(r,g,b) + CHAR)
        
        
        RETVAL += "\n"
    return RETVAL

if __name__ == "__main__":
    print(convert())