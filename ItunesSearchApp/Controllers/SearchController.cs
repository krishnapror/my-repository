using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ItunesSearchApp.Controllers
{
    public class SearchController : Controller
    {
        static HttpClient client = new HttpClient();
        public async Task<ActionResult> Index()
        {
            var obj = new List<Result>();
            return View(obj);
        }
        public async Task<List<Result>> SearchItunes(string searchRegex)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var streamTask = client.GetStringAsync("https://itunes.apple.com/search?term="+ searchRegex +"& limit=25");
            var result = await streamTask;
            var searchResults = JsonConvert.DeserializeObject<ItunesSearchResult>(result);
            return searchResults.results.Where(x => x.artistId != 0).Distinct().ToList();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string searchRegex)
        {
            var results = await SearchItunes(searchRegex);
            return View(results);
        }
    }
    public class Result
    {
        public string wrapperType { get; set; }
        public string kind { get; set; }
        public int artistId { get; set; }
        public int collectionId { get; set; }
        public int trackId { get; set; }
        public string artistName { get; set; }
        public string collectionName { get; set; }
        public string trackName { get; set; }
        public string collectionCensoredName { get; set; }
        public string trackCensoredName { get; set; }
        public string artistViewUrl { get; set; }
        public string collectionViewUrl { get; set; }
        public string trackViewUrl { get; set; }
        public string previewUrl { get; set; }
        public string artworkUrl30 { get; set; }
        public string artworkUrl60 { get; set; }
        public string artworkUrl100 { get; set; }
        public double collectionPrice { get; set; }
        public double trackPrice { get; set; }
        public DateTime releaseDate { get; set; }
        public string collectionExplicitness { get; set; }
        public string trackExplicitness { get; set; }
        public int discCount { get; set; }
        public int discNumber { get; set; }
        public int trackCount { get; set; }
        public int trackNumber { get; set; }
        public int trackTimeMillis { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public string primaryGenreName { get; set; }
        public bool isStreamable { get; set; }
    }

    public class ItunesSearchResult
    {
        public int resultCount { get; set; }
        public List<Result> results { get; set; }
    }


}