namespace lab6
{
	public class Material
	{
		public Color DiffuseColor { get; set; } = Color.FromArgb(200,25,25);
		public float AmbientIntensity { get; set; } = 0.2f;
		public float DiffuseIntensity { get; set; } = 1.9f;
	}
}
