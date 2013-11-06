namespace CarroRobo.Domain.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Enumeradores;
	using Extensions;

	/// <summary>
	/// Objeto principal do carro robo
	/// </summary>
	public class CarroRobo
	{
		#region Propriedades

		/// <summary>
		/// Comunicacao do Carro Robô
		/// </summary>
		public IComunicacaoCarroCobo Comunicacao { get; set; }

		/// <summary>
		/// Tipo de comunicação do carro Robo
		/// </summary>
		public TipoComunicacaoEnum TipoComunicacao { get; set; }

		/// <summary>
		/// Lista de sensores possíveis do carro robô
		/// </summary>
		public List<SensorBase> Sensores { get; set; }

		/// <summary>
		/// Lista de motores do carro Robo
		/// </summary>
		public List<Motor> Motores { get; set; }

		#endregion

		#region Ações

		/// <summary>
		/// Envia valores da lista de motores <see cref="Motores"/> ao microcontrolador independentemente da forma de comunicação
		/// </summary>
		/// <returns>Resultado da Ação, retorna erro no caso de problemas ao enviar</returns>
		public ResultadoAcao AtualizarMotores()
		{
			var resultado = new ResultadoAcao(ResultadoAcaoEnum.Sucesso, string.Empty);

			if (Motores == null)
			{
				resultado.Mensagem = "Lista de motores nula, inicialize a lista com os motores.";
				resultado.Resultado = ResultadoAcaoEnum.Erro;
				return resultado;
			}

			if (Motores.Count == 0)
			{
				resultado.Mensagem = "Lista de motores vazia, adicione motores na lista com os motores.";
				resultado.Resultado = ResultadoAcaoEnum.Erro;
				return resultado;
			}

			foreach (var motor in Motores)
			{
				var codificar = motor.Codificar();

				var resultadoEnviarDados = Comunicacao.EnviarDados(codificar);
				if (resultadoEnviarDados.Resultado != ResultadoAcaoEnum.Sucesso)
				{
					resultado = resultadoEnviarDados;
					return resultado;
				}

				resultado.Mensagem += codificar + "enviado.\r\n";
			}

			return resultado;
		}

		/// <summary>
		/// Obtem Distancia do primeiro sensor ultrassom
		/// </summary>
		/// <returns>Retorna objeto padrao de resultado, caso tenha sucesso, retorna a distancia enviada pelo microcontrolador</returns>
		public ResultadoAcao<decimal> ObterDistanciaUltrassom()
		{
			return ObterDistanciaUltrassom(0);
		}

		/// <summary>
		/// Obtem Distancia do primeiro sensor ultrassom
		/// </summary>
		/// <param name="numeroSensor">Numero do sensor de ultrassom</param>
		/// <returns>Retorna objeto padrao de resultado, caso tenha sucesso, retorna a distancia enviada pelo microcontrolador</returns>
		public ResultadoAcao<decimal> ObterDistanciaUltrassom(int numeroSensor)
		{
			var resultado = new ResultadoAcao<decimal>();

			if (Sensores == null)
			{
				resultado.Mensagem = "Lista de sensores nula, inicialize a lista e adicione um sensor de ULTRASSOM.";
				resultado.Resultado = ResultadoAcaoEnum.Erro;
				resultado.ObjetoRetorno = -1;
				return resultado;
			}

			if (Sensores.Count == 0)
			{
				resultado.Mensagem = "Lista de sensores vazia, adicione um sensor de ULTRASSOM.";
				resultado.Resultado = ResultadoAcaoEnum.Erro;
				resultado.ObjetoRetorno = -1;
				return resultado;
			}

			if (!Sensores.Any(a => a.TipoSensor == TipoSensorEnum.Ultrassom))
			{
				resultado.Mensagem = "Lista de sensores não apresenta nenhum sensor de ULTRASSOM, adicione um sensor de ULTRASSOM.";
				resultado.Resultado = ResultadoAcaoEnum.Erro;
				resultado.ObjetoRetorno = -1;
				return resultado;
			}

			Ultrassom sensor = numeroSensor == 0
								   ? Sensores.First(a => a.TipoSensor == TipoSensorEnum.Ultrassom) as Ultrassom
								   : Sensores.Where(a => a.TipoSensor == TipoSensorEnum.Ultrassom).Cast<Ultrassom>().FirstOrDefault(b => b.NumeroSensor == numeroSensor);

			if (sensor == null)
			{
				resultado.Mensagem = "Não existe este número de sensor de ULTRASSOM.";
				resultado.Resultado = ResultadoAcaoEnum.Erro;
				resultado.ObjetoRetorno = -1;
				return resultado;
			}

			throw new NotImplementedException("Implementar metodo de envio e retorno da distancia");
		}

		/// <summary>
		/// Acende Farol Frontal
		/// </summary>
		/// <returns>Resultado da acao de acender o farol</returns>
		public ResultadoAcao AcenderFarol()
		{
			var resultado = new ResultadoAcao();

			var verificacaoSensoresLed = VerificaSensoresLed();

			if (verificacaoSensoresLed.Resultado == ResultadoAcaoEnum.Erro)
			{
				return verificacaoSensoresLed;
			}

			throw new NotImplementedException("Implementar metodo que acende os farois");
		}

		/// <summary>
		/// Apaga o Farol Frontal
		/// </summary>
		/// <returns>Resultado da acao de apagar o farol</returns>
		public ResultadoAcao ApagarFarol()
		{
			var resultado = new ResultadoAcao();

			var verificacaoSensoresLed = VerificaSensoresLed();

			if (verificacaoSensoresLed.Resultado == ResultadoAcaoEnum.Erro)
			{
				return verificacaoSensoresLed;
			}

			throw new NotImplementedException("Implementar metodo que acende os farois");
		}

		private ResultadoAcao VerificaSensoresLed()
		{
			ResultadoAcao resultado = new ResultadoAcao();
			if (Sensores == null)
			{
				resultado.Mensagem = "Lista de sensores nula, inicialize a lista e adicione um sensor de LED Farol Frontal.";
				resultado.Resultado = ResultadoAcaoEnum.Erro;
				return resultado;
			}

			if (Sensores.Count == 0)
			{
				resultado.Mensagem = "Lista de sensores vazia, adicione um sensor de LED Farol Frontal.";
				resultado.Resultado = ResultadoAcaoEnum.Erro;
				return resultado;
			}

			if (!Sensores.Any(a => a.TipoSensor == TipoSensorEnum.Led))
			{
				resultado.Mensagem =
					"Lista de sensores não apresenta nenhum sensor de LED Farol Frontal, adicione um sensor de LED Farol Frontal.";
				resultado.Resultado = ResultadoAcaoEnum.Erro;
				return resultado;
			}

			return resultado;
		}

		#endregion
	}

	/// <summary>
	/// Interface de comunicação do carro robô
	/// </summary>
	public interface IComunicacaoCarroCobo
	{
		/// <summary>
		/// Enviar comando para o microcontrolador
		/// </summary>
		/// <param name="comando">comando para que será implementado no microcrontrolador</param>
		/// <returns>Resultado do envio</returns>
		ResultadoAcao EnviarDados(string comando);

		/// <summary>
		/// Enviar comando com separador de linha para o microcontrolador
		/// </summary>
		/// <param name="comando">comando para que será implementado no microcrontrolador</param>
		/// <returns>Resultado do envio</returns>
		ResultadoAcao EnviarLinhaDados(string comando);

		/// <summary>
		/// Recebe Dados do tipo de comunicação
		/// </summary>
		/// <returns>Dados retornados</returns>
		ResultadoAcao<string> ReceberDados();
	}
}
