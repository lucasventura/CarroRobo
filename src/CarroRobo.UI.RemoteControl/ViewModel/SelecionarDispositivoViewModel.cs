namespace CarroRobo.UI.RemoteControl.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using System.IO.Ports;
	using System.Linq;
	using System.Threading;
	using System.Windows.Input;
	using System.Windows.Threading;
	using Domain;
	using Domain.Enumeradores;
	using Domain.Extensions;
	using Domain.Model;
	using Microsoft.DirectX.DirectInput;
	using View;
	using global::UI.Shared;

	/// <summary>
	/// View Model de <see cref="SelecionarDispositivoView"/>
	/// </summary>
	public class SelecionarDispositivoViewModel : BaseViewModel
	{
		private readonly SelecionarDispositivoView _view;
		private Dispositivo _dispositivoSelecionado;
		private JoystickState _state;
		private Device _dispositivo;
		private SerialPort _serialPort;
		private string _velocidadeEsquerda;
		private string _velocidadeDireita;
		private string _sinal;
		private bool _controleXbox;

		/// <summary>
		/// Construtor Padrão
		/// </summary>
		/// <param name="view">view de <see cref="SelecionarDispositivoView"/></param>
		public SelecionarDispositivoViewModel(SelecionarDispositivoView view)
		{
			_view = view;

			ObterDispositivos();

			ConfigurarCarroRobo();
		}

		private void ConfigurarCarroRobo()
		{
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

			FarolFrontal farolFrontal = new FarolFrontal();

			Carro = new CarroRobo();

			Carro.Sensores = new List<SensorBase>();
			Carro.Sensores.Add(farolFrontal);

			Carro.Motores = new List<Motor>();
			Carro.Motores.Add(motor1);
			Carro.Motores.Add(motor2);
			Carro.Motores.Add(motor3);
			Carro.Motores.Add(motor4);
		}

		public CarroRobo Carro { get; set; }

		/// <summary>
		/// Velocidade da roda esquerda
		/// </summary>
		public string VelocidadeEsquerda
		{
			get
			{
				return _velocidadeEsquerda;
			}

			set
			{
				_velocidadeEsquerda = value;
				RaisePropertyChanged("VelocidadeEsquerda");
			}
		}

		/// <summary>
		/// Velocidade da roda Direita
		/// </summary>
		public string VelocidadeDireita
		{
			get
			{
				return _velocidadeDireita;
			}

			set
			{
				_velocidadeDireita = value;
				RaisePropertyChanged("VelocidadeDireita");
			}
		}

		/// <summary>
		/// SInal do motor
		/// </summary>
		public string Sinal
		{
			get
			{
				return _sinal;
			}

			set
			{
				_sinal = value;
				RaisePropertyChanged("Sinal");
			}
		}

		/// <summary>
		/// Dispositivo disponíveis na combo box
		/// </summary>
		public ObservableCollection<Dispositivo> Dispositivos { get; set; }

		/// <summary>
		/// Dispositivo Selecionado
		/// </summary>
		public Dispositivo DispositivoSelecionado
		{
			get
			{
				return _dispositivoSelecionado;
			}

			set
			{
				_dispositivoSelecionado = value;

				RaisePropertyChanged("DispositivoSelecionado");
			}
		}

		/// <summary>
		/// Comando de conectar com dispositivo
		/// </summary>
		public ICommand ConectarCommand
		{
			get
			{
				return new RelayCommand(Conectar, PodeConectar);
			}
		}

		/// <summary>
		/// Comando para Parar Motores
		/// </summary>
		public ICommand PararCommand
		{
			get
			{
				return new RelayCommand(Parar, PodeParar);
			}
		}

		/// <summary>
		/// Estado do COntrole
		/// </summary>
		public JoystickState State
		{
			get
			{
				return _state;
			}

			set
			{
				_state = value;

				RaisePropertyChanged("State");
			}
		}

		private void ObterDispositivos()
		{
			Dispositivos = new ObservableCollection<Dispositivo>();

			DeviceList dispositivosAtivos = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
			foreach (DeviceInstance dispositivosAtivo in dispositivosAtivos)
			{
				Dispositivo dispositivo = new Dispositivo();
				dispositivo.Nome = dispositivosAtivo.InstanceName + " - " + dispositivosAtivo.DeviceType;
				dispositivo.Tipo = dispositivosAtivo.DeviceType;
				dispositivo.Guid = dispositivosAtivo.InstanceGuid;
				Dispositivos.Add(dispositivo);
				DispositivoSelecionado = dispositivo;
			}
		}

		private bool PodeConectar()
		{
			return true;
		}

		private void Conectar()
		{
			ConectarDispositivo();

			Carro.TipoComunicacao = TipoComunicacaoEnum.Serial;
			ComunicacaoSerial comunicacaoSerial = new ComunicacaoSerial("COM7", 9600);
			var result = comunicacaoSerial.AbrirPorta();
			if (result.Resultado == ResultadoAcaoEnum.Erro)
			{
				result = comunicacaoSerial.AbrirPorta();
			}
			Carro.Comunicacao = comunicacaoSerial;

			DispatcherTimer dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Interval = TimeSpan.FromMilliseconds(150);
			dispatcherTimer.Tick += ObterStatusDispositivo;
			dispatcherTimer.Start();

			DispatcherTimer dispatcherTimer2 = new DispatcherTimer();
			dispatcherTimer2.Interval = TimeSpan.FromMilliseconds(50);
			dispatcherTimer2.Tick += EscutarMensagens;
			dispatcherTimer2.Start();
		}

		private void EscutarMensagens(object sender, EventArgs e)
		{
			string dadosNovos;
			Carro.Comunicacao.DadosRecebidos.TryDequeue(out dadosNovos);

			if (string.IsNullOrEmpty(dadosNovos))
			{
				return;
			}

			dadosNovos = dadosNovos.Replace("\r\n", "");

			try
			{
				var i = int.Parse(dadosNovos);
				if (i < 30)
				{
					Carro.AcenderFarol();
					Thread.Sleep(30);
					Carro.ApagarFarol();
					Thread.Sleep(30);
					Carro.AcenderFarol();
				}
			}
			catch (Exception exp)
			{
				Console.WriteLine(exp);
				throw new Exception(exp.Message);
			}
		}

		private bool PodeParar()
		{
			return true;
		}

		private void Parar()
		{
			Carro.PararMotores();
		}

		private void ConectarDispositivo()
		{
			var helper = new System.Windows.Interop.WindowInteropHelper(_view);

			_dispositivo = new Device(DispositivoSelecionado.Guid);
			
			if (_dispositivo.DeviceInformation.InstanceName.Contains("XBOX"))
			{
				_controleXbox = true;
			}

			// Set joystick axis ranges.
			var eixos = _dispositivo.GetObjects(DeviceObjectTypeFlags.Axis);

			// Configura os eixos já para o PWM
			foreach (DeviceObjectInstance doi in eixos)
			{
				InputRange inputRange = new InputRange(0, 255);

				// Eixo do volante
				if (doi.Name.Contains("Wheel axis"))
				{
					inputRange = new InputRange(-255, 255);
				}

				if (doi.Name.Contains("X Axis") && _controleXbox)
				{
					inputRange = new InputRange(-255, 255);
				}

				if (doi.Name.Contains("Z Axis") && _controleXbox)
				{
					inputRange = new InputRange(-255, 255);
				}

				_dispositivo.Properties.SetRange(ParameterHow.ById, doi.ObjectId, inputRange);
			}

			_dispositivo.Properties.AxisModeAbsolute = true;

			EffectList effects = _dispositivo.GetEffects(EffectType.All);

			// _dispositivo.SetCooperativeLevel(helper.Handle, CooperativeLevelFlags.NonExclusive | CooperativeLevelFlags.Background);

			_dispositivo.SetDataFormat(DeviceDataFormat.Joystick);

			// _dispositivo.Properties.BufferSize = 128;
			_dispositivo.Acquire();
		}

		private void ObterStatusDispositivo(object sender, EventArgs e)
		{
			JoystickState state = _dispositivo.CurrentJoystickState;

			State = state;

			EnviarDados();
		}

		private void EnviarDados()
		{
			var botaoApertado = VerificaBotao();

			if (botaoApertado.Resultado != ResultadoAcaoEnum.Sucesso)
			{
				throw new Exception("Erro ao verificar botoes pressionados.. " + botaoApertado.Mensagem);
			}

			bool freioAtivo = VerificaFreioAtivo();

			int frente = ObterVelocidadeFrente();

			var traz = ObterVelocidadeTraz();

			if (freioAtivo)
			{
				frente = 0;
			}
			else
			{
				traz = 0;
			}

			var velo = frente - traz;

			int veloRodaEsquerda = velo;
			int veloRodaDireita = velo;

			if (State.X >= 0)
			{
				int coeficienteRodasDireita = State.X;
				veloRodaDireita = (velo * (255 - coeficienteRodasDireita)) / 255;
			}
			else
			{
				int coeficienteRodasEsquerda = State.X * -1;
				veloRodaEsquerda = (velo * (255 - coeficienteRodasEsquerda)) / 255;
			}

			VelocidadeDireita = veloRodaDireita.ToString();
			VelocidadeEsquerda = veloRodaEsquerda.ToString();

			foreach (var motorDireita in Carro.Motores)
			{
				motorDireita.Potencia = motorDireita.LadoMotor == LadoMotorEnum.Direita
											? veloRodaDireita
											: veloRodaEsquerda;
			}

			var result = Carro.AtualizarMotores();
			if (result.Resultado != ResultadoAcaoEnum.Sucesso)
			{
				throw new Exception("Erro ao atualizar motores.. " + result.Mensagem);
			}
		}

		private ResultadoAcao VerificaBotao()
		{
			ResultadoAcao result = new ResultadoAcao();

			byte[] buttons = State.GetButtons();

			if (!buttons.Any(a => a == 128))
			{
				result.Mensagem = "Nenhum botão pressionado";
				return result;
			}

			// botao A do controle do Xbox
			if (buttons[0] == 128)
			{
				result = Carro.AcenderFarol();
			}

			// botao B do controle do Xbox
			if (buttons[1] == 128)
			{
				result = Carro.ApagarFarol();
			}

			return result;
		}

		private int ObterVelocidadeTraz()
		{
			if (_controleXbox)
			{
				return State.Z;
			}

			return 255 - State.Rz; ;
		}

		private int ObterVelocidadeFrente()
		{
			if (_controleXbox)
			{
				return State.Z * -1;
			}

			return 255 - State.Y;
		}

		private bool VerificaFreioAtivo()
		{
			if (_controleXbox)
			{
				return State.Z > 0;
			}

			return State.Rz < 255;
		}
	}
}