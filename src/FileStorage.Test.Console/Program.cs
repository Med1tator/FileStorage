using System.IO;
using System.Net.Http;

namespace FileStorage.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent("data");
                var streamContent1 = new StreamContent(File.Open(@"C:\Users\medit.ding\Desktop\temp\golang.png", FileMode.Open));
                var streamContent2 = new StreamContent(File.Open(@"C:\Users\medit.ding\Desktop\temp\golang1.png", FileMode.Open));

                content.Add(streamContent1, "golang", "golang.png");
                content.Add(streamContent2, "golang1", "golang1.png");
                var result = httpClient.PostAsync("https://localhost:44351/api/file/upload", content).Result;

                System.Console.WriteLine(result.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
