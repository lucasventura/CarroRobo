namespace CarroRobo.Domain
{
	using System;
	using System.Collections.Generic;
	using System.IO.Ports;
	using Enumeradores;
	using Extensions;
	using Model;
	using System.Linq;

	/// <summary>
	/// Tipo de Comunicacao entre o carro robô e o controle
	/// </summary>
	public class ComunicacaoSerial : IComunicacaoCarroCobo
	{
		private static SerialPort _serialPort;

		/// <summary>
		/// Construtor de ComunicacaoSerial com possibilidade de setar porta e BaudRate default de 115200
		/// </summary>
		/// <param name="porta">Nome da porta Serial</param>
		public ComunicacaoSerial(string porta)
		{
			InicializarPortaSerial(porta, 115200);
		}

		/// <summary>
		/// Construtor de ComunicacaoSerial com possibilidade de setar porta e baudRate
		/// </summary>
		/// <param name="porta">Nome da porta Serial</param>
		/// <param name="baudRate">BaudRate da porta serial</param>
		public ComunicacaoSerial(string porta, int baudRate)
		{
			InicializarPortaSerial(porta, baudRate);
		}

		/// <summary>
		/// Abre porta serial para transmitir os dados
		/// </summary>
		/// <returns>Retorna Sucesso caso porta esteja ou seja aberta</returns>
		public ResultadoAcao AbrirPorta()
		{
			var resultadoAcao = new ResultadoAcao(ResultadoAcaoEnum.Sucesso, string.Format("Serial {0} aberta com sucesso", _serialPort.PortName));

			if (_serialPort.IsOpen)
			{
				resultadoAcao.Mensagem = string.Format("Porta {0} já está aberta", _serialPort.PortName);
			}

			try
			{
				_serialPort.Open();
			}
			catch (Exception exception)
			{
				resultadoAcao.Mensagem = exception.Message;

				resultadoAcao.Resultado = ResultadoAcaoEnum.Erro;
			}

			return resultadoAcao;
		}

		/// <summary>
		/// Enviar comando para o microcontrolador
		/// </summary>
		/// <param name="comando">comando para que será implementado no microcrontrolador</param>
		/// <returns>Resultado do envio</returns>
		public ResultadoAcao EnviarDados(string comando)
		{
			ResultadoAcao resultadoAcao = VerificaPortaSerialIsOpen();

			if (resultadoAcao.Resultado == ResultadoAcaoEnum.Sucesso)
			{
				_serialPort.Write(comando);
			}

			return resultadoAcao;
		}

		/// <summary>
		/// Enviar comando com separador de linha para o microcontrolador
		/// </summary>
		/// <param name="comando">comando para que será implementado no microcrontrolador</param>
		/// <returns>Resultado do envio</returns>
		public ResultadoAcao EnviarLinhaDados(string comando)
		{
			ResultadoAcao resultadoAcao = VerificaPortaSerialIsOpen();

			if (resultadoAcao.Resultado == ResultadoAcaoEnum.Sucesso)
			{
				_serialPort.WriteLine(comando);
			}

			return resultadoAcao;
		}

		/// <summary>
		/// Recebe Dados do tipo de comunicação
		/// </summary>
		/// <returns>Dados retornados</returns>
		public ResultadoAcao<string> ReceberDados()
		{
			var resultadoAcao = new ResultadoAcao<string>();

			resultadoAcao.ObjetoRetorno = _serialPort.ReadLine();

			return resultadoAcao;
		}

		/// <summary>
		/// Lista de Portas Seriais Disponíveis
		/// </summary>
		public static List<string> Portas { get { return SerialPort.GetPortNames().ToList(); } }

		private static void InicializarPortaSerial(string porta, int baudRate)
		{
			_serialPort = new SerialPort();
			_serialPort.PortName = porta;
			_serialPort.BaudRate = baudRate;
		}

		private static ResultadoAcao VerificaPortaSerialIsOpen()
		{
			var resultadoAcao = new ResultadoAcao();

			if (!_serialPort.IsOpen)
			{
				resultadoAcao.Mensagem = "Porta Serial não aberta. Por favor, chame o método AbrirPorta().";
				resultadoAcao.Resultado = ResultadoAcaoEnum.Erro;
			}

			return resultadoAcao;
		}
	}
}
