using System.Numerics;

namespace Alkaid.Core.Data;

public struct Color {
    public float R;
    public float G;
    public float B;
    public Color() {
        R = G = B = 0;
    }
    public Color(float r, float g, float b) {
        R = r;
        G = g;
        B = b;
    }
    public Color(Vector3 vec) {
        R = vec.X;
        G = vec.Y;
        B = vec.Z;
    }
    public Color Clamp() {
        if (R > 255) R = 255;
        if (G > 255) G = 255;
        if (B > 255) B = 255;
        return this;
    }
    public static Color operator +(Color lhs, Color rhs) {
        return new Color(lhs.R + rhs.R, lhs.G + rhs.G, lhs.B + rhs.B);
    }
    public static Color operator *(Color lhs, Color rhs) {
        return new Color(lhs.R * rhs.R, lhs.G * rhs.G, lhs.B * rhs.B);
    }
    public static Color operator *(float scalar, Color color) {
        return new Color(scalar * color.R, scalar * color.G, scalar * color.B);
    }
    public static Color operator *(Color color, float scalar) {
        return new Color(scalar * color.R, scalar * color.G, scalar * color.B);
    }
    public static Color operator /(Color color, float scalar) {
        return new Color(color.R/scalar, color.G/scalar, color.B/scalar);
    }


    public static explicit operator Color(Vector3 v) {
        return new Color(v.X, v.Y, v.Z);
    }
    public override string ToString() {
        return $"<{R,2}, {G,2}, {B,2}>";
    }
    public static readonly Color Black = new(0, 0, 0);
    public static readonly Color White = new(1, 1, 1);
    public static readonly Color Red = new(1, 0, 0);
    public static readonly Color Green = new(0, 1, 0);
    public static readonly Color Blue = new(0, 0, 1);
    public static readonly Color Yellow = new(1, 1, 0);
    public static readonly Color Magenta = new(1, 0, 1);
    public static readonly Color Cyan = new(0, 1, 1);
    public static readonly Color Gray = new(0.5f, 0.5f, 0.5f);
    public static readonly Color None = new(0, 0, 0);
}
