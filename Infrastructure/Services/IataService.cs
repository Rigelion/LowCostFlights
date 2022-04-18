using Infrastructure.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class IataService : IIataService
{
    public IataService()
    {
        string folder = Path.GetDirectoryName(Environment.ProcessPath) + @"\Data\";
        var airportsJsonPath = Directory.GetFiles(folder, "airports.json").First();

        using StreamReader sr = new StreamReader(airportsJsonPath);
        string json = sr.ReadToEnd();
        Iatas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IataModel>>(json)!;
    }

    public List<IataModel> Iatas { get; set; } = null!;

    public IataModel? GetIata(string iataCode) => Iatas.FirstOrDefault(x => x.Iata == iataCode);

    public IEnumerable<IataModel> GetIatas() => Iatas;
}

