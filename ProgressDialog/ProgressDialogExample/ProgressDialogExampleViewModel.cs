using System;
using Prism.Mvvm;
using Prism.Commands;
using ProgressDialog;
using System.Threading.Tasks;
using System.Threading;

namespace ProgressDialogExample
{
    class ProgressDialogExampleViewModel : BindableBase
    {
        public DelegateCommand Testcommand_popup => new DelegateCommand(executeMethod: Testfunction_popup);
        public DelegateCommand Testcommand_inline => new DelegateCommand(Testfunction_inline);

        private IProgressStatus testStatus;
        private string progressUpdatedEventLast = "Never";
        private string finishedEventLast = "Never";
        private string cancelledEventLast = "Never";

        public IProgressStatus TestStatus
        {
            get => testStatus;
            private set
            {
                testStatus = value;
                RaisePropertyChanged(nameof(TestStatus));
            }
        }

        /// <summary>Async test function getting called by UI through delegate command.</summary>
        private async void Testfunction_popup()
        {
            /// Setup <see cref="ExampleProgressStatus"/> object to propagte updates & cancel request between view and function
            IProgressStatus progressStatus = new ProgressStatus();
            progressStatus.ProgessUpdated += HandleProgessUpdatedEvent;
            progressStatus.Finished += HandleFinishedEvent;
            progressStatus.Cancelled += HandleCancelledEvent;

            /// Start the async function to run in the background.
            Task ts = LongFunction(progressStatus);

            /// Instantiate & open the progress bar window asynchronously.
            /// One can also use .ShowDialog(), but it will block the thread until the window is closed - the task will still run, since it was already started async, but the try / catch block will not work.
            /// Otherwise, one can use .Show(), but this means the Dialog window won't be modal and interaction with other windows is possible.
            ProgressDialogWindow progressWindow = new ProgressDialogWindow("Example Progress Window", progressStatus);
            Task<bool?> progressWindowTask = progressWindow.ShowDialogAsync();

            /// Wait for the async task to finish, handle cancelation exception.
            try
            {
                await ts;
            }
            catch (OperationCanceledException)
            {
                // handle canceled operation
            }

            // close the window
            progressWindow.Close();
            await progressWindowTask;
        }

        

        private async void Testfunction_inline()
        {
            /// Setup <see cref="ExampleProgressStatus"/> object to propagte updates & cancel request between view and function
            TestStatus = new ProgressStatus();
            TestStatus.ProgessUpdated += HandleProgessUpdatedEvent;
            TestStatus.Finished += HandleFinishedEvent;
            TestStatus.Cancelled += HandleCancelledEvent;

            /// Start the async function to run in the background.
            Task ts = LongFunction(TestStatus);

            /// Instantiate & open the progress bar window asynchronously.
            /// One can also use .ShowDialog(), but it will block the thread until the window is closed - the task will still run, since it was already started async, but the try / catch block will not work.
            /// Otherwise, one can use .Show(), but this means the Dialog window won't be modal and interaction with other windows is possible.


            /// Wait for the async task to finish, handle cancelation exception.
            try
            {
                await ts;
            }
            catch (OperationCanceledException)
            {
                // handle canceled operation
            }
        }

        #region DemonstrateEvents
        // Properties for the view and handler functions for the events to demonstrate the operation of the events.

        public string ProgressUpdatedEventLast
        {
            get => progressUpdatedEventLast;
            set
            {
                progressUpdatedEventLast = value;
                RaisePropertyChanged(nameof(ProgressUpdatedEventLast));
            }
        }

        public string FinishedEventLast
        {
            get => finishedEventLast;
            set
            {
                finishedEventLast = value;
                RaisePropertyChanged(nameof(FinishedEventLast));
            }
        }

        public string CancelledEventLast
        {
            get => cancelledEventLast;
            set
            {
                cancelledEventLast = value;
                RaisePropertyChanged(nameof(CancelledEventLast));
            }
        }

        private void HandleCancelledEvent(IProgressStatus progressStatus) => CancelledEventLast = DateTime.Now.ToString();

        private void HandleFinishedEvent(IProgressStatus progressStatus) => FinishedEventLast = DateTime.Now.ToString();

        private void HandleProgessUpdatedEvent(IProgressStatus progressStatus) => ProgressUpdatedEventLast = DateTime.Now.ToString();
        #endregion

        /// <summary>Async function to execute.</summary>
        /// <param name="progressStatus">Status to update.</param>
        /// <returns></returns>
        public async Task LongFunction(IProgressStatus progressStatus)
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Run(() => { Thread.Sleep(1000); });
                progressStatus.Update("Steps completed " + (i + 1).ToString() + "/10", (i + 1) * 10);
                progressStatus.CT.ThrowIfCancellationRequested();
            }
        }
    }
}
