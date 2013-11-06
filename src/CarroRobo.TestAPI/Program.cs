namespace CarroRobo.TestAPI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CarroRobo.Domain;
	using Domain.Enumeradores;
	using Domain.Model;

	class Program
	{
		static void Main(string[] args)
		{
			var carro = new CarroRobo();
			carro.TipoComunicacao = TipoComunicacaoEnum.Serial;

			var portas = ComunicacaoSerial.Portas;
			ComunicacaoSerial comunicacaoSerial = new ComunicacaoSerial(portas.First());
			comunicacaoSerial.AbrirPorta();
			carro.Comunicacao = comunicacaoSerial;

			Motor motor1 = new Motor();
			motor1.LadoMotor = LadoMotorEnum.Direita;
			motor1.LocalizacaoMotor = LocalizacaoMotorEnum.Frente;
			motor1.Potencia = 100;

			var test = motor1.Codificar();
			Console.WriteLine(test);

			carro.Motores = new List<Motor>();
			carro.Motores.Add(motor1);

			Ultrassom ultrassom = new Ultrassom();
			ultrassom.Nome = "ULTRASSOM";

			FarolFrontal farolFrontal = new FarolFrontal();
			farolFrontal.Nome = "Farol";

			carro.Sensores = new List<SensorBase>();
			carro.Sensores.Add(ultrassom);
			carro.Sensores.Add(farolFrontal);

			var result = carro.AtualizarMotores();
			Console.WriteLine(result.Mensagem);
			Console.WriteLine(carro);
		}
	}
}
