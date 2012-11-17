using System;
using System.Text;
using System.Xml;

namespace SAS.Tasks.DataSqueezer
{
    /// <summary>
    /// Use this class to keep track of the 
    /// options that are set within your task.
    /// You must save and restore these settings when the user
    /// interacts with your task user interface,
    /// and when the task runs within the process flow.
    /// </summary>
    public class DataSqueezerTaskSettings
    {
        /// <summary>
        /// Set initial default values
        /// </summary>
        public DataSqueezerTaskSettings()
        {
            Compress = "NO";
            IncludeReport = true;
            OutputData = "WORK.OUT_";
        }

        #region Properties, or task settings

        /// <summary>
        /// Reference for the output data set
        /// </summary>
        public string OutputData { get; set; }
        /// <summary>
        /// Method for compression, if any
        /// </summary>
        public string Compress { get; set; }
        /// <summary>
        /// Whether to generate a summary report
        /// </summary>
        public bool IncludeReport { get; set; }

        #endregion

        #region Code to save/restore task settings
        public string ToXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement el = doc.CreateElement("DataSqueezerTask");
            el.SetAttribute("outputData", OutputData);
            el.SetAttribute("compress", Compress);
            el.SetAttribute("includeReport", XmlConvert.ToString(IncludeReport));
            doc.AppendChild(el);
            return doc.OuterXml;
        }

        public void FromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
                XmlElement el = doc["DataSqueezerTask"];
                OutputData = el.GetAttribute("outputData");
                Compress = el.GetAttribute("compress");
                IncludeReport = XmlConvert.ToBoolean(el.GetAttribute("includeReport"));
            }
            catch (XmlException)
            {
                // couldn't read the XML content
            }
        }
        #endregion

        #region Routine to build a SAS program
        /// <summary>
        /// Much of the "stock" SAS code for this task is
        /// stored as embedded resources within the 
        /// .NET assembly.
        /// This makes the code files a bit easier to maintain.
        /// </summary>
        /// <param name="librefMember">The data reference for the input data</param>
        /// <returns>A complete SAS program</returns>
        public string GetSasProgram(string librefMember)
        {
            StringBuilder sb = new StringBuilder();
            string[] inParts = librefMember.Split(new char[] { '.' });
            string[] outParts = OutputData.Split(new char[] { '.' });

            string macro = SAS.Tasks.Toolkit.Helpers.UtilityFunctions.ReadFileFromAssembly("SAS.Tasks.DataSqueezer.squeezechars.sas");
            macro = string.Format(macro, inParts[0].ToUpper(), inParts[1].ToUpper(), outParts[0].ToUpper(), outParts[1].ToUpper());

            sb.AppendLine(macro);

            if (IncludeReport)
            {
                sb.AppendLine(SAS.Tasks.Toolkit.Helpers.UtilityFunctions.ReadFileFromAssembly("SAS.Tasks.DataSqueezer.preReport.sas"));
            }

            sb.AppendLine("/* macro call to \"squeeze\" the data and optionally compress it */");
            sb.AppendFormat("%squeezeChars(&inLib..&inMem., &outLib..&outMem., compress={0});\n", Compress);

            if (IncludeReport)
            {
                sb.AppendLine(SAS.Tasks.Toolkit.Helpers.UtilityFunctions.ReadFileFromAssembly("SAS.Tasks.DataSqueezer.postReport.sas"));
            }

            return sb.ToString();
        }
        #endregion
    }
}
