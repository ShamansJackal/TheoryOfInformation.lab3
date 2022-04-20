using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheoryOfInformation.lab3.Encryptions.Keys
{
    public interface IKey
    {
        List<uint> Encrypte(List<uint> file);
        List<uint> Dencrypte(List<uint> file);
    }
}
