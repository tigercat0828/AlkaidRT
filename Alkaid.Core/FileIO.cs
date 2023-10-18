public static class FileIO {
    public static void WritePPM(string filename, uint[] pixels, int width, int height) {

        using StreamWriter writer = new(filename);
        // Write the PPM header
        writer.WriteLine("P3");                 // P6 format for binary PPM
        writer.WriteLine($"{width} {height}");  // Width, height
        writer.WriteLine("255");                // Maximum color value

        for (int i = 0; i < pixels.Length; i++) {
            uint pixel = pixels[i];
            byte b = (byte)((pixel >> 0) & 0xFF);
            byte g = (byte)((pixel >> 8) & 0xFF);
            byte r = (byte)((pixel >> 16) & 0xFF);
            writer.WriteLine($"{r,3} {g,3} {b,3}");
        }
    }
}
