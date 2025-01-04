using System.Text;

namespace Otus_HW4_Reflection
{
    internal static class MySerializer
    {

        public static string SerializePublicFields<T>(T obj) where T : class
        {
            StringBuilder result = new StringBuilder();
            result.Append('{');

            var type = obj.GetType();
            var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var values = fields.Select(f => $"\"{f.Name}\":{f.GetValue(obj)}");
            result.Append(String.Join(",", values));

            result.Append("}");
            return result.ToString();
        }


        public static T Deserialize<T>(string json) where T : new()
        {
           
            var resultObj = new T();    

            var fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            Dictionary<string, int> dictionary = json.Trim(['{', '}']).Split(',').Select(p => p.Split(':')).ToDictionary(k => k[0].Trim('\"'), v => int.Parse(v[1]));

            foreach (var field in fields)
            {
                field.SetValue(resultObj, dictionary.GetValueOrDefault(field.Name));
            }

            return resultObj;
        }
    }
}