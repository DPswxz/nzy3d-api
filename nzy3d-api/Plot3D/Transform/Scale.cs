using nzy3D.Maths;
using OpenTK.Graphics.OpenGL;

namespace nzy3D.Plot3D.Transform
{

    public class Scale : ITransformer
	{


		private Coord3d _scale;
		public Scale(Coord3d scale)
		{
			_scale = scale;
		}

		public Maths.Coord3d Compute(Maths.Coord3d input)
		{
			return input.multiply(_scale);
		}

		public void Execute()
		{
			GL.Scale(_scale.x, _scale.y, _scale.z);
		}

		public override string ToString()
		{
			return "(Scale)" + _scale.ToString();
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
