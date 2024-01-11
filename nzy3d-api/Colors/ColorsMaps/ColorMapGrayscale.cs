namespace nzy3D.Colors.ColorMaps
{
    /// <summary>
    /// Creates a new instance of ColorMapGrayscale. 
    ///  A ColorMapWhiteRed objects provides a color for points standing
    ///  between a Zmin and Zmax values.
    /// 
    /// The points standing outside these [Zmin;Zmax] boundaries are assigned
    ///  to the same color than the points standing on the boundaries.
    /// 
    /// The grayscale colormap is a progressive transition from black to white.
    /// </summary>
    public class ColorMapGrayscale : IColorMap
	{
		/// <inheritdoc/>
		public bool Direction { get; set; } = true;

		public Color GetColor(IColorMappable colorable, double v)
		{
			return GetColor(0, 0, v, colorable.ZMin, colorable.ZMax);
		}

		public Color GetColor(IColorMappable colorable, double x, double y, double z)
		{
			return GetColor(x, y, z, colorable.ZMin, colorable.ZMax);
		}

		/// <summary>
		/// Helper function 
		/// </summary>
		private Color GetColor(double x, double y, double z, double zMin, double zMax)
		{
            double rel_value;
            if (z < zMin)
			{
				rel_value = Direction ? 0 : 1;
			}
			else if (z > zMax)
			{
				rel_value = Direction ? 1 : 0;
			}
			else
			{
				if (Direction)
				{
					rel_value = (z - zMin) / (zMax - zMin);
				}
				else
				{
					rel_value = (zMax - z) / (zMax - zMin);
				}
			}
			return new Color(rel_value, rel_value, rel_value);
		}

		/// <summary>
		/// Returns the string representation of this colormap
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public override string ToString()
		{
			return "ColorMapGrayscale";
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
