using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Net.Http;
using PortfolioProject.Models;
namespace PortfolioProject.Controllers
{
	public class ProjectsController : Controller
	{
		private readonly IMemoryCache _cache;
		private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProjectsController> _logger;
        public ProjectsController(IMemoryCache cache, IHttpClientFactory httpClientFactory, ILogger<ProjectsController> logger) {
            _cache = cache;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index() {
			string cacheKey = "GitHubProjects";
			string gitHubApiUrl = "https://api.github.com/users/opermaster/repos";

			if (!_cache.TryGetValue(cacheKey, out List<Project> projects)) {
				
				var client = _httpClientFactory.CreateClient();
				client.DefaultRequestHeaders.Add("User-Agent", "YourAppName");

                _logger.LogInformation("GET from: "+ gitHubApiUrl);

                var response = await client.GetAsync(gitHubApiUrl);
				if (response.IsSuccessStatusCode) {
					var json = await response.Content.ReadAsStringAsync();
					projects = JsonSerializer.Deserialize<List<Project>>(json);

					
					_cache.Set(cacheKey, projects, TimeSpan.FromHours(1));
				}
			}

			return View(projects);
		}
	}
}
