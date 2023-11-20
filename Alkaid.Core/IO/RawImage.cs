using Alkaid.Core.Data;

namespace Alkaid.Core.IO;
public class RawImage {
    const int B = 0, G = 8, R = 16, A = 24;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Length => Width * Height;
    public uint[] Pixels;
    public uint this[int i] {
        get { return Pixels[i]; }
        set { Pixels[i] = value; }
    }
    public uint this[int i, int j] {
        get { return Pixels[j * Width + i]; }
        set { Pixels[j * Width + i] = value; }
    }

    public RawImage(RawImage other) {
        Pixels = other.Pixels.ToArray();
        Width = other.Width;
        Height = other.Height;
    }
    public RawImage(int width, int height) {
        Width = width;
        Height = height;
        Pixels = new uint[width * height];
    }
    public RawImage(int width, int height, uint[] pixels) {
        Width = width;
        Height = height;
        Pixels = pixels.ToArray();
    }
    public void SetPixel(int x, int y, uint pixel) {
        int index = y * Width + x;
        Pixels[index] = pixel;
    }

    public void SetPixel(int x, int y, Color color) {
        uint pixelValue = (uint)color.R << R | (uint)color.G << G | (uint)color.B << B | 0xFF000000;
        SetPixel(x, y, pixelValue);
    }
    public void SaveFile(string filename) {
        FileIO.WritePPM(filename, Pixels, Width, Height);
    }
    public uint GetPixel(int x, int y) {
        return Pixels[y * Width + x];
    }

}