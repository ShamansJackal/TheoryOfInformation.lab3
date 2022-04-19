using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TheoryOfInformation.lab3.Service.CustomMath;

namespace TheoryOfInformation.lab3.Encryptions.Keys
{
    public class ElGamal : IKey
    {
        public uint p { get; }
        public uint x { get; }
        public uint k { get; }
        public uint y { get; private set; }
        public uint g { get; private set; }

        public IReadOnlyList<uint> roots { get; private set; }

        public ElGamal(uint p, uint x, uint k)
        {
            if (!IsPrime(p)) throw new Exception($"p:{p} не являеться простым");
            else if (x>=p-1 || x < 2) throw new Exception($"x:{x} не находиться в (1, p-1:{p-1})");
            else if (k>=p-1 || k < 2 || gcd(k, p-1)!=1) throw new Exception($"k:{k} не находиться в (1, p-1:{p-1}) или не являеться взаимно простым с p-1:{p-1}");

            this.p = p;
            this.x = x;
            this.k = k;

            roots = GetRoots(p);
            ChangeG(0);
        }

        public void ChangeG(int index)
        {
            index = index < roots.Count ? index : roots.Count;
            g = roots[index];
            y = FastPower(g, x, p);
        }
        public byte[] Dencrypte(byte[] file)
        {
            throw new NotImplementedException();
        }

        public byte[] Encrypte(byte[] file, out byte resize)
        {
            uint p_copy = p;
            resize = 0;
            while (p_copy > 0)
            {
                resize++;
                p_copy >>= 8;
            }

            uint a = FastPower(g, k, p);
            uint mul = FastPower(y, k, p);

            byte[] result = new byte[file.Length<<1];
            for(int i = 0; i < file.Length; i++)
            {
                uint b = (mul * file[i]) % p;
                result[i << 1] = (byte)a;
                result[(i << 1) + 1] = (byte)b;
            }
            return result;
        }
    }
}
