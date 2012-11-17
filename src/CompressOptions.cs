using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Tasks.DataSqueezer
{
    /// <summary>
    /// Simple class to represent the available COMPRESS options and their
    /// friendly descriptions
    /// </summary>
    public class CompressOptions
    {
        /// <summary>
        /// SAS keyword for the COMPRESS option
        /// </summary>
        public string Syntax { get; set; }
        /// <summary>
        /// Descriptive name for the type of compression
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Return a list of the possible options
        /// For use in a UI element, such as a dropdown
        /// combobox.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, CompressOptions> GetSupportedOptions()
        {
            Dictionary<string, CompressOptions> opts = new Dictionary<string, CompressOptions>();

            opts.Add("NO", new CompressOptions() 
            { Syntax = "NO", Name = "None" });
            opts.Add("CHAR", new CompressOptions() 
            { Syntax = "CHAR", Name = "Run Length Encoding (RLE)" });
            opts.Add("BINARY", new CompressOptions() 
            { Syntax = "BINARY", Name = "Ross Data Compression (RDC)" });

            return opts;
        }
    }
}
