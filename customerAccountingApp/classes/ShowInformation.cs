using finalExam_diplom_.classes.databaseTables;
using Npgsql;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace finalExam_diplom_.classes
{
    public class GetClientsRequestsTable()
    {
        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7070/")
        };

        public async Task<List<ClientsRequestTable>> GetClientsRequestsAsync(int? statusId)
        {
            var status = await _httpClient.GetFromJsonAsync<List<ClientsRequestTable>>($"api/requests?statusId={statusId}");
            return status ?? new List<ClientsRequestTable>();
        }

        public async Task<List<ClientsRequestTable>> GetClientsRequestsSearchAsync(int? statusId, string? search)
        {
            var status = await _httpClient.GetFromJsonAsync<List<ClientsRequestTable>>($"api/requests/search?statusId={statusId}&search={Uri.EscapeDataString(search)}");
            return status ?? new List<ClientsRequestTable>();
        }
    }

    public class GetEmployeeTable()
    {

        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7070/")
        };
        public async Task<List<EmployeesTable>> GetEmployeesAsync()
        {
            var status = await _httpClient.GetFromJsonAsync<List<EmployeesTable>>("api/employees");
            return status ?? new List<EmployeesTable>();
        }
        public async Task<List<EmployeesTable>> GetEmployeesSearchAsync(string? search)
        {
            var status = await _httpClient.GetFromJsonAsync<List<EmployeesTable>>($"api/employees/search?search={Uri.EscapeDataString(search)}");
            return status ?? new List<EmployeesTable>();
        }
    };
}
