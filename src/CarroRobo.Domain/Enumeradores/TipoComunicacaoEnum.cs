namespace CarroRobo.Domain.Enumeradores
{
	/// <summary>
	/// Enumerador com os possíveis modo de comunicacao do carro robo. 
	/// </summary>
	public enum TipoComunicacaoEnum
	{
		/// <summary>
		/// Comunicação Serial via bluetooth
		/// </summary>
		Serial,

		/// <summary>
		/// Comunicao Wifi com protocolo WebSocket
		/// </summary>
		WebSocket
	}
}