namespace CarroRobo.Domain.Extensions
{
	using Enumeradores;

	/// <summary>
	/// Classe base para retorno de um ação ou chamada
	/// </summary>
	public class ResultadoAcao<T>
	{
		/// <summary>
		/// Contrutor Parametrizado
		/// </summary>
		/// <param name="resultado">Resultado da Ação</param>
		/// <param name="mensagem">Mensagem auxiliar</param>
		public ResultadoAcao(ResultadoAcaoEnum resultado, string mensagem)
		{
			Resultado = resultado;
			Mensagem = mensagem;
		}

		/// <summary>
		/// Construtor Padrão, inicializa com sucesso e mensagem "Sucesso"
		/// </summary>
		public ResultadoAcao()
		{
			Resultado = ResultadoAcaoEnum.Sucesso;
			Mensagem = "Sucesso";
		}

		/// <summary>
		/// Indica se o resultado desta ação teve sucesso ou não
		/// </summary>
		public ResultadoAcaoEnum Resultado { get; set; }

		/// <summary>
		/// Mensagem adicional ao resultado
		/// </summary>
		public string Mensagem { get; set; }

		/// <summary>
		/// Objeto de retorno declarado como tipo genérico
		/// </summary>
		public T ObjetoRetorno { get; set; }
	}

	/// <summary>
	/// Classe base para retorno de um ação ou chamada
	/// </summary>
	public class ResultadoAcao
	{
		/// <summary>
		/// Contrutor Parametrizado
		/// </summary>
		/// <param name="resultado">Resultado da Ação</param>
		/// <param name="mensagem">Mensagem auxiliar</param>
		public ResultadoAcao(ResultadoAcaoEnum resultado, string mensagem)
		{
			Resultado = resultado;
			Mensagem = mensagem;
		}

		/// <summary>
		/// Construtor Padrão, inicializa com sucesso e mensagem "Sucesso"
		/// </summary>
		public ResultadoAcao()
		{
			Resultado = ResultadoAcaoEnum.Sucesso;
			Mensagem = "Sucesso";
		}

		/// <summary>
		/// Indica se o resultado desta ação teve sucesso ou não
		/// </summary>
		public ResultadoAcaoEnum Resultado { get; set; }

		/// <summary>
		/// Mensagem adicional ao resultado
		/// </summary>
		public string Mensagem { get; set; }
	}
}