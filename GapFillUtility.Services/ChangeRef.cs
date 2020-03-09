using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GapFillUtility.Services
{
    [Obsolete("This code should be referenced from WK.Health.System.LoaderCoordinator.Contract")]
    public class LoadRequest
    {
        //// [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[global::System.ComponentModel.DataAnnotations.RegularExpressionAttribute("^(?i)(update|reload)$")]
        public string Action { get; set; }
        //[global::System.ComponentModel.DataAnnotations.RequiredAttribute]
        public string Collection { get; set; }
        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ProductId { get; set; }
        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ProductVersion { get; set; }
        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UpdateId { get; set; }
        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ContentSetId { get; set; }
        // [JsonIgnore]
        // [Obsolete("Please use ContentUrlArray instead")]
        public string ContentUrls { get; set; }
        public string[] ContentUrlArray { get; set; }
        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }
        public string TaskId { get; set; }
        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AssetCountX AssetCount { get; set; }
        // [JsonIgnore]
        // [Obsolete("Please use UpdateId instead")]
        public string VersionId { get; set; }
    }

    // todo: this class should be removed 
    // using WK.Health.System.LoaderCoordinator.Contract.SSR;
    // namespace WK.Health.System.LoaderCoordinator.Contract.SSR
    [Obsolete("This code should be referenced from WK.Health.System.LoaderCoordinator.Contract.SSR")]
    public class AssetCountX
    {
        public string UpdateId { get; set; }
        public string ChangesetId { get; set; }
        public string ProductId { get; set; }
        public string ProductVersion { get; set; }
        public OfTypeCountX[] OfTypeCounts { get; set; }
        public int TotalAdded { get; set; }
        public int TotalUpdated { get; set; }
        public int TotalDeleted { get; set; }
    }

    // namespace WK.Health.System.LoaderCoordinator.Contract.SSR
    [Obsolete("This code should be referenced from WK.Health.System.LoaderCoordinator.Contract.SSR")]
    public class OfTypeCountX
    {
        public string OfType { get; set; }
        public int Added { get; set; }
        public int Deleted { get; set; }
        public int Updated { get; set; }
    }
}
