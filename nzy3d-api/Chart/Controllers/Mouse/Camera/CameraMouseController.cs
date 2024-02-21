using nzy3D.Chart.Controllers.Camera;
using nzy3D.Chart.Controllers.Thread.Camera;
using nzy3D.Events.Mouse;
using nzy3D.Maths;

namespace nzy3D.Chart.Controllers.Mouse.Camera
{

    public class CameraMouseController : AbstractCameraController, IMouseListener, IMouseMotionListener, IMouseWheelListener
	{

		protected Coord2d _prevMouse;
		protected CameraThreadController _threadController;

		protected float _prevZoom = 1;
		public CameraMouseController()
		{
		}

		public CameraMouseController(Chart chart)
		{
			Register(chart);
		}

		public override void Register(Chart chart)
		{
			base.Register(chart);
			_prevMouse = Coord2d.ORIGIN;
			chart.Canvas.AddMouseListener(this);
			chart.Canvas.AddMouseMotionListener(this);
			chart.Canvas.AddMouseWheelListener(this);
		}

		public override void Dispose()
		{
			foreach (Chart c in _targets) {
				c.Canvas.RemoveMouseListener(this);
				c.Canvas.RemoveMouseMotionListener(this);
				c.Canvas.RemoveMouseWheelListener(this);
			}
			if (!(_threadController == null)) {
				_threadController.Dispose();
				// Instead of threadController.stop();
			}
			base.Dispose();
		}

		/// <summary>
		/// Remove existing threadcontroller (if existing) and add the one passed in parameters as controller.
		/// </summary>
		public void AddSlaveThreadController(CameraThreadController controller)
		{
			RemoveSlaveThreadController();
			_threadController = controller;
		}

		public void RemoveSlaveThreadController()
		{
			if (((_threadController != null))) {
				_threadController.StopT();
				_threadController = null;
			}
		}


		public void MouseClicked(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		/// <summary>
		/// Handles toggle between mouse rotation/auto rotation: double-click starts the animated
		/// rotation, while simple click stops it.
		/// </summary>
		public void MousePressed(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (HandleSlaveThread(false))
			{
				return;
			}
			_prevMouse.x = e.X;
			_prevMouse.y = e.Y;
		}

		/// <summary>
		/// Handles toggle between mouse rotation/auto rotation: double-click starts the animated
		/// rotation, while simple click stops it.
		/// </summary>
		public void MouseDoubleClicked(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (HandleSlaveThread(true))
			{
				return;
			}
			_prevMouse.x = e.X;
			_prevMouse.y = e.Y;
		}

		public bool HandleSlaveThread(bool isDoucleClick)
		{
			if (isDoucleClick) {
				if (_threadController != null) {
					_threadController.Start();
					return true;
				}	
			}	
			if (_threadController != null) {
				_threadController.StopT();
			}
			return false;
		}


		public void MouseReleased(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		//Public Sub MouseDragged(sender As Object, e As System.Windows.Forms.MouseEventArgs) Implements Events.Mouse.IMouseMotionListener.MouseDragged
		//  ' Never raised by Winfo
		//End Sub

		public void MouseMoved(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if (e.Button != System.Windows.Forms.MouseButtons.None)
            {
				Coord2d mouse = new Coord2d(e.X, e.Y);
				// Rotate
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
					Coord2d move = mouse.substract(_prevMouse).divide(100);
					Rotate(move);
				}
				////z-shift
				//if (e.Button == System.Windows.Forms.MouseButtons.Right)
				//{
				//	Coord2d move = mouse.substract(_prevMouse);
				//	if (move.y != 0)
				//	{
				//		Shift((float)(move.y / 250));
				//	}
				//}
				_prevMouse = mouse;
			}
		}

		public void MouseWheelMoved(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_threadController?.StopT();
			if (e.Delta > 0)
			{
				_prevZoom = 0.9f;
			}
			else
			{
				_prevZoom = 1.1f;
			}
			ZoomX(_prevZoom);
			ZoomY(_prevZoom);
			ZoomZ(_prevZoom);
		}
	}
}
