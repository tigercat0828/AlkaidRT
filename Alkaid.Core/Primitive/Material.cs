using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkaid.Core.Primitive {
    public class Material {
        public Color Albedo { get; set; }
        public float Ka;
        public float Kd;
        public float Ks;
        public float Shineness;
        public float Reflect;
        public Material() : this(Color.White, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f) {}

        public Material(Color albedo) : this(albedo, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f) {}

        public Material(Color albedo, float ka, float kd, float ks, float shineness, float reflexive) {
            Albedo = albedo;
            Ka = ka;
            Kd = kd;
            Ks = ks;
            Shineness = shineness;
            Reflect = reflexive;
        }

        public override string ToString() {
            return $"Albedo :{Albedo}, Ka : {Ka}, Kd : {Kd}, Ks : {Ks}, Sn : {Shineness}, Re :{Reflect}";
        }
    }
}
