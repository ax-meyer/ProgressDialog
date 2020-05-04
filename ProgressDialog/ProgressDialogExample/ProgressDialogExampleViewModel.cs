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
        public DelegateCommand testcommand => new DelegateCommand(testfunction);

        /// <summary>Async test function getting called by UI through delegate command.</summary>
        private async void testfunction()
        {
            /// Setup <see cref="ExampleProgressStatus"/> object to propagte updates & cancel request between view and function
            ProgressStatus progressStatus = new ProgressStatus();

            /// Start the async function to run in the background.
            Task ts = longFunction(progressStatus);

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
                // hande canceled operation
            }

            // close the window
            progressWindow.Close();
            await progressWindowTask;
        }

        /// <summary>Async function to execute.</summary>
        /// <param name="progressStatus">Status to update.</param>
        /// <returns></returns>
        public async Task longFunction(ProgressStatus progressStatus)
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Run(() => { Thread.Sleep(1000); });
                if (i == 3)
                {
                    throw new InvalidCastException();
                }
                progressStatus.Update("Steps completed " + (i + 1).ToString() + "/10", (i+1) * 10);
                progressStatus.CT.ThrowIfCancellationRequested();
            }
        }
    }
}
