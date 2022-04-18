using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IIataService
    {
        IEnumerable<IataModel> GetIatas();

        IataModel? GetIata(string iataCode);
    }
}
