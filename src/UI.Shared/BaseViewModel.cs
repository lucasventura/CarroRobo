namespace UI.Shared
{
    using System.ComponentModel;

    /// <summary>
    /// ViewModel Base
    /// </summary>
    public class BaseViewModel : BaseNotifyPropertyChanged
    {
        private bool _processando;
        private string _statusProcessamento;

        /// <summary>
        /// Indica se está sendo efetuado processamento
        /// </summary>
        public virtual bool Processando
        {
            get
            {
                return _processando;
            }

            set
            {
                _processando = value;

                RaisePropertyChanged("Processando");
            }
        }

        /// <summary>
        /// Mensagem de processamento
        /// </summary>
        public virtual string StatusProcessamento
        {
            get
            {
                return _statusProcessamento;
            }

            set
            {
                _statusProcessamento = value;

                RaisePropertyChanged("StatusProcessamento");
            }
        }

        /// <summary>
        /// Executa um comando async
        /// </summary>
        /// <param name="statusProcessamento">Mensagem de processamento</param>
        /// <param name="work">Handler que será executado em outra thread</param>
        /// <param name="completed">Handler que será executado quando o <see cref="work"/> terminar</param>
        /// <param name="userState">Parametro extra para UserState</param>
        protected void ExecutarComandoAsync(string statusProcessamento, DoWorkEventHandler work, RunWorkerCompletedEventHandler completed, object userState = null)
        {
            Processando = true;
            StatusProcessamento = statusProcessamento;

            var bw = new BackgroundWorker();

            bw.DoWork += work;

            bw.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs args)
                                        {
                                            Processando = false;
                                            completed(sender, args);
                                        };

            bw.RunWorkerAsync(userState);
        }
    }
}