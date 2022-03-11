using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace RedisCacheDemoApp
{

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionMultiplexer connection =ConnectionMultiplexer.Connect("");
            IDatabase cache = connection.GetDatabase();
            cache.StringSet("stringValue", "my string", new TimeSpan(0, 10, 0));
            cache.StringAppend("stringValue", " - more added to my string");
            string entry = cache.StringGet("stringValue");
            Console.WriteLine(entry);

            RedisKey key = "intValue";
            RedisValue value = 10;
            cache.StringSet(key, value);

            cache.StringIncrement("intValue");
            value = (int)cache.StringGet("intValue");
            Console.WriteLine(value);

            Person person = new Person() { Age = 19, Name = "Foo1" };
            string serializedFoo = JsonConvert.SerializeObject(person);
            cache.StringSet("serializedFoo", serializedFoo);
            person = JsonConvert.DeserializeObject<Person>(cache.StringGet("serializedFoo"));
            Console.WriteLine(person.Name + " " + person.Age);

            cache.HashSet("user1", "Dell Laptops", 2);
            cache.HashSet("user1", "SSD Drive", 10);
            cache.HashSet("user2", "Wooden Table", 1);
            cache.HashSet("user2", "Plastic Chair", 5);
            var qtyOfGivenProductByUser = cache.HashGet("user1", "Dell Laptops");
            Console.WriteLine(qtyOfGivenProductByUser);
            HashEntry[] allItemsInCartforUser1 = cache.HashGetAll("user1");
            foreach (var item in allItemsInCartforUser1)
            {
                Console.WriteLine($"{item.Name} - {item.Value}");
            }
        }
    }
}
