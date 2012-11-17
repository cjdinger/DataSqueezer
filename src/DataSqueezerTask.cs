using System;
using System.Text;
using SAS.Shared.AddIns;
using SAS.Tasks.Toolkit;

namespace SAS.Tasks.DataSqueezer
{
    // unique identifier for this task
    [ClassId("f952a545-2387-44ce-b6b8-0a4ee3335bf1")]
    // location of the task icon to show in the menu and process flow
    [IconLocation("SAS.Tasks.DataSqueezer.task.ico")]
    // does this task require input data? 
    [InputRequired(InputResourceType.Data)]
    public class DataSqueezerTask : SAS.Tasks.Toolkit.SasTask
    {
        #region private members

        private DataSqueezerTaskSettings settings;

        #endregion

        #region Initialization
        public DataSqueezerTask()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // 
            // DataSqueezerTask
            // 
            this.ProcsUsed = "DATA step";
            this.ProductsRequired = "BASE";
            this.TaskCategory = "SAS Press Examples";
            this.TaskDescription = "Reduce the storage needed for a SAS data set";
            this.TaskName = "Squeeze SAS data";

        }
        #endregion

        #region overrides
        public override bool Initialize()
        {
            settings = new DataSqueezerTaskSettings();
            return true;
        }

        public override string GetXmlState()
        {
            return settings.ToXml();
        }

        public override void RestoreStateFromXml(string xmlState)
        {
            settings = new DataSqueezerTaskSettings();
            settings.FromXml(xmlState);
        }

        /// <summary>
        /// Show the task user interface
        /// </summary>
        /// <param name="Owner"></param>
        /// <returns>whether to cancel the task, or run now</returns>
        public override ShowResult Show(System.Windows.Forms.IWin32Window Owner)
        {
            DataSqueezerTaskForm dlg = new DataSqueezerTaskForm(this.Consumer);
            dlg.Settings = settings;
            if (System.Windows.Forms.DialogResult.OK == dlg.ShowDialog(Owner))
            {
                // gather settings values from the dialog
                settings = dlg.Settings;
                return ShowResult.RunNow;
            }
            else
                return ShowResult.Canceled;
        }

        /// <summary>
        /// Get the SAS program that this task should generate
        /// based on the options specified.
        /// </summary>
        /// <returns>a valid SAS program to run</returns>
        public override string GetSasCode()
        {
            return settings.GetSasProgram(string.Format("{0}.{1}", Consumer.ActiveData.Library, Consumer.ActiveData.Member));
        }

        // returns 1 data output, always
        public override int OutputDataCount
        {
            get { return 1; }
        }

        // build the output description for the data set
        public override System.Collections.Generic.List<ISASTaskDataDescriptor> OutputDataDescriptorList
        {
            get
            {
                System.Collections.Generic.List<ISASTaskDataDescriptor> outList = 
                    new System.Collections.Generic.List<ISASTaskDataDescriptor>();

                string[] parts = settings.OutputData.Split(new char[] { '.' });
                if (parts.Length == 2)
                {
                    outList.Add(
                        // use this helper method to build the output descriptor
                        SAS.Shared.AddIns.SASTaskDataDescriptor.CreateLibrefDataDescriptor(
                            Consumer.AssignedServer, parts[0], parts[1], "")
                        );
                }
                return outList;
            }
        }
        #endregion

    }
}
