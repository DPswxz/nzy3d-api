using nzy3D.Chart;
using nzy3D.Colors;
using nzy3D.Colors.ColorMaps;
using nzy3D.Maths;
using nzy3D.Plot3D.Builder;
using nzy3D.Plot3D.Builder.Concrete;
using nzy3D.Plot3D.Primitives;
using nzy3D.Plot3D.Primitives.Axes.Layout.Renderers;
using nzy3D.Plot3D.Rendering.Canvas;
using nzy3D.Plot3D.Rendering.Legends.Colorbars;
using nzy3D.Plot3D.Rendering.View;
using nzy3D.Plot3D.Rendering.View.Modes;
using nzy3d_wpfDemo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace nzy3d_winformsDemo
{
    internal static class ChartsHelper
    {
        /// <summary>
        /// Surface
        /// Build a nice surface to display with cool alpha colors
        /// (alpha 0.8 for surface color and 0.5 for wireframe).
        /// </summary>
        public static Chart GetMapperSurface(Renderer3D renderer3D)
        {
            // Create the chart
            Chart chart = new Chart(renderer3D, Quality.Nicest);
            chart.View.Maximized = false;
            //chart.View.CameraMode = CameraMode.PERSPECTIVE;

            // Create a range for the graph generation
            Range range = new Range(-150, 150);
            const int steps = 50;

            // Build a nice surface to display with cool alpha colors 
            // (alpha 0.8 for surface color and 0.5 for wireframe)
            Shape surface = Builder.BuildOrthonomal(new OrthonormalGrid(range, steps, range, steps), new MyMapper());
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin, surface.Bounds.ZMax, new Color(1, 1, 1, 0.8));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = true;
            surface.WireframeColor = Color.CYAN;
            surface.WireframeColor.Mul(new Color(1, 1, 1, 0.5));

            // Add surface to chart
            chart.Scene.Graph.Add(surface);

            return chart;
        }

        public static Chart GetFRB_H15_dec_2021(Renderer3D renderer3D)
        {
            // Generate data
            var labels = new TickLabelMap();
            List<Coord3d> coords = new List<Coord3d>();
            bool isHeader = true;
            string[] header;
            foreach (var line in File.ReadAllLines("FRB_H15_dec_2021.csv"))
            {
                var data = line.Split('\u002C');
                if (isHeader)
                {
                    header = data;
                    for (int i = 1; i < data.Length; i++)
                    {
                        labels.Register(i, header[i]);
                    }
                    isHeader = false;
                }
                else
                {
                    var date = DateTime.ParseExact(data[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var dateL = date.ToOADate();
                    for (int i = 1; i < data.Length; i++)
                    {
                        coords.Add(new Coord3d(i, dateL, double.Parse(data[i])));
                    }
                }
            }



            // Create the chart
            Chart chart = new Chart(renderer3D, Quality.Nicest);
            chart.View.Maximized = false;
            chart.View.CameraMode = CameraMode.PERSPECTIVE;
            chart.AxeLayout.YTickRenderer = new DateTickRenderer("dd/MM/yyyy");
            chart.AxeLayout.YAxeLabel = "Date";
            chart.AxeLayout.XTickRenderer = labels;
            chart.AxeLayout.XAxeLabel = "Maturity";
            chart.AxeLayout.ZAxeLabel = "Rate (%)";

            // Create surface
            var surface = Builder.BuildDelaunay(coords);
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin * 1.05, surface.Bounds.ZMax * 0.95, new Color(1, 1, 1, 0.9));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = false;
            surface.WireframeColor = Color.GREEN;
            surface.WireframeColor.Mul(new Color(1, 1, 1, 0.2));

            // Add surface to chart
            chart.Scene.Graph.Add(surface);

            return chart;
        }

        public static Chart GetScanDateFromCSV(Renderer3D renderer3D)
        {
            // Generate data
            TickLabelMap labels = new TickLabelMap();
            List<Coord3d> coords = new List<Coord3d>();
            bool isHeader = true;
            string[] header;
            foreach (var line in File.ReadAllLines(@"C:\Users\汪新智\Desktop\2024-02-20-11.csv"))
            {
                var data = line.Split('\u002C');
                if (isHeader)
                {
                    header = data;
                    //for (int i = 1; i < data.Length; i++)
                    //{
                    //    labels.Register(i, header[i]);
                    //}
                    isHeader = false;
                }
                else
                {
                    //var date = DateTime.ParseExact(data[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //var dateL = date.ToOADate();
                    for (int i = 1; i < data.Length; i++)
                    {
                        coords.Add(new Coord3d(i + 219, double.Parse(data[0]), double.Parse(data[i])));
                    }
                }
            }

            // Create the chart
            Chart chart = new Chart(renderer3D, Quality.Intermediate);
            chart.View.Maximized = false;
            //chart.ViewMode = ViewPositionMode.TOP;
            //chart.View.CameraMode = CameraMode.ORTHOGONAL;
            //chart.AxeLayout.XTickRenderer = labels;
            //chart.AxeLayout.YTickRenderer = new DateTickRenderer("dd/MM/yyyy");
            chart.AxeLayout.XAxeLabel = "Wavelength";
            chart.AxeLayout.YAxeLabel = "Time";
            chart.AxeLayout.ZAxeLabel = "Absorbance";

            // Create surface
            Shape surface = Builder.BuildDelaunay(coords);
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin * 1.05, surface.Bounds.ZMax * 0.95, new Color(1, 1, 1, 0.9));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = false;
            surface.WireframeColor = Color.GREEN;
            surface.WireframeColor.Mul(new Color(1, 1, 1, 0.2));


            // Add surface to chart
            chart.Scene.Graph.Add(surface);

            return chart;
        }

        public static Chart GetScanDateFromDB(Renderer3D renderer3D)
        {
            // 获取数据
            List<double> timeList = new List<double>();
            PtDB.SelectTime("20240219161004", timeList);
            int interval = (int)Math.Ceiling((double)timeList.Count / 40);
            //获取吸收值
            List<double> absorbanceList = new List<double>();

            List<Coord3d> coords = new List<Coord3d>();

            for (int i = 0; i < timeList.Count; i = i + interval)
            {
                absorbanceList.Clear();
                PtDB.ReadTime("20240219161004", timeList[i].ToString(), absorbanceList);
                for (int j = 0; j < absorbanceList.Count; j = j + 2)
                {
                    coords.Add(new Coord3d(j + 200, timeList[i], absorbanceList[j]));
                }
            }

            // Create the chart
            Chart chart = new Chart(renderer3D, Quality.Fastest);
            chart.View.Maximized = false;
            chart.AxeLayout.XAxeLabel = "Wavelength(nm)";
            chart.AxeLayout.YAxeLabel = "Time(min)";
            chart.AxeLayout.ZAxeLabel = "Absorbance(mAu)";
            


            // Create surface
            Shape surface = Builder.BuildDelaunay(coords);
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin * 1.05, surface.Bounds.ZMax * 0.95, new Color(1, 1, 1, 0.8));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = false;
            surface.WireframeColor = Color.CYAN;
            surface.WireframeColor.Mul(new Color(1, 1, 1, 0.5));
            surface.Legend = new ColorbarLegend(surface, chart.View.Axe.getLayout());
            surface.LegendDisplayed = true;
                                 
            // Add surface to chart
            chart.Scene.Graph.Add(surface);

            return chart;
        }

        public static Chart GetDelaunaySurface(Renderer3D renderer3D)
        {
            // Create data
            const int size = 100;
            List<Coord3d> coords = new List<Coord3d>(size);
            float x, y, z;
            var r = new Random(0);

            for (int i = 0; i < size; i++)
            {
                x = (float)(r.NextDouble() - 0.5f);
                y = (float)(r.NextDouble() - 0.5f);
                z = (float)(r.NextDouble() - 0.5f);
                coords.Add(new Coord3d(x, y, z));
            }
            coords = coords.OrderBy(p => p.x).ThenBy(p => p.y).ThenBy(p => p.z).ToList();

            // Create chart
            Chart chart = new Chart(renderer3D, Quality.Nicest);
            chart.View.Maximized = false;

            //chart.View.CameraMode = CameraMode.PERSPECTIVE;

            // Create surface
            var surface = Builder.BuildDelaunay(coords);
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin, surface.Bounds.ZMax, new Color(1, 1, 1, 0.8));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = true;
            surface.WireframeColor = Color.CYAN;
            surface.WireframeColor.Mul(new Color(1, 1, 1, 0.5));
            // Add surface to chart     
            chart.Scene.Graph.Add(surface);

            return chart;
        }

        public static Chart NullChart(Renderer3D renderer3D)
        {
            List<Coord3d> coords = new List<Coord3d>()
            {
                new Coord3d(1, 0, 0),
                new Coord3d(0, 1, 0),
                new Coord3d(0, 0, 1)
            };
            // Create chart
            Chart chart = new Chart(renderer3D, Quality.Nicest);
            chart.View.Maximized = true;
            chart.AxeLayout.XAxeLabel = "Wavelength(nm)";
            chart.AxeLayout.YAxeLabel = "Time(min)";
            chart.AxeLayout.ZAxeLabel = "Absorbance(mAu)";

            Shape surface = Builder.BuildDelaunay(coords);
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin, surface.Bounds.ZMax, new Color(1, 1, 1, 0.8));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = true;
            surface.WireframeColor = Color.CYAN;
            surface.WireframeColor.Mul(new Color(1, 1, 1, 0.5));
            // Add surface to chart     
            chart.Scene.Graph.Add(surface);
            return chart;
        }

        /// <summary>
        /// GroupedLineScatter
        /// </summary>
        /// <param name="renderer3D"></param>
        /// <returns></returns>
        //public static Chart GetGroupedLineScatter(Renderer3D renderer3D)
        //{
        //    // Create data
        //    const float a = 0.50f;
        //    const int size = 4;
        //    var points = new List<Coord3d[]>(size);
        //    var colors = new Color[]
        //    {
        //        new Color(1.0, 0.0, 0.0, a), // RED
        //        new Color(0.0, 1.0, 0.0, a), // GREEN
        //        new Color(0.0, 0.0, 1.0, a), // BLUE
        //        new Color(1.0, 1.0, 0.0, a), // YELLOW
        //        //new Color(1.0, 0.0, 1.0), // MAGENTA
        //        //new Color(0.0, 1.0, 1.0), // CYAN
        //    };

        //    float x, y, z;
        //    var r = new Random(0);

        //    for (int i = 0; i < size; i++)
        //    {
        //        int size2 = r.Next(150);

        //        var points2 = new Coord3d[size2];
        //        for (int j = 0; j < size2; j++)
        //        {
        //            x = (float)(r.NextDouble() - 0.5f);
        //            y = (float)(r.NextDouble() - 0.5f);
        //            z = (float)(r.NextDouble() - 0.5f);
        //            points2[j] = new Coord3d(x, y, z);
        //        }
        //        points.Add(points2);
        //    }

        //    // Create chart
        //    Chart chart = new Chart(renderer3D, Quality.Nicest);
        //    chart.View.Maximized = false;
        //    chart.View.CameraMode = CameraMode.PERSPECTIVE;

        //    // Create scatter
        //    var scatter = new GroupedLineScatter(points, colors);

        //    // Add surface to chart
        //    chart.Scene.Graph.Add(scatter);

        //    return chart;
        //}

        /// <summary>
        /// Scatter
        /// Build a nice scatter to display with cool alpha colors
        /// (alpha 0.25).
        /// </summary>
        /// <param name="renderer3D"></param>
        /// <returns></returns>
        public static Chart GetScatterGraph(Renderer3D renderer3D)
        {
            return GetScatterGraph(renderer3D, 500_000);
        }

        public static Chart GetScatterGraph(Renderer3D renderer3D, int size)
        {
            // Create data
            var points = new Coord3d[size];
            var colors = new Color[size];

            float x, y, z;
            const float a = 0.25f;

            var r = new Random(0);
            for (int i = 0; i < size; i++)
            {
                x = (float)(r.NextDouble() - 0.5f);
                y = (float)(r.NextDouble() - 0.5f);
                z = (float)(r.NextDouble() - 0.5f);
                points[i] = new Coord3d(x, y, z);
                colors[i] = new Color(x, y, z, a);
            }

            // Create chart
            Chart chart = new Chart(renderer3D, Quality.Nicest);
            chart.View.Maximized = false;

            // Create scatter
            var scatter = new Scatter(points, colors);

            // Add surface to chart
            chart.Scene.Graph.Add(scatter);

            return chart;
        }

        public static Chart GetScatterGraphFromDB(Renderer3D renderer3D)
        {
            // 获取数据
            List<double> timeList = new List<double>();
            PtDB.SelectTime("20240219161004", timeList);

            //获取吸收值
            List<double> absorbanceList = new List<double>();
            List<Coord3d> coords = new List<Coord3d>();

            for (int i = 0; i < timeList.Count(); i = i + 1)
            {
                absorbanceList.Clear();
                PtDB.ReadTime("20240219161004", timeList[i].ToString(), absorbanceList);
                for (int j = 0; j < absorbanceList.Count(); j++)
                {
                    coords.Add(new Coord3d(j + 200, timeList[i], absorbanceList[j]));
                }
            }

            int size = timeList.Count * 201;
            // Create data
            var points = new Coord3d[size];
            var colors = new Color[size];

            float x, y, z;
            const float a = 0.25f;

            var r = new Random(0);
            for (int i = 0; i < size; i++)
            {
                x = (float)(r.NextDouble() - 0.5f);
                y = (float)(r.NextDouble() - 0.5f);
                z = (float)(r.NextDouble() - 0.5f);
                points[i] = coords[i];
                colors[i] = new Color(x, y, z, a);
            }

            // Create chart
            Chart chart = new Chart(renderer3D, Quality.Nicest);
            chart.View.Maximized = false;

            // Create scatter
            var scatter = new Scatter(points, colors);

            // Add surface to chart
            chart.Scene.Graph.Add(scatter);

            return chart;
        }
    }
}
