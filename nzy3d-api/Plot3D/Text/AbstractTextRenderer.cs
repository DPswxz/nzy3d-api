using nzy3D.Maths;

namespace nzy3D.Plot3D.Text
{

    public abstract class AbstractTextRenderer : ITextRenderer
    {

        internal Coord2d defScreenOffset;

        internal Coord3d defSceneOffset;
        public AbstractTextRenderer()
        {
            defScreenOffset = new Coord2d();
            defSceneOffset = new Coord3d();
        }

        public abstract void DrawSimpleText(Rendering.View.Camera cam, string s, Coord3d position, Colors.Color color);

        public BoundingBox3d DrawText(Rendering.View.Camera cam, string s, Coord3d position, Align.Halign halign, Align.Valign valign, Colors.Color color)
        {
            return DrawText(cam, s, position, halign, valign, color, defScreenOffset, defSceneOffset);
        }

        public BoundingBox3d DrawText(Rendering.View.Camera cam, string s, Coord3d position, Align.Halign halign, Align.Valign valign, Colors.Color color, Coord2d screenOffset)
        {
            return DrawText(cam, s, position, halign, valign, color, screenOffset, defSceneOffset);
        }

        public abstract BoundingBox3d DrawText(Rendering.View.Camera cam, string s, Coord3d position, Align.Halign halign, Align.Valign valign, Colors.Color color, Coord2d screenOffset, Coord3d sceneOffset);

        public BoundingBox3d DrawText(Rendering.View.Camera cam, string s, Coord3d position, Align.Halign halign, Align.Valign valign, Colors.Color color, Coord3d sceneOffset)
        {
            return DrawText(cam, s, position, halign, valign, color, defScreenOffset, sceneOffset);
        }

    }

}
