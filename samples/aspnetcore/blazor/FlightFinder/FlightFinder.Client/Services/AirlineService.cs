using FlightFinder.Shared;
using Microsoft.AspNetCore.Blazor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightFinder.Client.Services
{
    public class AirlineService
    {
        private readonly HttpClient http;

        public AirlineService(HttpClient httpInstance)
        {
            http = httpInstance;
        }
        public async Task<Itinerary[]> Search(SearchCriteria criteria)
        {    
            var results = await http.PostJsonAsync<Itinerary[]>("/api/flightsearch", criteria);
            return results;
        }
    }
}
