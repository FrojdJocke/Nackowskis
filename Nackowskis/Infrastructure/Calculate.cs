using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nackowskis.Infrastructure
{
    public static class Calculate
    {
        /// <summary>
        /// Compares two values and returns the positive differance between the two
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2">test</param>
        /// <returns></returns>
        public static int Difference(int val1, int val2)
        {
            var res = val2 - val1;
            var result = res < 0 ? res * 1 : res;
            return result < 0 ? result * -1 : result;
        }
    }
}
