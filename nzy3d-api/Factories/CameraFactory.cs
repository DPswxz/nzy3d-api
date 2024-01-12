using nzy3D.Maths;
using nzy3D.Plot3D.Rendering.View;

namespace nzy3D.Factories
{

    public class CameraFactory
    {

        public static Camera getInstance(Coord3d center)
        {
            return new Camera(center);
        }

    }

}