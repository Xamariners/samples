
using FlightFinder.Shared;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Browser.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly List<string> logs = new List<string>();
        public IReadOnlyList<string> Logs => logs;

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
                AddToLog(criteria.FromAirport + DateTime.Now.ToString());

                AddToLog(string.Join(";", historyService.SearchHistoryList.Select(x => x.FromAirport)));
                
                SearchResults = await airlineService.Search(criteria);
                historyService.AddToHistoryList(criteria);
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

        public void AddToLog(String log)
        {
            logs.Add(log);
            NotifyStateChanged();
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

