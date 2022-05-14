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

		public static long mod(long x, long m)
		{
			return (x % m + m) % m;
		}

		public static uint FastPowerModul(uint Number, uint Pow, uint Mod)
		{
			long Result = 1;
			long Bit = Number % Mod;

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

		public static (int, int) gcd_ext(int a, int b)
		{
			int d0 = a; int d1 = b;
			int x0 = 1; int x1 = 0;
			int y0 = 0; int y1 = 1;
            while (d1 > 1)
            {
				int q = d0 / d1;
				int d2 = d0 % d1;
				int x2 = x0 - q * x1;
				int y2 = y0 - q * y1;

				d0 = d1; d1 = d2;
				x0 = x1; x1 = x2;
				y0 = y1; y1 = y2;
            }
			return (x1, y1);
		}
	}
}