namespace nzy3D.Colors
{
    public interface IColorMappable
	{

		/// <summary>
		/// Get/Set the lower value boundary for a <see cref="colors.ColorMaps.IColorMap"/>.
		/// </summary>
		double ZMin { get; set; }

		/// <summary>
		/// Get/Set the upper value boundary for a <see cref="colors.ColorMaps.IColorMap"/>.
		/// </summary>
		double ZMax { get; set; }
	}
}
