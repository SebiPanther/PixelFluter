using System;
using System.Collections.Generic;
using PixelFluter.Library;

internal class CommandRepository
{
    internal List<string> Convert(int offsetX, int offsetY, List<Pixel> pixels)
    {
        var result = new List<string>();
        foreach(var pixel in pixels)
        {
            result.Add($"PX {offsetX + pixel.X} {offsetY + pixel.Y} {pixel.R:X2}{pixel.G:X2}{pixel.B:X2}{pixel.A:X2}");
        }
        return result;
    }
}