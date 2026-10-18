using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using finalExam_diplom_.classes.databaseTables;

public class AuthClient
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://localhost:7070/")
    };

    public async Task<DispatchersTable?> LoginAsync(
        string login,
        string password)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/authorization/login",
            new
            {
                Login = login,
                Password = password
            });

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<DispatchersTable>();
    }
}