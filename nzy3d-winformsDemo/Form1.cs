using nzy3D.Chart;
using nzy3D.Chart.Controllers.Mouse.Camera;
using nzy3D.Chart.Controllers.Thread.Camera;
using nzy3D.Maths;
using nzy3D.Plot3D.Primitives.Axes.Layout;
using nzy3D.Plot3D.Rendering.View;
using System;
using System.Windows.Forms;

namespace nzy3d_winformsDemo
{
    public partial class Form1 : Form
    {

        private CameraThreadController cameraController;
        private IAxeLayout axeLayout;

        public Form1()
        {
            InitializeComponent();
            //InitRenderer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitRenderer();
        }

        private void InitRenderer()
        {

            // Create the Renderer 3D control.
            //Renderer3D myRenderer3D = new Renderer3D();

            // Add the Renderer control to the panel
            // mainPanel.Controls.Clear();
            //mainPanel.Controls.Add(myRenderer3D);

            Chart chart = ChartsHelper.GetScanDateFromDB(myRenderer3D);

            axeLayout = chart.AxeLayout;

            // Create a mouse control
            CameraMouseController mouse = new CameraMouseController();
            mouse.AddControllerEventListener(myRenderer3D);
            chart.addController(mouse);

            // This is just to ensure code is reentrant (used when code is not called in Form_Load but another reentrant event)
            DisposeBackgroundThread();

            // Create a thread to control the camera based on mouse movements
            cameraController = new CameraThreadController();
            cameraController.AddControllerEventListener(myRenderer3D);
            mouse.AddSlaveThreadController(cameraController);
            chart.addController(cameraController);
            cameraController.Start();

            // Associate the chart with current control
            myRenderer3D.SetView(chart.View);

            this.Refresh();
        }

        private void DisposeBackgroundThread()
        {
            if (cameraController != null)
            {
                cameraController.Dispose();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeBackgroundThread();
        }

    }
}
