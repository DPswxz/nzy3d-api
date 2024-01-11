namespace nzy3D.Colors
{
    /// <summary>
    /// <see cref=" ISingleColorable"/> objects have a single plain color and a must define a setter for it
    /// </summary>
    /// <remarks></remarks>
    public interface ISingleColorable
	{

		/// <summary>
		/// Get/Set the color
		/// </summary>
		/// <remarks></remarks>
		Color Color { get; set; }
	}
}

