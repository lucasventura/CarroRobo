namespace CarroRobo.Domain.Model
{
	using System;
	using Enumeradores;

	/// <summary>
	/// Farol frontal do carro
	/// </summary>
	public class FarolFrontal : SensorBase
	{
		private bool _ligado;

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

		/// <summary>
		/// Indica de o Farol está ligado ou não
		/// </summary>
		public bool Ligado
		{
			get { return _ligado; }
		}

		/// <summary>
		/// Liga o Farol Frontal
		/// </summary>
		public void LigarFarol()
		{
			_ligado = true;
		}

		/// <summary>
		/// Desliga o Farol frontal
		/// </summary>
		public void DesligarFarol()
		{
			_ligado = false;
		}

		/// <summary>
		/// Prepara string que será enviada ao microControlador através do <see cref="TipoComunicacaoEnum"/>
		/// </summary>
		/// <returns>String com protocolo que será interpretado pelo microcontrolador</returns>
		public string Codificar()
		{
			return Ligado ? "FAROLIG;" : "FARODES;";
		}
	}
}