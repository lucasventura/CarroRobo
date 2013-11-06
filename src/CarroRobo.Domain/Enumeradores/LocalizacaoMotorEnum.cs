namespace CarroRobo.Domain.Enumeradores
{
	using System.ComponentModel;

	/// <summary>
	/// Enumerador com as localizações dos motores. No caso de 2 motores, setar como motores traseiros (Traz).
	/// A descrição da Localizacao do motor representa a letra utilizada no protocolo de comunicação com o Microcrontrolador
	/// </summary>
	public enum LocalizacaoMotorEnum
	{
		/// <summary>
		/// Enumerador para motores localizados na frente do carro robo
		/// </summary>
		[Description("F")]
		Frente,

		/// <summary>
		/// Enumerador para motores localizados na parte de traz do carro robo
		/// </summary>
		[Description("T")]
		Traz,
	}
}