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
        public async Task<List<DictionaryItems>> GetStatusAsync()
        {
            var status = await _httpClient.GetFromJsonAsync<List<DictionaryItems>>("api/dictionaries/statuses");
            return status ?? new List<DictionaryItems>();
        }

        public async Task<List<DictionaryItems>> GetPriorityAsync()
        {
            var status = await _httpClient.GetFromJsonAsync<List<DictionaryItems>>("api/dictionaries/priorities");
            return status ?? new List<DictionaryItems>();
        }

        public async Task<List<DictionaryItems>> GetMasterAsync()
        {
            var status = await _httpClient.GetFromJsonAsync<List<DictionaryItems>>("api/dictionaries/masters");
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

        public async Task<List<DictionaryItems>> GetCategory()
        {
            var status = await _httpClient.GetFromJsonAsync<List<DictionaryItems>>("api/dictionaries/categories");
            return status ?? new List<DictionaryItems>();
        }
    }
}
