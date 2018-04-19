using FlightFinder.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightFinder.Client.Services
{
    public class HistoryService
    {
        private readonly List<SearchCriteria> searchHistoryList = new List<SearchCriteria>();
      
        public IReadOnlyList<SearchCriteria> SearchHistoryList => searchHistoryList;

        // Lets components receive change notifications
        // Could have whatever granularity you want (more events, hierarchy...)
        public event Action OnChange;
        
        public void AddToHistoryList(SearchCriteria criteria)
        {
            searchHistoryList.Add(criteria);
            NotifyStateChanged();
        }

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
