using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
    public interface IObjective
    {
        int Score(Roll roll);
    }
}
