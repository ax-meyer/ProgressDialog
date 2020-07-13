using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;

namespace ProgressDialog
{
    /// <summary>Basic class for progress status. Provides everything that is needed for a simple, working implementation.</summary>
    public class ProgressStatus : INotifyPropertyChanged, IProgressStatus
    {
        private int progressPercent = 0;
        private string message = "Waiting for task to start...";
        private bool isFinished;
        private bool IsRunning(object obj) => !(isFinished || IsCancelled);

        /// <summary>Gets CancellationTokenSource to use to cancel the async function.</summary>
        private CancellationTokenSource CTS { get; set; } = new CancellationTokenSource();

        public CancellationToken CT => CTS.Token;

        /// <summary>Command executed when cancel button is clicked.</summary>
        public ICommand CancelCommand => new RelayCommand(Cancel, IsRunning);

        /// <summary>Gets a value indicating whether the associated task was cancelled.</summary>
        public bool IsCancelled => CTS.IsCancellationRequested;

        /// <summary>Event published when the associated task is finished.</summary>
        public event Action<IProgressStatus> Finished;

        /// <summary>Event published when the associated task is cancelled.</summary>
        public event Action<IProgressStatus> Cancelled;

        /// <summary>Event published when the progress is updated.</summary>
        public event Action<IProgressStatus> ProgessUpdated;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Gets or sets a value indicating whether the associated task is finished.</summary>
        public bool IsFinished
        {
            get => isFinished;
            set
            {
                isFinished = value;
                progressPercent = 100;
                RaisePropertyChanged(nameof(IsFinished));
                RaisePropertyChanged(nameof(ProgressPercent));
                ((RelayCommand)CancelCommand).OnCanExecuteChanged();
                Finished?.Invoke(this);
            }
        }

        /// <summary>Gets message to be displayed in ProgressDialog.</summary>
        public string Message
        {
            get => message;
            private set
            {
                message = value;
                RaisePropertyChanged(nameof(Message));
            }
        }

        /// <summary>Gets progress in percent shown by ProgressBar in ProgressDialog.</summary>
        public int ProgressPercent
        {
            get => progressPercent;
            private set
            {
                progressPercent = value;

                if (isFinished == true)
                {
                    isFinished = false;
                    RaisePropertyChanged(nameof(IsFinished));
                }

                RaisePropertyChanged(nameof(ProgressPercent));
            }
        }

        /// <summary>Update ProgressDialog.</summary>
        /// <param name="message">New message to be shown.</param>
        /// <param name="progressPercent">New progress level to be shown.</param>
        public void Update(string message, int progressPercent)
        {
            Message = message;
            if (progressPercent >= 100)
            {
                IsFinished = true;
            }
            ProgressPercent = progressPercent;
            ProgessUpdated?.Invoke(this);
        }

        private void Cancel()
        {
            if (!IsFinished)
            {
                CTS.Cancel();
                RaisePropertyChanged(nameof(IsCancelled));
                Cancelled?.Invoke(this);
            }
        }

        private void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action _execute;

        public event EventHandler CanExecuteChanged;
        
        public RelayCommand(Action execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(canExecute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter = null)
        {
            return _canExecute(null);
        }

        public void Execute(object parameter = null)
        {
            _execute();
        }
    }
}
