using nzy3D.Plot3D.Rendering.Ordering;

namespace nzy3D.Factories
{

    public class OrderingStrategyFactory
    {

        public static AbstractOrderingStrategy getInstance()
        {
            return DEFAULTORDERING;
        }


        public static BarycentreOrderingStrategy DEFAULTORDERING = new BarycentreOrderingStrategy();
    }

}