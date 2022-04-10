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
        string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName) + @"\Data\";
        var airportsJsonPath = Directory.GetFiles(folder, "airports.json").First();

        using StreamReader sr = new StreamReader(airportsJsonPath);
        string json = sr.ReadToEnd();
        Iatas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IataModel>>(json)!;
    }

    public List<IataModel> Iatas { get; set; } = null!;

    public IEnumerable<IataModel> GetIatas ()
    {
        return Iatas;
    }
}

