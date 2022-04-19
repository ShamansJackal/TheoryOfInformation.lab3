using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheoryOfInformation.lab3.Encryptions.Keys
{
    public interface IKey
    {
        IEnumerable<long> Encrypte(byte[] file, out byte resize);
        byte[] Dencrypte(byte[] file);
    }
}
