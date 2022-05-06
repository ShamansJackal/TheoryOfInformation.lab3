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
	}
}