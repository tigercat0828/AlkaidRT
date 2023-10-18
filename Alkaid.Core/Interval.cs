using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Alkaid.Core; 
public struct Interval {
    public float min;
    public float max;
    public Interval(float min, float max) {
        this.min = min;
        this.max = max;
    }
    public Interval() : this(0,0){}
    public bool Surrounds(float x) => x > min && x < max;
    public bool Contains(float x) => x >= min && x <= max;
}
