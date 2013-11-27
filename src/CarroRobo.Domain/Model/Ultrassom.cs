namespace CarroRobo.Domain.Model
{
	using Enumeradores;

	/// <summary>
	/// SensorBase de Ultrassom 
	/// </summary>
	public class Ultrassom : SensorBase
	{
		/// <summary>
		/// Construtor Padrão
		/// </summary>
		public Ultrassom()
		{
			TipoSensor = TipoSensorEnum.Ultrassom;
			Nome = "Ultrassom";
		}

		/// <summary>
		/// Construtor parametrizado com opção de nome para o sensor
		/// </summary>
		/// <param name="nomeSensor">nome do sensor</param>
		public Ultrassom(string nomeSensor)
		{
			TipoSensor = TipoSensorEnum.Ultrassom;
			Nome = nomeSensor;
		}

		/// <summary>
		/// Numero do sensor de ultrassom
		/// </summary>
		public int NumeroSensor { get; set; }
		
		/// <summary>
		/// Distancia enviada pelo sensor
		/// </summary>
		public int Distancia { get; set; }
	}
}