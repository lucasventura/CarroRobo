namespace CarroRobo.Domain.Model
{
	using System.Globalization;
	using Enumeradores;
	using Extensions;

	/// <summary>
	/// Classe do motor do carro robo
	/// </summary>
	public class Motor
	{
		/// <summary>
		/// Localizacao do motor. <see cref="LocalizacaoMotorEnum"/>
		/// </summary>
		public LocalizacaoMotorEnum LocalizacaoMotor { get; set; }

		/// <summary>
		/// Potencia do motor (PWM) - Valor variável de -255 a 255
		/// </summary>
		public int Potencia { get; set; }

		/// <summary>
		/// Lado do motor. <seealso cref="LadoMotorEnum"/>
		/// </summary>
		public LadoMotorEnum LadoMotor { get; set; }

		/// <summary>
		/// Prepara string que será enviada ao microControlador através do <see cref="TipoComunicacaoEnum"/>
		/// </summary>
		/// <returns>String com protocolo que será interpretado pelo microcontrolador</returns>
		public string Codificar()
		{
			string ladoMotor = LadoMotor.GetDescription();
			string localizacaoMotor = LocalizacaoMotor.GetDescription();
			string sinal = Potencia > 0 ? "+" : "-";
			return string.Format("{0}{1}{2}{3};", localizacaoMotor, ladoMotor, sinal, Potencia.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0'));
		}
	}
}