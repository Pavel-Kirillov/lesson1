using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace lesson1
{
    internal class Program
    {
        static async Task HttpResponseToFile(int postId, StreamWriter fileStream)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{postId}");
            var content = await response.Content.ReadAsStreamAsync();

            JsonElement root = JsonDocument.Parse(content).RootElement;

            lock (fileStream)
            {
                foreach (var element in root.EnumerateObject())
                {
                    fileStream.WriteLine(element.Value);
                }
                fileStream.WriteLine();
            }
        }
        static async Task Main()
        {
            var tasks = new List<Task>();
            using (StreamWriter fileStream = File.CreateText("out.txt"))
            {
                for (int i = 4; i <= 13; i++)
                {
                    tasks.Add(HttpResponseToFile(i, fileStream));
                }
                await Task.WhenAll(tasks);
            }
        }
    }
}
