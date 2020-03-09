using System.Collections.Generic;

namespace GapFillUtility.Services
{
    public static class ServiceConstants
    {
        public static List<string> CCCLoaders()
        {
            return new List<string>
            {
                "asset",
                "crossref",
                "linkfarm",
                "mappingsearch",
                "metadata",
                "multifieldsearch",
                "typeaheadsearch"
            };
        }

        public static List<string> Environments()
        {
            return new List<string>
            {
                "development",
                "staging",
                "production"
            };
        }
    }
}
