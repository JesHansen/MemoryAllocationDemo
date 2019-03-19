using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringPool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press ENTER for nonpooled.");
            Console.ReadKey();
            UseNonPooled();
            UseNonPooled();
            UseNonPooled();
            Console.WriteLine("Press ENTER for pooled.");
            Console.ReadKey();
            UsePooled();
            UsePooled();
            UsePooled();
        }

        private static void UsePooled()
        {
            Console.WriteLine("Starting pooled");
            var now = Stopwatch.StartNew();
            var values = new string[1000000];
            var pool = new StringCache();
            
            for (int i = 0; i < 1000000; i++)
            {
                var sb = new StringBuilder();
                sb.Append(pool.PoolString($"Jes Hansen "));
                sb.AppendLine(pool.PoolString((i % 10).ToString()));
                values[i] = pool.PoolString(sb.ToString());
            }
            var done = now.Elapsed;
            Console.WriteLine($"Done, pooled, press ENTER to release. {done.TotalMilliseconds} ms.");
            Console.ReadKey();

            File.WriteAllLines(@"C:\temp\pooled.txt", values);
        }

        private static void UseNonPooled()
        {
            Console.WriteLine("Starting non pooled");
            var now = Stopwatch.StartNew();
            var values = new string[1000000];
            
            for (int i = 0; i < 1000000; i++)
            {
                var sb = new StringBuilder();
                sb.Append($"Jes Hansen ");
                sb.AppendLine((i % 10).ToString());
                values[i] = sb.ToString();
            }

            var done = now.Elapsed;
            Console.WriteLine($"Done, non-pooled, press ENTER to release. {done.TotalMilliseconds} ms.");
            Console.ReadKey();

            File.WriteAllLines(@"C:\temp\nonpooled.txt", values);
        }
    }

    class StringCache
    {
        private readonly Dictionary<string, string> pool = new Dictionary<string, string>();

        public string PoolString(string value)
        {
            if (!pool.TryGetValue(value, out var cachedString))
            {
                pool.Add(value, value);
                return pool[value];
            }

            return cachedString;
        }
    }
}
