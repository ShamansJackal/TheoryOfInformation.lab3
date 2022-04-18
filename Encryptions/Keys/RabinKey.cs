using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TheoryOfInformation.lab1.Service.CustomMath;

namespace TheoryOfInformation.lab1.Encryptions.Keys
{
    public class RabinKey : IKey
    {
        public uint q { get; }
        public uint p { get; }
        public long b { get; }
        public long n { get; }

        public RabinKey(uint p, uint q, long b)
        {
            if (!IsPrime(q)) throw new Exception($"{q} не являеться простым");
            else if (!IsPrime(p)) throw new Exception($"{p} не являеться простым");
            else if(b>=p*q) throw new Exception($"b не может быть больше чем q*p");
            else if(p % 4 != 3 || q % 4 != 3) throw new Exception($"должно выполняться условие: p ≡ q ≡ 3 mod 4");

            this.q = q;
            this.p = p;
            this.b = b;
            this.n = q*p;
        }

        public byte[] Dencrypte(byte[] file)
        {
            long len = n;
            byte mul = 0;
            while (len > 0)
            {
                len >>= 8;
                mul++;
            }

            int yp = 0, yq = yp;
            var s = gcd(p, q, ref yp, ref yq);

            List<byte> buffer = new List<byte>();
            for(uint i=0; i<file.Length; i+=mul)
            {
                buffer.Add(_decp(file.Skip((int)i).Take(mul), yp, yq));
            }

            return buffer.ToArray();
        }

        public IEnumerable<long> Encrypte(byte[] file, out byte resize)
        {
            long len = n;
            byte mul = 0;

            while (len > 0)
            {
                len >>= 8;
                mul++;
            }

            List<long> buffer = new List<long>();
            foreach(byte elm in file)
                buffer.Add(elm * (elm + b) % n);
            resize = mul;

            return buffer;
        }

        public byte _decp(IEnumerable<byte> eml, int yp, int yq)
        {
            byte sss = 0;
            long tmp = 0;
            foreach(long b in eml)
                tmp += b << (8 * sss);

            uint D = (uint)((b * b + 4 * tmp) % n);

            uint Mp = FastPower(D, (p + 1) / 4) % p;
            uint Mq = FastPower(D, (q + 1) / 4) % q;

            long d1 = (yp * p * Mq + yq * q * Mp) % n;
            long d2 = n - d1;
            long d3 = (yp * p * Mq - yq * q * Mp) % n;
            long d4 = n - d3;
            return 0;
        }
    }
}
