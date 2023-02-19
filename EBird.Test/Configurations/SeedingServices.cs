using Newtonsoft.Json;

namespace EBird.Test.Configurations;

public static class SeedingServices
{
    private static string pathFile = "../../../MockData/FileData/";

    public static dynamic LoadJson(string f)
    {
        using (StreamReader r = new StreamReader(pathFile + f))
        {
            string json = r.ReadToEnd();
            dynamic array = JsonConvert.DeserializeObject(json);
            return array;
        }
    }

    public static dynamic LoadFileToString(string f)
    {
        using (StreamReader r = new StreamReader(pathFile + f))
        {
            string contents = r.ReadToEnd();
            return contents;
        }
    }
}
