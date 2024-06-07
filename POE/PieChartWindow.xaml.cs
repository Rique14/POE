using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Windows;

namespace POE
{
    public partial class PieChartWindow : Window
    {
        public PieChartWindow(Dictionary<string, double> foodGroupTotals)
        {
            InitializeComponent();

            var pieSeries = new PieSeries { StrokeThickness = 2.0, AngleSpan = 360, StartAngle = 0 };

            foreach (var foodGroup in foodGroupTotals)
            {
                pieSeries.Slices.Add(new PieSlice(foodGroup.Key, foodGroup.Value));
            }

            var model = new PlotModel { Title = "Food Group Distribution" };
            model.Series.Add(pieSeries);

            PieChartView.Model = model;
        }
    }
}
