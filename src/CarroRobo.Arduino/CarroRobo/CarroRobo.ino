#include <NewPing.h>

/* -------------------------Definicao dos Pinos---------------------------- */
#define RearRightForwardPin 9
#define RearRightBackwardPin 8
#define RearLeftForwardPin 5
#define RearLeftBackwardPin 4
#define FrontRightForwardPin 7
#define FrontRightBackwardPin 6
#define FrontLeftForwardPin 3
#define FrontLeftBackwardPin 2
#define RearRightPotenciaPin 12
#define RearLeftPotenciaPin 11
#define FrontRightPotenciaPin 13
#define FrontLeftPotenciaPin 10
/* ------------------------------------------------------------------------ */

#define LED1Pin 23
#define LED2Pin 25
#define LED3Pin 27
#define LED4Pin 22
#define LED5Pin 24
#define LED6Pin 26
 
#define trigPin 29
#define echoPin 28
#define MAX_DISTANCE 400
long distancia = 0;
int limiteDistancia = 30;
NewPing sonar(trigPin, echoPin, MAX_DISTANCE);
 
/* ---------------Variaveis que serao enviadas aos pinos------------------- */
int RearRightForward;
int RearRightBackward;
int RearLeftForward;
int RearLeftBackward;
int FrontRightForward;
int FrontRightBackward;
int FrontLeftForward;
int FrontLeftBackward;
int RearRightPotencia;
int RearLeftPotencia;
int FrontRightPotencia;
int FrontLeftPotencia;
/* ------------------------------------------------------------------------ */

#define BUFFERSIZE 8
char inBuffer[BUFFERSIZE];

/* ------------------------------------------------------------------------ */
void setup()
{  
	// Inicializacao das portas seriais (0 - utilizada para debug, 1 - Troca de dados do bluetooth)
	Serial.begin(115200); 
	Serial1.begin(9600);  

	// Seta o modo de opera??o dos pinos
	pinMode(RearRightForwardPin, OUTPUT);
	pinMode(RearRightBackwardPin, OUTPUT);
	pinMode(RearLeftForwardPin, OUTPUT);
	pinMode(RearLeftBackwardPin, OUTPUT);
	pinMode(FrontRightForwardPin, OUTPUT);
	pinMode(FrontRightBackwardPin, OUTPUT);
	pinMode(FrontLeftForwardPin, OUTPUT);
	pinMode(FrontLeftBackwardPin, OUTPUT);
	pinMode(RearRightPotenciaPin, OUTPUT);
	pinMode(RearLeftPotenciaPin, OUTPUT);
	pinMode(FrontRightPotenciaPin, OUTPUT);
	pinMode(FrontLeftPotenciaPin, OUTPUT);

	pinMode(LED1Pin, OUTPUT);
	pinMode(LED2Pin, OUTPUT);
	pinMode(LED3Pin, OUTPUT);
	pinMode(LED4Pin, OUTPUT);
	pinMode(LED5Pin, OUTPUT);
	pinMode(LED6Pin, OUTPUT);

	pinMode(trigPin, OUTPUT);
	pinMode(echoPin, INPUT);
	// Para os motores ao primeiro momento
	PararMotores();    
}

void loop()
{       
    long distanciaTemp = ObterDistancia();  
    if(distanciaTemp >0)
    {
      distancia = distanciaTemp;
      String str = 
      Serial1.println(distancia); 
    }
   
    ImprimirVariavelFormatada("distancia: ", distancia); 
 
  boolean resultado = LerMensagem();
  Serial1.flush();
  memset(inBuffer, 0, sizeof(BUFFERSIZE));   
}

boolean LerMensagem()
{
  int result = Serial1.readBytesUntil(';', inBuffer, BUFFERSIZE);
  if(result <= 0)
  {
    return false;
  }
  
  //ImprimirVariavelFormatada("result: ", result);
  String str (inBuffer);    
  // ImprimirVariavelFormatada("Buffer: ", str); 
    
  // mensagem do tipo Motor
  if(str.startsWith("M"))
  {
      String motor = str.substring(1,3);
      String sentidoStr = str.substring(3,4);
      boolean sentido = sentidoStr == "-" ? LOW : HIGH;	 
      int potencia = str.substring(4,7).toInt();	 
      
       /*ImprimirVariavelFormatada("Motor: ", motor);
       ImprimirVariavelFormatada("sentidoStr: ",sentidoStr);
       ImprimirVariavelFormatada("sentido: ",sentido);
       ImprimirVariavelFormatada("sentidoI: ",!sentido);
       ImprimirVariavelFormatada("potencia: ",potencia);*/                
      
      if(distancia > 0 && distancia <= limiteDistancia && sentido == HIGH)
      {       
        potencia = 0;          
      }
      
      // frontal esquerdo
      if(motor == "FE")
      {        
        digitalWrite( FrontLeftForwardPin, sentido );
        digitalWrite( FrontLeftBackwardPin, !sentido );
        analogWrite(  FrontLeftPotenciaPin, potencia );
        return true;
      }	
     
     // frontal direito
      if(motor == "FD")
      {        
        digitalWrite( FrontRightForwardPin, sentido );
        digitalWrite( FrontRightBackwardPin, !sentido );
        analogWrite(  FrontRightPotenciaPin, potencia );
        return true;
      }
     
      //traseiro esquerdo
      if(motor == "TE")
      {       
        digitalWrite( RearLeftForwardPin, sentido );
        digitalWrite( RearLeftBackwardPin, !sentido );
        analogWrite(  RearLeftPotenciaPin, potencia );
        return true;
      }
     
      //traseiro direito
      if(motor == "TD")
      {        
        digitalWrite( RearRightForwardPin, sentido );
        digitalWrite( RearRightBackwardPin, !sentido );
        analogWrite(  RearRightPotenciaPin, potencia );
        return true;
      }	
      
      ImprimirVariavelFormatada("Invalido", "");      
      return false;
  }
  
  if(str.startsWith("F"))
  {
    String parseFarol = str.substring(0,7);
    ImprimirVariavelFormatada("Farol", parseFarol);
    if(parseFarol == "FAROLIG")
    {
      digitalWrite( LED1Pin, HIGH );
      digitalWrite( LED2Pin, HIGH );
      digitalWrite( LED3Pin, HIGH );
      digitalWrite( LED4Pin, HIGH );
      digitalWrite( LED5Pin, HIGH );
      digitalWrite( LED6Pin, HIGH );
      return true;
    }
    else if (parseFarol == "FARODES")
    {
      digitalWrite( LED1Pin, LOW );
      digitalWrite( LED2Pin, LOW );
      digitalWrite( LED3Pin, LOW );
      digitalWrite( LED4Pin, LOW );
      digitalWrite( LED5Pin, LOW );
      digitalWrite( LED6Pin, LOW );
      return true;
    }
    
    return false;    
  }
  
  if(str.substring(0,4) == "DIST")
  {
     limiteDistancia = str.substring(4,7).toInt();
  }      
 
  
  ImprimirVariavelFormatada("Nao e motor", "");
  return false;
}

int getInt(String text, int size)
{
  char temp[size+1];
  text.toCharArray(temp, size+1);
  int x = atoi(temp);
  return x;
} 

// Para os motores setando as direcoes para low e o pwm para 0
void PararMotores()
{
	digitalWrite( RearRightForwardPin, LOW );
	digitalWrite( RearRightBackwardPin, LOW );
	digitalWrite( RearLeftForwardPin, LOW );
	digitalWrite( RearLeftBackwardPin, LOW );
	digitalWrite( FrontRightForwardPin, LOW );
	digitalWrite( FrontRightBackwardPin, LOW );
	digitalWrite( FrontLeftForwardPin, LOW );
	digitalWrite( FrontLeftBackwardPin, LOW );
	analogWrite( RearRightPotenciaPin, 0 );
	analogWrite( RearLeftPotenciaPin, 0 );
	analogWrite( FrontRightPotenciaPin, 0 );
	analogWrite( FrontLeftPotenciaPin, 0 );
}

long ObterDistancia ()
{
  return sonar.ping_cm(); 
}

void  ImprimirVariavelFormatada (String titulo, String texto)
{
    Serial.print(titulo);
    Serial.println(texto);
}

void  ImprimirVariavelFormatada  (String titulo, int valor)
{
    Serial.print(titulo);
    Serial.println(valor);
}

void  ImprimirVariavelFormatada  (String titulo, boolean valor)
{
    Serial.print(titulo);
    Serial.println(valor);
}

void  ImprimirVariavelFormatada  (String titulo, long valor)
{
    Serial.print(titulo);
    Serial.println(valor);
}
