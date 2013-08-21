using SampleApp.Shared.Models;
using System.Collections.Generic;

namespace SampleApp.Shared.Services
{
    public class SampleItemService
    {
        private static List<SampleItem> _items = new List<SampleItem>(new SampleItem[] { 
                new SampleItem { Title = "One", Description = "First item" },
                new SampleItem { Title = "Two", Description = "Second item" },
                new SampleItem { Title = "Three", Description = "Third item" }
        });

        public List<SampleItem> GetItems()
        {
            return _items;
        }
    }
}
