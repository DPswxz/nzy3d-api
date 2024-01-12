using nzy3D.Events.Keyboard;
using nzy3D.Events.Mouse;

namespace nzy3D.Plot3D.Rendering.Canvas
{

    public interface ICanvas
	{

		/// <summary>
		/// Returns a reference to the held view.
		/// </summary>

		View.View View { get; }

		/// <summary>
		/// Returns the renderer's width, i.e. the display width.
		/// </summary>
		int RendererWidth { get; }

		/// <summary>
		/// Returns the renderer's height, i.e. the display height.
		/// </summary>
		int RendererHeight { get; }

		/// <summary>
		/// Invoked when a user requires the Canvas to be repainted (e.g. a non 3d layer has changed).
		/// </summary>
		void ForceRepaint();

		/// <summary>
		/// Returns an image with the current renderer's size.
		/// </summary>
		System.Drawing.Bitmap Screenshot();

		/// <summary>
		/// Performs all required cleanup when destroying a Canvas.
		/// </summary>
		void Dispose();
		void AddMouseListener(IMouseListener listener);
		void RemoveMouseListener(IMouseListener listener);
		void AddMouseWheelListener(IMouseWheelListener listener);
		void RemoveMouseWheelListener(IMouseWheelListener listener);
		void AddMouseMotionListener(IMouseMotionListener listener);
		void RemoveMouseMotionListener(IMouseMotionListener listener);
		void AddKeyListener(IKeyListener listener);

		void RemoveKeyListener(IKeyListener listener);
	}
}
