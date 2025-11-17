namespace lab6
{
	public class Material
	{
		public Color DiffuseColor { get; set; } = Color.FromArgb(200,200,100,200);
		public float AmbientIntensity { get; set; } = 0.4f;
		public float DiffuseIntensity { get; set; } = 0.9f;
	}
}
