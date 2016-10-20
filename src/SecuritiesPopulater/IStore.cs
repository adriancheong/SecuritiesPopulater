using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuritiesPopulater
{
    interface IStore
    {
        void Populate(int numberOfSecurities, int startingSecurityId);
    }
}
