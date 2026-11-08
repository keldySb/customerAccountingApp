using finalExam_diplom_.classes.databaseTables;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace finalExam_diplom_.classes
{
    public class DictionaryClass
    {
        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7070/")
        };

        public async Task<List<DictionaryItems>> GetStatusAsync(bool changeStatus = false)
        {
            string url = "api/dictionaries/statuses";
            if (changeStatus)
            {
                url += "?changeStatus=true";
            }

            var status = await _httpClient.GetFromJsonAsync<List<DictionaryItems>>(url);

            return status ?? new List<DictionaryItems>();
        }

        public async Task<List<DictionaryItems>> GetPriorityAsync()
        {
            var status = await _httpClient.GetFromJsonAsync<List<DictionaryItems>>("api/dictionaries/priorities");
            return status ?? new List<DictionaryItems>();
        }

        public async Task<List<DictionaryItems>> GetManagerAsync()
        {
            var status = await _httpClient.GetFromJsonAsync<List<DictionaryItems>>("api/dictionaries/managers");
            return status ?? new List<DictionaryItems>();
        }

        public async Task<List<DictionaryItems>> GetReason()
        {
            var status = await _httpClient.GetFromJsonAsync<List<DictionaryItems>>("api/dictionaries/reasons");
            return status ?? new List<DictionaryItems>();
        }

        public async Task<List<DictionaryItems>> GetCategory(int? reason)
        {
            var status = await _httpClient.GetFromJsonAsync<List<DictionaryItems>>($"api/dictionaries/categories?reason={reason}");
            return status ?? new List<DictionaryItems>();
        }
    }
}
