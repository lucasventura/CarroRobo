namespace CarroRobo.Domain.Model
{
	using Enumeradores;

	/// <summary>
	/// Classe base de sensor
	/// </summary>
	public class SensorBase
	{
		/// <summary>
		/// Nome do sensor
		/// </summary>
		public string Nome { get; set; }

		/// <summary>
		/// Tipo do sensor
		/// </summary>
		public TipoSensorEnum TipoSensor { get; set; }
	}
}