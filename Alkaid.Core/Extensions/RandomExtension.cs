﻿using System.Numerics;

namespace Alkaid.Core.Extensions;
public static class RandomExtension {

    public static Vector3 RandomVec3(this Random random) {
        return new Vector3(random.NextSingle(), random.NextSingle(), random.NextSingle());
    }
    /// <summary>
    /// return a random vector which x, y, z in [min, max]
    /// </summary>
    public static Vector3 RandomVec3(this Random random, float min, float max) {
        float x = random.NextSingle(min, max);
        float y = random.NextSingle(min, max);
        float z = random.NextSingle(min, max);
        return new Vector3(x, y, z);
    }
    public static float NextSingle(this Random random, float min, float max) {
        float t = random.NextSingle();
        return t * (max - min) + min;
    }
    /// <summary>
    /// return random direction vector which length is 1
    /// </summary>
    public static Vector3 UnitVector(this Random random) {
        while (true) {
            Vector3 tmp = new(random.NextSingle() * 2.0f - 1.0f, random.NextSingle() * 2.0f - 1.0f, random.NextSingle() * 2.0f - 1.0f);
            if (tmp.LengthSquared() < 1) {
                return Vector3.Normalize(tmp);
            }
        }
    }
    public static Vector3 InUnitSphere(this Random random) {
        while (true) {
            Vector3 tmp = new(random.NextSingle() * 2.0f - 1.0f, random.NextSingle() * 2.0f - 1.0f, random.NextSingle() * 2.0f - 1.0f);
            if (tmp.LengthSquared() < 1) {
                return tmp;
            }
        }
    }
    public static Vector3 UnitDisk(this Random random) {
        while (true) {
            Vector3 p = new Vector3(random.NextSingle(-1, 1), random.NextSingle(-1, 1), 0);
            if (p.LengthSquared() < 1)
                return p;
        }
    }
    public static Vector3 InUnitHemisphere(this Random random, Vector3 normal) {
        Vector3 unit = random.UnitVector();
        if (Vector3.Dot(unit, normal) > 0.0)
            return unit;
        else
            return -unit;
    }
}
