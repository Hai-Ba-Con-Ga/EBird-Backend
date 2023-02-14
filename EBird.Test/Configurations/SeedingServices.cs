using Newtonsoft.Json;

namespace EBird.Test.Configurations;

public static class SeedingServices
{
    public static dynamic LoadJson(string f)
    {
        using (StreamReader r = new StreamReader("../../../MockData/" + f))
        {
            string json = r.ReadToEnd();
            dynamic array = JsonConvert.DeserializeObject(json);
            return array;
        }
    }

    public static dynamic LoadFileToString(string f)
    {
        using (StreamReader r = new StreamReader("../../../MockData/" + f))
        {
            string contents = r.ReadToEnd();
            return contents;
        }
    }
}
