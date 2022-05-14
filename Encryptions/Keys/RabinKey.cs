using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static TheoryOfInformation.lab3.Service.CustomMath;

namespace TheoryOfInformation.lab3.Encryptions.Keys
{
    public class RabinKey : IKey
    {
        public uint q { get; }
        public uint p { get; }
        public uint b { get; }
        public uint n { get; }
        private byte _resize = 0;

        public byte resize => _resize;

        public RabinKey(uint p, uint q, uint b)
        {
            if (!IsPrime(q)) throw new Exception($"{q} не являеться простым");
            else if (!IsPrime(p)) throw new Exception($"{p} не являеться простым");
            else if(p % 4 != 3 || q % 4 != 3) throw new Exception($"должно выполняться условие: p ≡ q ≡ 3 mod 4");
            else if(b>=p*q) throw new Exception($"b не может быть больше чем q*p");

            this.q = q;
            this.p = p;
            this.b = b;
            this.n = q*p;

            long tmp = n;
            while (tmp > 0)
            {
                tmp >>= 8;
                _resize++;
            }
        }

        public List<uint> Dencrypte(List<uint> file)
        {
            List<uint> result = new List<uint>(file.Count);
                foreach (var symb in file)
                {
                    uint D = (b * b + 4 * symb) % n;

                    uint mp = FastPowerModul(D, (p + 1) / 4, p);
                    uint mq = FastPowerModul(D, (q + 1) / 4, q);


                    var muls = gcd_ext((int)p, (int)q);

                    long d1 = mod(muls.Item1 * p * mq + muls.Item2 * q * mp, n);
                    long d2 = n - d1;
                    long d3 = mod(muls.Item1 * p * mq - muls.Item2 * q * mp, n);
                    long d4 = n - d3;
                    List<long> ds = new List<long>() { d1, d2, d3, d4 };

                    foreach (var di in ds)
                    {
                        long mi = 0;
                        if ((di - b) % 2 == 0)
                            mi = mod((di - b) / 2, n);
                        else
                            mi = mod((n + di - b) / 2, n);

                        if (mi < 256 && mi > -1)
                        {
                            result.Add((uint)mi);
                            break;
                        }
                    }
                }
                return result;
        }

        public List<uint> Encrypte(List<uint> file)
        {
            List<uint> result = new List<uint>(file.Count);
            foreach(var symb in file)
            {
                uint tmp = (symb % n * (symb + b) % n) % n;
                result.Add(tmp);
            }
            return result;
        }
    }
}
