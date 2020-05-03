using Prism.Commands;
using Prism.Mvvm;
using System.Threading;
using System.Windows;

namespace ProgressDialog
{
    /// <summary>Basic class for progress status. Provides everything that is needed for a simple, working implementation.</summary>
    public abstract class AbstractProgressStatus : BindableBase
    {
        private int progressPercent = 0;
        private string message = "Waiting for task to start...";

        /// <summary>Gets CancellationTokenSource to use to cancel the async function.</summary>
        public CancellationTokenSource CTS { get; private set; } = new CancellationTokenSource();

        /// <summary>Command executed when cancel button is clicked.</summary>
        public DelegateCommand<Window> CancelCommand => new DelegateCommand<Window>(cancelCommand);

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
                RaisePropertyChanged(nameof(ProgressPercent));
            }
        }

        /// <summary>Update ProgressDialog.</summary>
        /// <param name="message">New message to be shown.</param>
        /// <param name="progressPercent">New progress level to be shown.</param>
        public void Update(string message, int progressPercent)
        {
            Message = message;
            ProgressPercent = progressPercent;
        }

        /// <summary>Function called by <see cref="CancelEvent"/>. Sends Cancel request to <see cref="CTS"/> and closes the window.</summary>
        /// <param name="win"></param>
        private void cancelCommand(Window win)
        {
            CTS.Cancel();
            win.Close();
        }
    }
}
