
namespace lab6
{
	public partial class LightSettingsForm : Form
	{
		private LightSource lightSource;

		public LightSettingsForm(LightSource light)
		{
			InitializeComponent();
			this.lightSource = light;
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (this.DialogResult == DialogResult.OK)
			{
				lightSource.Position = new Point3D(
					(double)numericX.Value,
					(double)numericY.Value,
					(double)numericZ.Value
				);
			}
			base.OnFormClosing(e);
		}

	}
}
