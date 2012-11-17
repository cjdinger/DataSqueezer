using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SAS.Shared.AddIns;
using SAS.Tasks.Toolkit.Controls;
using System.Collections.Generic;

namespace SAS.Tasks.DataSqueezer
{
    /// <summary>
    /// This windows form inherits from the TaskForm class, which
    /// includes a bit of special handling for SAS Enterprise Guide.
    /// </summary>
    public partial class DataSqueezerTaskForm : SAS.Tasks.Toolkit.Controls.TaskForm
    {
        #region Class Properties
        /// <summary>
        /// The stored settings for the task
        /// </summary>
        public DataSqueezerTaskSettings Settings { get; set; }

        /// <summary>
        /// helper to populate compression options
        /// </summary>
        private Dictionary<string, CompressOptions> availOpts = 
            CompressOptions.GetSupportedOptions();
        #endregion

        #region Initialization

        public DataSqueezerTaskForm(SAS.Shared.AddIns.ISASTaskConsumer3 consumer)
        {
            InitializeComponent();

            // provide a handle to the SAS Enterprise Guide application
            this.Consumer = consumer;

            // init compression options
            cmbCompression.DisplayMember = "Name";

            foreach (string key in availOpts.Keys)
            {
                cmbCompression.Items.Add(availOpts[key]);
            }
            cmbCompression.SelectedIndex = 0;
        }

        // initialize the form with the values from the settings
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // set the controls value based on the settings
            chkCreateReport.Checked = Settings.IncludeReport;
            txtOutputData.Text = Settings.OutputData;

            if (availOpts.ContainsKey(Settings.Compress))
            {
                cmbCompression.SelectedItem = availOpts[Settings.Compress];
            }
        }

        #endregion

        #region OnClosing - validation
        // save any values from the dialog into the settings class
        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                // validate dataset name - some crude validation
                // a regular expression would be better
                string[] parts = txtOutputData.Text.Trim().Split(new char[] { '.' });
                if (txtOutputData.Text.Trim().Contains(" ") ||
                    parts.Length != 2 ||
                    parts[0].IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > -1 ||
                    parts[1].IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > -1)
                {
                    MessageBox.Show("Output data name must use valid LIBNAME.MEMBER notation.", 
                        "Output is invalid");
                    e.Cancel = true;
                }
                else
                {
                    Settings.OutputData = txtOutputData.Text;
                    Settings.IncludeReport = chkCreateReport.Checked;
                    CompressOptions sel = cmbCompression.SelectedItem as CompressOptions;
                    Settings.Compress = sel.Syntax;
                }
            }

            base.OnClosing(e);
        }
        #endregion

        #region Handler for output data browse

        /// <summary>
        /// This is the handler for the Browse button to select an
        /// output data set.
        /// It makes a single call to the consumer application
        /// (SAS Enterprise Guide) to show the output data selector,
        /// pinning the output to a single server (same server
        /// where the input data resides).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string cookie ="";
            ISASTaskDataName name =
                Consumer.ShowOutputDataSelector(this,
                 ServerAccessMode.OneServer,
                 Consumer.AssignedServer,
                 "", "", ref cookie);

            // if the user closes the dialog without making a selection,
            // the name will be null.
            if (name != null)
            {
                txtOutputData.Text = 
                    string.Format("{0}.{1}", name.Library, name.Member);
            }
        }
        #endregion
    }
}
