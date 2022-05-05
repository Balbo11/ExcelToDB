using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ExcelToDB
{
    internal class Program
    {


        static void Main(string[] args)
        {
            Excel excel = new Excel("C:\\Users\\a.sjovall\\OneDrive - ONEPOINT\\Documents\\Book1.xlsx");

            List<GameItemDto> excelData = new List<GameItemDto>();

            excelData = excel.ReadgameItems();

            AddData(excelData);

            Console.ReadKey();
            excel.Quit();


        }

        static async Task<List<GameItemDto>> AddItems(List<GameItemDto> excelData)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7070/");
            HttpResponseMessage response = new HttpResponseMessage();

            foreach (var item in excelData)
            {
                response = await client.PostAsJsonAsync("api/GameItems", item);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                item.Id = int.Parse(System.Text.Json.JsonDocument.Parse(json).RootElement.GetProperty("id").ToString());
                
            }




            return excelData;
        }

        static async Task<Uri> AddData(List<GameItemDto> excelData)
        {
            excelData = await AddItems(excelData);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7070/");
            HttpResponseMessage response = new HttpResponseMessage();

            List<Publisher> existingPublishers = new List<Publisher>();
            response = await client.GetAsync(client.BaseAddress + "api/Publishers");
            string json = await response.Content.ReadAsStringAsync();
            existingPublishers = JsonSerializer.Deserialize<List<Publisher>>(json);

            List<string> requestedPublishers = new List<string>();

            foreach (GameItemDto item in excelData)
                requestedPublishers.AddRange(item.Publishers);

            requestedPublishers = requestedPublishers.Distinct().ToList();
            foreach (Publisher publisher in existingPublishers)
            {
                if (requestedPublishers.Contains(publisher.name))
                {

                    requestedPublishers.RemoveAll(x => x.Equals(publisher.name));
                }
            }

            Publisher publisherToAdd = new Publisher();

            foreach (var publisher in requestedPublishers)
            {
                publisherToAdd.name = publisher;
                response = await client.PostAsJsonAsync("api/Publishers", publisherToAdd);
                response.EnsureSuccessStatusCode();
                json = await response.Content.ReadAsStringAsync();
                existingPublishers.Add(JsonSerializer.Deserialize<Publisher>(json));
            }

            response = await AddGameItemPublishers(excelData, existingPublishers);

            return response.Headers.Location;


        }

        static async Task<HttpResponseMessage> AddGameItemPublishers(List<GameItemDto> gameItems, List<Publisher> existingPublishers)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7070/");
            HttpResponseMessage response = new HttpResponseMessage();

            GameItemPublisherDto gameItemPublisherDto = new GameItemPublisherDto();
            foreach (GameItemDto item in gameItems)
            {
                gameItemPublisherDto.GameItemId = item.Id;
                foreach (Publisher publisher in existingPublishers)
                {
                    if (item.Publishers.Contains(publisher.name))
                    {
                        gameItemPublisherDto.PublisherId = publisher.id;
                        response = await client.PostAsJsonAsync("api/GameItems/addPublisher", gameItemPublisherDto);
                    }
                }
            }


            return response;
        }
    }
}
