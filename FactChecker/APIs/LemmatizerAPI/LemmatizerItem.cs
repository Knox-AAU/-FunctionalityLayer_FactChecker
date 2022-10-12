using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.APIs.LemmatizerAPI
{
    /// <summary>
    /// A class containing a field for a lemmatized string. 
    /// </summary>
    public class LemmatizerItem
    {
        public string lemmatized_string;
        public string lemmatized_language { get; set; }
    }
}
