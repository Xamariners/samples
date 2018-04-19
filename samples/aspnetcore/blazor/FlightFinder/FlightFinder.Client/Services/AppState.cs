
using FlightFinder.Shared;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Browser.Interop;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightFinder.Client.Services
{
    public class AppState
    {
        // Actual state
        public IReadOnlyList<Itinerary> SearchResults { get; private set; }
        public bool SearchInProgress { get; private set; }

        private readonly List<Itinerary> shortlist = new List<Itinerary>();
        public IReadOnlyList<Itinerary> Shortlist => shortlist;

        // Lets components receive change notifications
        // Could have whatever granularity you want (more events, hierarchy...)
        public event Action OnChange;

        // Receive 'http' instance from DI
        private readonly HttpClient http;
        private readonly HistoryService historyService;
        private readonly AirlineService airlineService;

        public AppState(HttpClient httpInstance, HistoryService historyServiceInstance, AirlineService airlineServiceInstance)
        {
            http = httpInstance;
            historyService = historyServiceInstance;
            airlineService = airlineServiceInstance;
        }

        public async Task Search(SearchCriteria criteria)
        {
            SearchInProgress = true;
            NotifyStateChanged();
            try
            {
                historyService.AddToHistoryList(criteria);
                SearchResults = await airlineService.Search(criteria);               
            }
            catch(Exception ex)
            {
                RegisteredFunction.Invoke<object>("Alert", ex.Message);
            }
            finally
            {
                SearchInProgress = false;
                NotifyStateChanged();
            }           
        }

        public void AddToShortlist(Itinerary itinerary)
        {
            shortlist.Add(itinerary);
            NotifyStateChanged();
        }

        public void RemoveFromShortlist(Itinerary itinerary)
        {
            shortlist.Remove(itinerary);
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

