using finalExam_diplom_.classes.databaseTables;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Net.Http;
using System.Net.Http.Json;

namespace finalExam_diplom_.classes
{
    public class DiagramsClass
    {
        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7070/")
        };
        public async Task<List<DiagramTable>> GetCountAsync(string? reason, string? category)
        {
            var status = await _httpClient.GetFromJsonAsync<List<DiagramTable>>($"api/count/statuses?reason={reason}&category={category}");
            return status ?? new List<DiagramTable>();
        }

        public async Task<List<DiagramTable>> GetCountPrioritiesAsync(string? reason, string? category)
        {
            var status = await _httpClient.GetFromJsonAsync<List<DiagramTable>>($"api/count/priorities?reason={reason}&category={category}");
            return status ?? new List<DiagramTable>();
        }
    }
}
