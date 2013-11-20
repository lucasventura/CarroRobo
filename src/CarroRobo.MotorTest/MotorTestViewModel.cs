namespace CarroRobo.MotorTest
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Threading;
	using System.Windows;
	using System.Windows.Input;
	using Domain;
	using Domain.Enumeradores;
	using Domain.Model;
	using UI.Shared;

	public class MotorTestViewModel : BaseViewModel
	{
		private readonly MotorTestView _view;

		public MotorTestViewModel(MotorTestView view)
		{
			_view = view;

			Motores = new ObservableCollection<MotorDto>();

			Motor motor1 = new Motor();
			motor1.LadoMotor = LadoMotorEnum.Esquerda;
			motor1.LocalizacaoMotor = LocalizacaoMotorEnum.Frente;
			motor1.Potencia = 0;

			Motor motor2 = new Motor();
			motor2.LadoMotor = LadoMotorEnum.Direita;
			motor2.LocalizacaoMotor = LocalizacaoMotorEnum.Frente;
			motor2.Potencia = 0;

			Motor motor3 = new Motor();
			motor3.LadoMotor = LadoMotorEnum.Esquerda;
			motor3.LocalizacaoMotor = LocalizacaoMotorEnum.Traz;
			motor3.Potencia = 0;

			Motor motor4 = new Motor();
			motor4.LadoMotor = LadoMotorEnum.Direita;
			motor4.LocalizacaoMotor = LocalizacaoMotorEnum.Traz;
			motor4.Potencia = 0;

			Carro = new CarroRobo();
			Carro.TipoComunicacao = TipoComunicacaoEnum.Serial;

			ComunicacaoSerial comunicacaoSerial = new ComunicacaoSerial("COM7", 9600);
			comunicacaoSerial.AbrirPorta();
			Carro.Comunicacao = comunicacaoSerial;

			Carro.Motores = new List<Motor>();
			Carro.Motores.Add(motor1);
			Carro.Motores.Add(motor2);
			Carro.Motores.Add(motor3);
			Carro.Motores.Add(motor4);

			MotorDto motorDto1 = new MotorDto(motor1, Carro);
			MotorDto motorDto2 = new MotorDto(motor2, Carro);
			MotorDto motorDto3 = new MotorDto(motor3, Carro);
			MotorDto motorDto4 = new MotorDto(motor4, Carro);
			Motores.Add(motorDto1);
			Motores.Add(motorDto2);
			Motores.Add(motorDto3);
			Motores.Add(motorDto4);
		}

		public CarroRobo Carro { get; set; }

		public ObservableCollection<MotorDto> Motores { get; set; }

		public ICommand EnviarTodasPotenciasCommand
		{
			get
			{
				return new RelayCommand<MotorDto>(EnviarPotencias, PodeEnviarTodasPotencias);
			}
		}

		public ICommand PararMotoresCommand
		{
			get
			{
				return new RelayCommand<MotorDto>(PararMotores, PodePararMotores);
			}
		}

		private bool PodePararMotores(MotorDto obj)
		{
			return true;
		}

		private void PararMotores(MotorDto obj)
		{
			foreach (var motor in Motores)
			{
				motor.Potencia = 0;
			}
		}

		private bool PodeEnviarTodasPotencias(MotorDto obj)
		{
			return true;
		}

		private void EnviarPotencias(MotorDto obj)
		{
			foreach (var motorDto in Motores)
			{
				EnviarPotencia(motorDto);
			}
		}

		public ICommand EnviarPotenciaCommand
		{
			get
			{
				return new RelayCommand<MotorDto>(EnviarPotencia, PodeEnviarPotencia);
			}
		}

		private bool PodeEnviarPotencia(MotorDto obj)
		{
			return true;
		}

		private void EnviarPotencia(MotorDto obj)
		{
			Carro.Comunicacao.EnviarDados(obj.Motor.Codificar());
		}
	}

	public class MotorDto : BaseViewModel
	{
		private readonly CarroRobo _carro;

		public MotorDto(Motor motor, CarroRobo carro)
		{
			_carro = carro;
			Motor = motor;
		}

		public Motor Motor { get; set; }

		public int Potencia
		{
			get { return Motor.Potencia; }
			set
			{
				Thread.Sleep(150);
				Motor.Potencia = value;
				var codificar = Motor.Codificar();
				Console.WriteLine(codificar);
				_carro.Comunicacao.EnviarDados(codificar);
				RaisePropertyChanged("Potencia");
			}
		}
	}

}
