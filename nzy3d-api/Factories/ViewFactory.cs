using nzy3D.Chart;
using nzy3D.Plot3D.Rendering.Canvas;
using nzy3D.Plot3D.Rendering.View;

namespace nzy3D.Factories
{

    public class ViewFactory
	{

		public static View getInstance(Scene scene, ICanvas canvas, Quality quality)
		{
			return new ChartView(scene, canvas, quality);
		}

	}

}
