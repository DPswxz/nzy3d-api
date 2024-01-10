using nzy3D.Chart;
using nzy3D.Colors;
using nzy3D.Colors.ColorMaps;
using nzy3D.Maths;
using nzy3D.Plot3D.Builder;
using nzy3D.Plot3D.Builder.Concrete;
using nzy3D.Plot3D.Primitives;
using nzy3D.Plot3D.Primitives.Axes.Layout.Renderers;
using nzy3D.Plot3D.Rendering.Canvas;
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
            chart.View.CameraMode = CameraMode.PERSPECTIVE;

            // Create a range for the graph generation
            Range range = new Range(-150, 150);
            const int steps = 50;

            // Build a nice surface to display with cool alpha colors 
            // (alpha 0.8 for surface color and 0.5 for wireframe)
            Shape surface = Builder.buildOrthonomal(new OrthonormalGrid(range, steps, range, steps), new MyMapper());
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
            var surface = Builder.buildDelaunay(coords);
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin * 1.05, surface.Bounds.ZMax * 0.95, new Color(1, 1, 1, 0.9));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = false;
            surface.WireframeColor = Color.GREEN;
            surface.WireframeColor.Mul(new Color(1, 1, 1, 0.2));

            // Add surface to chart
            chart.Scene.Graph.Add(surface);

            return chart;
        }

        public static Chart GetScanDate(Renderer3D renderer3D)
        {
            // Generate data
            var labels = new TickLabelMap();
            List<Coord3d> coords = new List<Coord3d>();
            bool isHeader = true;
            string[] header;
            foreach (var line in File.ReadAllLines(@"C:\Users\汪新智\Desktop\2024-01-09-14.csv"))
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
                        coords.Add(new Coord3d(i + 200, double.Parse(data[0]), double.Parse(data[i])));
                    }
                }
            }



            // Create the chart
            Chart chart = new Chart(renderer3D, Quality.Fastest);
            chart.View.Maximized = false;
            chart.View.CameraMode = CameraMode.ORTHOGONAL;
            //chart.AxeLayout.YTickRenderer = new DateTickRenderer("dd/MM/yyyy");
            chart.AxeLayout.YAxeLabel = "Time";
            //chart.AxeLayout.XTickRenderer = labels;
            chart.AxeLayout.XAxeLabel = "Wavelength";
            chart.AxeLayout.ZAxeLabel = "Absorbance";

            // Create surface
            Shape surface = Builder.buildDelaunay(coords);
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin * 1.05, surface.Bounds.ZMax * 0.95, new Color(1, 1, 1, 0.9));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = false;
            surface.WireframeColor = Color.GREEN;
            surface.WireframeColor.Mul(new Color(1, 1, 1, 0.2));

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
            chart.View.CameraMode = CameraMode.PERSPECTIVE;

            // Create surface
            var surface = Builder.buildDelaunay(coords);
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
            chart.View.CameraMode = CameraMode.PERSPECTIVE;

            // Create scatter
            var scatter = new Scatter(points, colors);

            // Add surface to chart
            chart.Scene.Graph.Add(scatter);

            return chart;
        }
    }
}
