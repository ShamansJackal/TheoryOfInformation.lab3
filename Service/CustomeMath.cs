using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheoryOfInformation.lab1.Service
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

		public static uint FastPower(uint @base, uint exponent)
		{
			if (@base != 0) // Сравнение вещественного числа с 0 не вполне корректно, но взято для простоты
			{
				uint power = 1;
				while (exponent > 0)                    // Пока текущее значение показателя степени не равно 0:
				{
					if ((exponent & 1) != 0)            // Если оно нечётно,
						power *= @base;                 // результат умножается на текущее значение основания.
					exponent >>= 1;                     // Показатель степени делится на 2.
					@base *= @base;                     // Основание возводится в квадрат.
				}
				return power;
            }
            else
            {
				return 0;
            }
		}

		public static uint gcd(uint a, uint b, ref int x,ref int y)
		{
			if (a == 0)
			{
				x = 0; y = 1;
				return b;
			}
			int x1 = 0, y1 = 0;
			uint d = gcd(b % a, a, ref x1, ref y1);
			x = (int)(y1 - (b / a) * x1);
			y = x1;
			return d;
		}
	}
}
