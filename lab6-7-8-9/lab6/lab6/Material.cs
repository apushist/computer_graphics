namespace lab6
{
	public class Material
	{
		public Color DiffuseColor { get; set; } = Color.FromArgb(200,200,100,200);
		public float AmbientIntensity { get; set; } = 0.2f;
		public float DiffuseIntensity { get; set; } = 0.8f;
		public float SpecularIntensity { get; set; } = 0.0f; // Для Гуро?
	}
}
