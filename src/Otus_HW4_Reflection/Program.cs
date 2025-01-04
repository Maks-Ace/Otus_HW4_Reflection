using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

namespace Otus_HW4_Reflection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // подготовка 
            int numberOfIterations = 100000;

            string mySerializedString = String.Empty;
            string serializedString = String.Empty;
            TimeSpan mySerializeElapsed;
            TimeSpan myDeSerializeElapsed;
            TimeSpan newtonSerializeElapsed;
            TimeSpan newtonDeSerializeElapsed;


            // выполнение
            // мой сериализатор
            mySerializeElapsed = TimeSerialization(MySerializer.SerializePublicFields, numberOfIterations, ref mySerializedString);
            myDeSerializeElapsed = TimeDeserialization(MySerializer.Deserialize<F>, numberOfIterations, mySerializedString);

            // newtonsoft
            newtonSerializeElapsed = TimeSerialization(JsonConvert.SerializeObject, numberOfIterations, ref serializedString);
            newtonDeSerializeElapsed = TimeDeserialization(JsonConvert.DeserializeObject<F>, numberOfIterations, serializedString);


            // вывод результатов
            Console.WriteLine($"Кол-во циклов: {numberOfIterations:N0}");
            Console.WriteLine($" Мой рефлекшн:");
            Console.WriteLine($"   Время на сериализацию: {mySerializeElapsed.TotalMilliseconds} ms");
            Console.WriteLine($"   Время на десериализацию: {myDeSerializeElapsed.TotalMilliseconds} ms");

            Console.WriteLine($" Стандартный механизм:");
            Console.WriteLine($"   Время на сериализацию: {newtonSerializeElapsed.TotalMilliseconds} ms");
            Console.WriteLine($"   Время на десериализацию: {newtonDeSerializeElapsed.TotalMilliseconds} ms");
            
        }


        /// <summary>
        /// Засекает время цикла сериализаций указанным методом
        /// </summary>
        /// <param name="serializationMethod">Метод для сериализации</param>
        /// <param name="numberOfIterations">Число циклов</param>
        /// <param name="serializedString">Сериализованная строка</param>
        /// <returns>Время  потраченное на все циклы</returns>
        private static TimeSpan TimeSerialization(Func<object, string> serializationMethod, int numberOfIterations, ref string serializedString)
        {
            F f = F.Get();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i <= numberOfIterations; i++)
            {
                serializedString = serializationMethod(f);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        /// <summary>
        /// Засекает время цикла десериализаций указанным методом
        /// </summary>
        /// <param name="numberOfIterations">Количество циклов</param>
        /// <param name="serializedString">Строка для десериализации</param>
        /// <returns>Время потраченное на все циклы</returns>
        private static TimeSpan TimeDeserialization(Func<string, F?> deserializationMethod, int numberOfIterations, string deserializableString)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //запустить в циклее выполнение 1000 десериалазаций
            for (int i = 0; i <= numberOfIterations; i++)
            {
                F deserializedObject = deserializationMethod(deserializableString) ?? new F();
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
