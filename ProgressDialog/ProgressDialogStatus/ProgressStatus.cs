﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading;

namespace ProgressDialog
{
    /// <summary>Basic class for progress status. Provides everything that is needed for a simple, working implementation.</summary>
    public class ProgressStatus : BindableBase, IProgressStatus
    {
        private int progressPercent = 0;
        private string message = "Waiting for task to start...";
        private bool isFinished;


        /// <summary>Gets CancellationTokenSource to use to cancel the async function.</summary>
        private CancellationTokenSource CTS { get; set; } = new CancellationTokenSource();

        public CancellationToken CT => CTS.Token;

        /// <summary>Command executed when cancel button is clicked.</summary>
        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);

        /// <summary>Gets a value indicating whether the associated task was cancelled.</summary>
        public bool IsCancelled => CTS.IsCancellationRequested;

        /// <summary>Event published when the associated task is finished.</summary>
        public event Action<IProgressStatus> Finished;

        /// <summary>Event published when the associated task is cancelled.</summary>
        public event Action<IProgressStatus> Cancelled;

        /// <summary>Event published when the progress is updated.</summary>
        public event Action<IProgressStatus> ProgessUpdated;

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
                if (progressPercent >= 100)
                {
                    isFinished = true;
                    RaisePropertyChanged(nameof(IsFinished));
                    Finished?.Invoke(this);
                }
                else if (isFinished == true)
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
            ProgressPercent = progressPercent;
            ProgessUpdated?.Invoke(this);
        }

        private void Cancel()
        {
            CTS.Cancel();
            RaisePropertyChanged(nameof(IsCancelled));
            Cancelled?.Invoke(this);
        }
    }
}
