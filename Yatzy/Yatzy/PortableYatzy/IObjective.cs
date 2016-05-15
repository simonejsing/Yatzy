using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
    public interface IObjective
    {
        string Identifier { get; }

        int Score(Roll roll);
    }
}
