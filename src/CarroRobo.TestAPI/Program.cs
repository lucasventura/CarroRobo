namespace CarroRobo.TestAPI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
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
			ComunicacaoSerial comunicacaoSerial = new ComunicacaoSerial("COM7", 9600);
			comunicacaoSerial.AbrirPorta();
			carro.Comunicacao = comunicacaoSerial;

			Motor motor1 = new Motor();
			motor1.LadoMotor = LadoMotorEnum.Esquerda;
			motor1.LocalizacaoMotor = LocalizacaoMotorEnum.Frente;
			motor1.Potencia = 100;
			//motor1.Potencia = 255;
			//motor1.Potencia = -255;

			Motor motor2 = new Motor();
			motor2.LadoMotor = LadoMotorEnum.Direita;
			motor2.LocalizacaoMotor = LocalizacaoMotorEnum.Frente;
			motor2.Potencia = 0;
			motor2.Potencia = 255;
			//motor2.Potencia = -255;
			
			Motor motor3 = new Motor();
			motor3.LadoMotor = LadoMotorEnum.Esquerda;
			motor3.LocalizacaoMotor = LocalizacaoMotorEnum.Traz;
			motor3.Potencia = -1;
			//motor3.Potencia = 255;
			//motor3.Potencia = -255;

			Motor motor4 = new Motor();
			motor4.LadoMotor = LadoMotorEnum.Direita;
			motor4.LocalizacaoMotor = LocalizacaoMotorEnum.Traz;
			motor4.Potencia = -1;
			//motor4.Potencia = 255;
			//motor4.Potencia = -255;

			//var test = motor1.Codificar();
			//Console.WriteLine(test);

			carro.Motores = new List<Motor>();
			carro.Motores.Add(motor1);
			carro.Motores.Add(motor2);
			carro.Motores.Add(motor3);
			carro.Motores.Add(motor4);

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
			Thread.Sleep(2000);
			
		
			motor1.Potencia = 255;
			motor2.Potencia = -255;
			result = carro.AtualizarMotores();
			
			Thread.Sleep(2000);
			
			foreach (var motor in carro.Motores)
			{
				motor.Potencia = 0;
			}
			
			result = carro.AtualizarMotores();
			


		}
	}
}
