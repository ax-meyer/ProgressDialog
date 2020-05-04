﻿using Prism.Commands;
using Prism.Mvvm;
using System.Threading;
using System.Windows;

namespace ProgressDialog
{
    /// <summary>Basic class for progress status. Provides everything that is needed for a simple, working implementation.</summary>
    public class ProgressStatus : BindableBase
    {
        private int progressPercent = 0;
        private string message = "Waiting for task to start...";

        /// <summary>Gets CancellationTokenSource to use to cancel the async function.</summary>
        private CancellationTokenSource CTS { get; set; } = new CancellationTokenSource();

        public CancellationToken CT => CTS.Token;

        /// <summary>Command executed when cancel button is clicked.</summary>
        public DelegateCommand CancelCommand => new DelegateCommand(CTS.Cancel);

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
    }
}