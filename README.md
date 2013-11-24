## Carro Robô 

Carro Robô é uma aplicação que integra uma API **.NET** para controle de um carro microcrontrolado atráves de uma interface de comunicação.

Os códigos contidos neste repositório demonstram como utilizar API e como programar o microcontrolador para interação.

Para os exemplos foram utilizados clientes WPF com aquisição de controles do tipo **DirectInput** e o kit Arduino Mega com microcontrolador ATMEL.

As implementações demonstram controle de direção dos ***`Motores`*** , comando para um sensor/atuador, neste caso ***`FarolFrontal`*** e um sensor de distância ***`Ultrassom`*** que envia as distâncias adquiridas pela interface de ***`Comunicação`*** .


----------
----------

### Como Utilizar ###

Utilizando a IDE Microsoft Visual Studio, adicione um projeto. Nos exemplos temos exemplos de Console Application e WPF Application.

- Adicione a referência do seu projeto a DLL abaixo:
> CarroRobo.Domain.dll* 



- Em seguida instancie seu CarroRobo:

> `var carro = new CarroRobo();`


- Defina o tipo de comunicação:

>
    carro.TipoComunicacao = TipoComunicacaoEnum.Serial;			
    ComunicacaoSerial comunicacaoSerial = new ComunicacaoSerial("COM7", 9600); // porta e baudRate
    comunicacaoSerial.AbrirPorta();
    carro.Comunicacao = comunicacaoSerial;

- Adicione os motores setando o lado do motor e a localização:

>
    carro.Motores = new List<Motor>();	
	Motor motor1 = new Motor();
    motor1.LadoMotor = LadoMotorEnum.Esquerda;
    motor1.LocalizacaoMotor = LocalizacaoMotorEnum.Frente;
    motor1.Potencia = 100;
	carro.Motores.Add(motor1);

- Adicione os sensores:

>
	Ultrassom ultrassom = new Ultrassom();
    ultrassom.Nome = "ULTRASSOM";    
    FarolFrontal farolFrontal = new FarolFrontal();
    farolFrontal.Nome = "Farol";    
    carro.Sensores = new List<SensorBase>();
    carro.Sensores.Add(ultrassom);
    carro.Sensores.Add(farolFrontal);


Se sua interface de comunicação estiver pronta e conectada você estará apto para enviar as informações para o seu carro Robo.

- Para atualizar os motores chame o método: 
> carro.AtualizarMotores();


- Para ligar ou desligar o farol frontal chame os métodos:
>  
    Carro.AcenderFarol();
    Carro.ApagarFarol();


----------
----------
### Projetos Futuros ###

A mesma API pode ser replicada para a linguagem JAVA por exemplo, para que o controle do **CarroRobo** possa ser executado em um dispositivo ***ANDROID***.

Para isso basta seguir o protocolo de comunicação com o Arduino. O Arduino espera uma seuqencia de caracteres, desde que esta sequência seja enviada corretamente, ele pode ser controlado por qualquer API.

Este projeto contempla o uso de motores e alguns sensores. Não há nenhuma restrição quanto ao uso de mais sensores do mesmo tipo ou novos sensores, como acelerômetro, câmeras, etc.

A forma de comunicação também pode ser implementada via Wifi. Para isso tanto o Arduino como a API teriam que ser implementadas de ponta a ponta para que funcionem. Na **API .NET** existe uma interface de comunicaçao que deve ser implementada para comunicação.

	/// <summary>
	/// Interface de comunicação do carro robô
	/// </summary>
	public interface IComunicacaoCarroCobo
	{
		/// <summary>
		/// Lista de Dados Recebidos da Comunciacao
		/// </summary>
		ConcurrentQueue<String> DadosRecebidos { get; set; }

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
		/// <param name="sender">Objeto que chamou o metodo</param>
		/// <param name="eventArgs">argumentos do evento</param>
		void ReceberDados(object sender, EventArgs eventArgs);
	}

----------
----------
### Documentação e Demonstrações ###

Na pasta docs, encontram-se o artigo final no modelo IEEE e vídeos e fotos que demonstram o projeto funcionando.

----------
----------
### Protocolo de Comunicação ###


- ***Motor:***

				
 **M** - indica que é uma informação de motor

 **'F'** ou **'T'** : indica se o **motor** é **F**rontal ou **T**raseiro
 
 **'E'** ou **'D'** : indica se o **lado** é **E**squerdo ou **D**ireito 

 **'+'** ou **'-'** : indica o sentido do motor

 **'000'**: valor de 3 dígitos de 0 a 255 com a potência do motor

 **':'** : final da informação




***Exemplos:***

	MFE+120; // seta potencia do motor esquerdo frontal para 120 sentido frente
	MFD-255; // seta potencia do motor direito frontal para 255 sentido traz
	MTE+0;   // seta potencia do motor esquerdo traseiro para 0 sentido frente (desliga motor)
	MTD-100; // seta potencia do motor direito traseiro para 100 sentido traz

- ***Farol:***

>
	FAROLIG; // Liga Farol
	FARODES; // Desliga Farol

- ***Limite Distancia:***

Seta o limite de distância que faz com que o carro não ande para frente.

> `DIST` - indica que é uma informação de distância
> 
> `030` - valor da distância limite que será setado

***Exemplos:***

	DIST100; // seta distância limite para 100cm
	DIST030; // seta distância limite para 30cm

----------
----------