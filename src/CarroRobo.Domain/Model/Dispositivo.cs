namespace CarroRobo.Domain.Model
{
	using System;
	using Microsoft.DirectX.DirectInput;

	// using SlimDX.DirectInput;

	/// <summary>
	/// Dispositivo para controle
	/// </summary>
	public class Dispositivo
	{
		/// <summary>
		/// Nome do dispositivo
		/// </summary>
		public string Nome { get; set; }

		/// <summary>
		/// Tipo do Controle
		/// </summary>
		public DeviceType Tipo { get; set; }
		
		/// <summary>
		/// Guid do dispositivo
		/// </summary>
		public Guid Guid { get; set; }

		/// <summary>
		/// Ovveride para aparecer nome do dispositivo em combo
		/// </summary>
		/// <returns>Nome do Dispositivo</returns>
		public override string ToString()
		{
			return Nome;
		}
	}
}