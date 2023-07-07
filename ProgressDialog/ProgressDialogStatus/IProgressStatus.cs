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
using System.Threading;
using System.Windows.Input;

namespace ProgressDialog
{
    public interface IProgressStatus
    {
        /// <summary>Gets CancellationToken to use to cancel the async function.</summary>
        CancellationToken Ct { get; }

        /// <summary>Command executed when cancel button is clicked. Requests cancellation through cancellation token.</summary>
        ICommand CancelCommand { get; }

        /// <summary>Gets a value indicating whether the associated task was cancelled.</summary>
        bool IsCancelled { get; }

        /// <summary>Event published when the associated task is finished.</summary>
        event Action<IProgressStatus> Finished;

        /// <summary>Event published when the associated task is cancelled.</summary>
        event Action<IProgressStatus> Cancelled;

        /// <summary>Event published when the progress is updated.</summary>
        event Action<IProgressStatus> ProgressUpdated;

        /// <summary>Gets or sets a value indicating whether the associated task is finished.</summary>
        bool IsFinished { get; set; }
        
        /// <summary>Gets message to be displayed in ProgressDialog.</summary>
        string Message { get; }
        
        /// <summary>Gets progress in percent shown by ProgressBar in ProgressDialog.</summary>
        int ProgressPercent { get; }

        /// <summary>Update ProgressDialog.</summary>
        /// <param name="message">New message to be shown.</param>
        /// <param name="progressPercent">New progress level to be shown.</param>
        void Update(string message, int progressPercent);
    }
}
