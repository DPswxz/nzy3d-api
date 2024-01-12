using nzy3D.Maths;

namespace nzy3D.Plot3D.Transform
{

    public interface ITransformer
    {

        // Execute the effective GL transformation held by this class.
        void Execute();
        Coord3d Compute(Coord3d input);
        // Apply the transformations to the input coordinates. (Warning: this method is a utility that may not be implemented.)

    }

}
