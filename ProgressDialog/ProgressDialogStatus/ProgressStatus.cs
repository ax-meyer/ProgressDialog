/*
____ Copyright Start ____

MIT License

Copyright (c) 2020 ax-meyer

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

____ Copyright End ____
*/

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;

namespace ProgressDialog;

/// <summary>Basic class for progress status. Provides everything that is needed for a simple, working implementation.</summary>
public class ProgressStatus : INotifyPropertyChanged, IProgressStatus
{
    private int _progressPercent;
    private string _message = "Waiting for task to start...";
    private bool _isFinished;
    private bool IsRunning(object? _) => !(_isFinished || IsCancelled);

    /// <summary>Gets CancellationTokenSource to use to cancel the async function.</summary>
    private CancellationTokenSource Cts { get; set; } = new CancellationTokenSource();

    public CancellationToken Ct => Cts.Token;

    /// <summary>Command executed when cancel button is clicked.</summary>
    public ICommand CancelCommand => new RelayCommand(Cancel, IsRunning);

    /// <summary>Gets a value indicating whether the associated task was cancelled.</summary>
    public bool IsCancelled => Cts.IsCancellationRequested;

    /// <summary>Event published when the associated task is finished.</summary>
    public event Action<IProgressStatus>? Finished;

    /// <summary>Event published when the associated task is cancelled.</summary>
    public event Action<IProgressStatus>? Cancelled;

    /// <summary>Event published when the progress is updated.</summary>
    public event Action<IProgressStatus>? ProgressUpdated;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Gets or sets a value indicating whether the associated task is finished.</summary>
    public bool IsFinished
    {
        get => _isFinished;
        set
        {
            _isFinished = value;
            _progressPercent = 100;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(ProgressPercent));
            ((RelayCommand)CancelCommand).OnCanExecuteChanged();
            Finished?.Invoke(this);
        }
    }

    /// <summary>Gets message to be displayed in ProgressDialog.</summary>
    public string Message
    {
        get => _message;
        private set
        {
            _message = value;
            RaisePropertyChanged();
        }
    }

    /// <summary>Gets progress in percent shown by ProgressBar in ProgressDialog.</summary>
    public int ProgressPercent
    {
        get => _progressPercent;
        private set
        {
            _progressPercent = value;

            if (_isFinished)
            {
                _isFinished = false;
                RaisePropertyChanged(nameof(IsFinished));
            }

            RaisePropertyChanged();
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
        ProgressUpdated?.Invoke(this);
    }

    private void Cancel()
    {
        if (!IsFinished)
        {
            Cts.Cancel();
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
    private readonly Predicate<object?> _canExecute;
    private readonly Action _execute;

    public event EventHandler? CanExecuteChanged;
        
    public RelayCommand(Action execute, Predicate<object?> canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
    }

    public void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool CanExecute(object? parameter = null)
    {
        return _canExecute(parameter);
    }

    public void Execute(object? _ = null)
    {
        _execute();
    }
}