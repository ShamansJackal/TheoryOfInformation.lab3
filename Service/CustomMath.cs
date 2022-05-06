using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheoryOfInformation.lab3.Service
{
	public static class CustomMath
	{
		public static bool IsPrime(uint number)
		{
			if (number < 2) return false;
			if (number % 2 == 0) return (number == 2);
			uint root = (uint)Math.Sqrt((double)number);
			for (int i = 3; i <= root; i += 2)
			{
				if (number % i == 0) return false;
			}
			return true;
		}

		public static int mod(int x, int m)
		{
			return (x % m + m) % m;
		}

		public static long mod(long x, long m)
		{
			return (x % m + m) % m;
		}

		public static HashSet<uint> GetMuls(uint x)
		{
			HashSet<uint> set = new HashSet<uint>();
			for (uint i = 2; i < x;)
			{
				if (x % i == 0)
				{
					set.Add(i);
					x /= i;
				}
				else
				{
					i++;
				}
			}
			set.Add(x);
			return set;
		}

		public static ulong Eyler(ulong n)
		{
			ulong res = n, en = Convert.ToUInt64(Math.Sqrt(n) + 1);
			for (ulong i = 2; i <= en; i++)
				if ((n % i) == 0)
				{
					while ((n % i) == 0)
						n /= i;
					res -= (res / i);
				}
			if (n > 1) res -= (res / n);
			return res;
		}

		public static List<uint> GetRoots(uint p)
		{
			uint euler = (uint)Eyler(p);

			var muls = GetMuls(euler);
			List<uint> roots = new List<uint>((int)(euler / 2));

			for (uint g = 1; g < p; g++)
			{
				if (FastPower(g, euler, p) != 1) continue;
				//if (gcd(p, g, ref dump1, ref dump2) != 1) continue;

				//if (p > 2 && (FastPower(g, euler / 2, p) == 1))
				//	continue;
				bool breakMain = false;


				foreach (var mul in muls)
				{
					if (FastPower(g, euler / mul, p) == 1)
					{
						breakMain = true;
						break;
					}
				}

				if (!breakMain)
					roots.Add(g);
			}
			return roots;
		}

		public static uint FastPowerModul(uint Number, uint Pow, uint Mod)
		{
			Int64 Result = 1;
			Int64 Bit = Number % Mod;

			while (Pow > 0)
			{
				if ((Pow & 1) == 1)
				{
					Result *= Bit;
					Result %= Mod;
				}
				Bit *= Bit;
				Bit %= Mod;
				Pow >>= 1;
			}
			return (uint)Result;
		}

		public static uint FastPower(uint a, uint n, uint m)
		{
			if (n == 0)
				return 1 % m;
			if (n % 2 == 1)
				return (FastPower(a, n - 1, m) * a) % m;
			else
				return FastPower((a * a) % m, n / 2, m);
		}

		public static uint gcd_ext(uint a, uint b, ref int x, ref int y)
		{
			if (a == 0)
			{
				x = 0; y = 1;
				return b;
			}
			int x1 = 0, y1 = 0;
			uint d = gcd_ext(b % a, a, ref x1, ref y1);
			x = (int)(y1 - (b / a) * x1);
			y = x1;
			return d;
		}

		public static uint gcd(uint a, uint b)
		{
			while (a != 0 && b != 0)
				if (a > b)
					a %= b;
				else
					b %= a;
			return a | b;
		}
	}
}