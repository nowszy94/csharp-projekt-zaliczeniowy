using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projekt.Model;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace projekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        static HttpClient httpClient = new HttpClient();

        private readonly GithubContext _context;

        public GithubController(GithubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<GithubResponse> GetGithubItems(string user)
        {
            var savedRepos = await _context.GithubRepoItems.ToListAsync();
            var reposFromGithub = user != null ? fetchGithubRepos(user) : new List<GithubRepo>();
            return new GithubResponse
            {
                savedRepos = savedRepos,
                reposFromGithub = reposFromGithub
            };
        }

        private List<GithubRepo> fetchGithubRepos(string user)
        {
            var restClient = new RestClient("https://api.github.com/users/" + user + "/repos");
            var restRequest = new RestRequest();
            restRequest.Method = Method.GET;
            var response = restClient.Execute(restRequest);
            var joResponse = JArray.Parse(response.Content);
            var result = joResponse.ToObject<List<GithubRepo>>();

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GithubRepo>> GetGithubItem(long id)
        {
            var githubItem = await _context.GithubRepoItems.FindAsync(id);

            if (githubItem == null)
            {
                return NotFound();
            }

            return githubItem;
        }

        [HttpPost]
        public async void PostRepo(GithubRepo item)
        {
            item.Id = 0;
            _context.GithubRepoItems.Add(item);
            await _context.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async void RemoveSavedRepo(int id)
        {
            _context.GithubRepoItems.Remove(await _context.GithubRepoItems.FindAsync(id));
            await _context.SaveChangesAsync();
        }
    }

    public class GithubResponse
    {
        public IEnumerable<GithubRepo> savedRepos { get; set; }
        public IEnumerable<GithubRepo> reposFromGithub { get; set; }
    }
}