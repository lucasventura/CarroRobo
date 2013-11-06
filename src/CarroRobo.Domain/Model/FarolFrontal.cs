namespace CarroRobo.Domain.Model
{
	using Enumeradores;

	/// <summary>
	/// Farol frontal do carro
	/// </summary>
	public class FarolFrontal : SensorBase
	{
		/// <summary>
		/// Construtor Padrão
		/// </summary>
		public FarolFrontal()
		{
			TipoSensor = TipoSensorEnum.Led;
			Nome = "Farol Frontal";
		}

		/// <summary>
		/// Construtor parametrizado com opção de nome para o sensor
		/// </summary>
		/// <param name="nomeSensor">nome do sensor</param>
		public FarolFrontal(string nomeSensor)
		{
			TipoSensor = TipoSensorEnum.Led;
			Nome = nomeSensor;
		}
	}
}