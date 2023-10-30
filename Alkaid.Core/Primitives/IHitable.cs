﻿using Alkaid.Core.Data;

namespace Alkaid.Core.Primitives;
public interface IHitable {
    public int ID { get; }
    public PhongMat Material { get; set; }
    public bool Hit(Ray ray);

    // may lead to bug
    public bool Hit(Ray ray, Interval interval, ref HitRecord record);
}