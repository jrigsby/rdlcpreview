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

         LoadReport();
         // for external image
         
         reportViewer1.RefreshReport();
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



         //add namespaces so that xml may be traversed
         //http://microsoft.public.sqlserver.reportingsvcs.narkive.com/U1JHd8Nj/unable-to-parse-rdlc-with-xpath
         var ns = new XmlNamespaceManager(xml.NameTable);
         //midway through editing a file the namespace changed, I had loaded a new version of VS so I assume thats it.
         //point is I couldnt count on default namespace being the same, it switched to 2016/01 so now loading dynamically
         //and switching from ns to call it default
         //ns.AddNamespace("ns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
         // ns.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
         foreach (XmlAttribute nsdef in report.Attributes) {
            var attributeName = nsdef.Name;
            if (attributeName.StartsWith("xmlns")) {
               if (attributeName.Contains(":")) {
                  ns.AddNamespace(attributeName.Split(':')[1], nsdef.InnerText);
               }
               else {
                  ns.AddNamespace("default", nsdef.InnerText);
               }
            }
         }

         reportViewer1.ProcessingMode = ProcessingMode.Local;
         reportViewer1.Reset();
         reportViewer1.LocalReport.EnableExternalImages = true;
         reportViewer1.LocalReport.ReportPath = _reportPath;
         reportViewer1.LocalReport.DataSources.Clear();

         //We need to add datasets used by the report as defined in the report itself.
         //The report knows the name it uses to refer to the dataset and the table in the xml dataset its refering to
         //We need to make that association so the user doesnt need to define them
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

         // Add a handler for SubreportProcessing
         reportViewer1.LocalReport.SubreportProcessing += new
            SubreportProcessingEventHandler(DemoSubreportProcessingEventHandler);
         reportViewer1.RefreshReport();
      }
      void DemoSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e) {
         var ds = new DataSet();
         ds.ReadXml(_dataPath, XmlReadMode.ReadSchema);
         foreach (var name in e.DataSourceNames) {
            e.DataSources.Add(new ReportDataSource(name, ds.Tables[name]));
         }
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
         LoadReport();
      }
      #endregion

      //private void reloadToolStripMenuItem_Click(object sender, EventArgs e) {
      //   var folder = AppDomain.CurrentDomain.BaseDirectory;
      //   DataSet ds = new DataSet();
      //   ds.ReadXml("c:\\temp\\data\\data.xml", XmlReadMode.ReadSchema);
      //   // Setup the report viewer object and get the array of bytes


      //   this.reportViewer1.LocalReport.DataSources.Clear();
      //   this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("PlanDetail", ds.Tables["PlanDetail"])); // Add datasource here
      //   this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("FundData", ds.Tables["FundData"]));
      //   this.reportViewer1.LocalReport.Refresh();

      //   this.reportViewer1.RefreshReport();
      //}
   }
}