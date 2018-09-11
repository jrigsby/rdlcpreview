using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Reporting.WinForms;

namespace ReportsApplication1 {
   public partial class Form1 : Form {
      private readonly string _folder = AppDomain.CurrentDomain.BaseDirectory;
      private string _lastReportFolder = AppDomain.CurrentDomain.BaseDirectory;
      private string _lastDataFolder = AppDomain.CurrentDomain.BaseDirectory;
      private string _dataPath = "";
      private string _reportPath = "";

      public Form1() {
         InitializeComponent();
      }

      private void Form1_Load(object sender, EventArgs e) {
         openFileDialog1.InitialDirectory = _folder;
         openFileDialog1.FileName = "";
         openFileDialog1.Multiselect = false;
         if (string.IsNullOrEmpty(Properties.Settings.Default.DefaultDataPath)) {
            Properties.Settings.Default.DefaultDataPath = AppDomain.CurrentDomain.BaseDirectory + "Data";
         }
         if (string.IsNullOrEmpty(Properties.Settings.Default.DefaultReportPath)) {
            Properties.Settings.Default.DefaultReportPath = AppDomain.CurrentDomain.BaseDirectory;
         }
         _lastDataFolder = Properties.Settings.Default.DefaultDataPath;
         _lastReportFolder = Properties.Settings.Default.DefaultReportPath;
         Properties.Settings.Default.Save();
         reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
         LoadReport();
      }

      #region Menu Events
      private void reloadToolStripMenuItem_Click(object sender, EventArgs e) {
         LoadReport();
      }

      private void assignDataFileToolStripMenuItem_Click(object sender, EventArgs e) {
         SetDataPath();
      }

      private void assignReportFileToolStripMenuItem_Click(object sender, EventArgs e) {
         SetReportPath();
      }
      #endregion

      #region Private methods
      private void LoadReport() {
         if (string.IsNullOrEmpty(_dataPath)) {
            SetDataPath();
            return;
         }
         if (string.IsNullOrEmpty(_reportPath)) {
            SetReportPath();
            return;
         }

         var ds = new DataSet();
         ds.ReadXml(_dataPath, XmlReadMode.ReadSchema);

         XmlDocument xml = new XmlDocument();
         xml.Load(_reportPath);

         var report = xml.DocumentElement;
         if (report == null) {
            MessageBox.Show(@"Report is corrupt, no root element found.");
            return;
         }

         var ns = ParseReportNamespace(xml);

         reportViewer1.ProcessingMode = ProcessingMode.Local;
         reportViewer1.Reset();
         reportViewer1.LocalReport.EnableExternalImages = true;
         reportViewer1.LocalReport.ReportPath = _reportPath;
         reportViewer1.LocalReport.DataSources.Clear();

         //We need to add datasets used by the report as defined in the report itself.
         //The report knows the name it uses to refer to the dataset and the table in the xml dataset its referring to
         //We need to make that association so the user doesn't need to define them
         try {
            // ReSharper disable once PossibleNullReferenceException
            foreach (XmlElement dataset in xml.SelectNodes("//default:Report/default:DataSets/default:DataSet", ns)) {
               var name = dataset.GetAttribute("Name");
               var table = dataset.SelectSingleNode("rd:DataSetInfo/rd:TableName", ns)?.InnerText;
               reportViewer1.LocalReport.DataSources.Add(new ReportDataSource(name, ds.Tables[table]));
            }
         }
         catch (Exception ex) {
            var msg = $"Problem loading datasets: {ex.Message}";
            MessageBox.Show(msg);
         }

         // Add a handler for Subreport Processing
         // ReSharper disable once RedundantDelegateCreation
         reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);
         reportViewer1.RefreshReport();
      }

      //Every time a subreport is called it will go through this method to assign the table from the
      //source data xml to create report datasets.
      void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e) {
         //TODO load from cache for this iteration of report processing
         var ds = new DataSet();
         ds.ReadXml(_dataPath, XmlReadMode.ReadSchema);

         //in this project the subreport is expected to be in same folder as the parent and should not contain the file extension
         //however this code should work with different paths, relative or absolute with or without extension

         var subReportPath = e.ReportPath;

         if (!System.IO.Path.IsPathRooted(e.ReportPath) && sender is LocalReport) {
            //report path not specifically identified
            var parentPath = ((LocalReport)sender).ReportPath;
            var parentFolder = System.IO.Path.GetDirectoryName(parentPath);
            subReportPath = subReportPath.StartsWith("\\") ? $"{parentFolder}{subReportPath}" : $"{parentFolder}\\{subReportPath}";
         }

         if (!subReportPath.ToLower().EndsWith(".rdlc")) {
            subReportPath = $"{subReportPath}.rdlc";
         }

         //TODO load from cache for this iteration of report processing
         var xml = new XmlDocument();
         xml.Load(subReportPath);
         if (xml.DocumentElement is null) throw new Exception("Report document empty.");

         var ns = ParseReportNamespace(xml);

         //go through the sub report datasets and associate report dataset to source data
         foreach (var name in e.DataSourceNames) {
            var tableName = xml.SelectSingleNode($"//default:Report/default:DataSets/default:DataSet[@Name='{name}']/rd:DataSetInfo/rd:TableName", ns)?.InnerText;
            if(string.IsNullOrEmpty(tableName)) throw new Exception($"Dataset table name not defined for {name}.");
            e.DataSources.Add(new ReportDataSource(name, ds.Tables[tableName]));
         }
      }

      private XmlNamespaceManager ParseReportNamespace(XmlDocument xml) {
         if (xml.DocumentElement is null) throw new Exception("Report document empty.");
         //add namespaces so that xml may be traversed
         //http://microsoft.public.sqlserver.reportingsvcs.narkive.com/U1JHd8Nj/unable-to-parse-rdlc-with-xpath
         var ns = new XmlNamespaceManager(xml.NameTable);
         //midway through editing a file the namespace changed, I had loaded a new version of VS so I assume thats it.
         //point is I couldn't count on default namespace being the same, it switched to 2016/01 so now loading dynamically
         //and switching from ns to call it default
         //ns.AddNamespace("ns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
         // ns.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
         foreach (XmlAttribute nsdef in xml.DocumentElement.Attributes) {
            var attributeName = nsdef.Name;
            if (!attributeName.StartsWith("xmlns")) continue;
            if (attributeName.Contains(":")) {
               ns.AddNamespace(attributeName.Split(':')[1], nsdef.InnerText);
            }
            else {
               ns.AddNamespace("default", nsdef.InnerText);
            }
         }

         return ns;
      }

      private void SetDataPath() {
         openFileDialog1.Filter = @"XML Files|*.xml";
         openFileDialog1.Title = @"Please select a data file to preview.";
         openFileDialog1.CheckFileExists = true;
         openFileDialog1.InitialDirectory = _lastDataFolder;

         if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

         _dataPath = openFileDialog1.FileName;

         var fi = new System.IO.FileInfo(openFileDialog1.FileName);
         _lastDataFolder = fi.DirectoryName;

         Properties.Settings.Default.DefaultDataPath = _lastDataFolder;
         Properties.Settings.Default.Save();

         LoadReport();
      }

      private void SetReportPath() {
         openFileDialog1.Filter = @"Client Report Definition Files|*.rdlc";
         openFileDialog1.Title = @"Please select a report file to preview.";
         openFileDialog1.CheckFileExists = true;
         openFileDialog1.InitialDirectory = _lastReportFolder;

         if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

         _reportPath = openFileDialog1.FileName;

         var fi = new System.IO.FileInfo(openFileDialog1.FileName);
         _lastReportFolder = fi.DirectoryName;

         Properties.Settings.Default.DefaultReportPath = _lastReportFolder;
         Properties.Settings.Default.Save();

         LoadReport();
      }
      #endregion
   }
}