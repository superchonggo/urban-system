using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Model
{
    public class FillMessage
    {
        public FillMessage()
        {
            ContentType = "Journal";
            Loaders = new string[] { "MappingSearch" };
            JobPriority = "Bulk/Reload";
        }

        public string ContentType { get; set; }
        public string[] ContentSetIds { get; set; }
        public string[] Loaders { get; set; }
        public string ProductId { get; set; }
        public string ProductVersion { get; set; }
        public string JobPriority { get; set; }
        public string UpdateId { get; set; }
        public string HarvestUri { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }


    public class BulkFile
    {
        public string Location { get; set; }
        public string ContentSetId { get; set; }
    }

    public class BulkUpdate
    {
        public string UpdateId { get; set; }
        public int TotalFiles { get; set; }
        public List<BulkFile> BulkFiles { get; set; }
    }

    public class Product
    {
        public string ProductId { get; set; }
        public string ProductVersion { get; set; }
        public int TotalUpdates { get; set; }
        public List<BulkUpdate> BulkUpdates { get; set; }
    }

    public class HarvestList
    {
        public string AsOf { get; set; }
        public List<Product> Products { get; set; }
    }
}
