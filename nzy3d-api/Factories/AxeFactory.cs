using nzy3D.Maths;
using nzy3D.Plot3D.Primitives.Axes;
using nzy3D.Plot3D.Rendering.View;

namespace nzy3D.Factories
{

    public class AxeFactory
	{
		public static object getInstance(BoundingBox3d box, View view)
		{
			AxeBox axe = new AxeBox(box);
			axe.View = view;
			return axe;
		}
	}
}