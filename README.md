# cripsy-and-unity
Small experiment on obtaining vintage and retro rendering in Unity. Also it's gonna be crispy!

# Base idea
## Pixelization
The most important thing is resolution. Screens we have today are simply too big to display small 320x200 games as Full HD. Getting small resolution on big screen can be problematic, but it is doable. In Unity there is a popular script that does that: PixelBoy. (https://gist.github.com/nothke/e68576aeca5ca6279343f8cd1e0d42ca)

Copying bigger texture onto smaller and smaller onto bigger does the job.

## Color Palettes
Our world is full of colors. Our screens can show more than 16 milion colors because we use 8 bits for each color (R, G, B). If we used less bits, we could get different palette of colors. To scale float representation of color in GLSL to some different palette I use:

```col.r = (float)((int)(col.r * _PaletteSize) % (_PaletteSize+1)) / (float)(_PaletteSize);```

For red... and for other colors too!

This is relatively simple solution, especially if we don't have predefined palette. I also multiply formula above by some float to achieve tint.



