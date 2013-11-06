namespace CarroRobo.Domain.Enumeradores
{
	using System.ComponentModel;

	/// <summary>
	/// Enumerador com os possíveis lado do Motor. 
	/// A descrição do motor representa a letra utilizada no protocolo de comunicação com o Microcrontrolador
	/// </summary>
	public enum LadoMotorEnum
	{
		/// <summary>
		/// Enumerador para motores posicionados no lado esquerdo do carro robo
		/// </summary>
		[Description("E")]
		Esquerda,

		/// <summary>
		/// Enumerador para motores posicionados no lado direito do carro robo
		/// </summary>
		[Description("D")]
		Direita,
	}
}