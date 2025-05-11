namespace AgrifoodManagement.Web.Models.Report
{
    public class ReportsViewModel
    {
        public double[] CellSpacing { get; set; }

        public List<ColumnData> ColumnChartData { get; set; }

        public List<PieData> PieData { get; set; }
        public string[] Palettes { get; set; }

        public List<SplineData> SplineChartData { get; set; }
    }
}
