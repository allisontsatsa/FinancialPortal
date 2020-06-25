using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.ViewModels
{
    public class ChartVM
    {
        public List<ChartDataSet> datasets { get; set; }
        public List<string> labels { get; set; }
        public ChartVM()
        {
            datasets = new List<ChartDataSet>();
            labels = new List<string>();
        }
    }

    public class ChartDataSet
    {
        public List<decimal> data { get; set; }
        public List<string> backgroundColor { get; set; }
        public ChartDataSet()
        {
            data = new List<decimal>();
            backgroundColor = new List<string>();
        }
    }

    public class BarChartVM

    {
        public List<BarChartDataSet> datasets { get; set; }
        public List<string> labels { get; set; }
        public BarChartVM()
        {
            datasets = new List<BarChartDataSet>();
            labels = new List<string>();
        }
    }

    public class BarChartDataSet
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public bool pointRadius { get; set; }
        public string pointColor { get; set; }
        public string pointStrokeColor { get; set; }
        public string pointHighlightFill { get; set; }
        public string pointHighlightStroke { get; set; }
        public List<decimal> data { get; set; }

        public BarChartDataSet()
        {
            data = new List<decimal>();
        }
    }
}
