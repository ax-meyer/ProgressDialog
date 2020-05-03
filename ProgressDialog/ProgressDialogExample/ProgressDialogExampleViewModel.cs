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
            ExampleProgressStatus progressStatus = new ExampleProgressStatus();

            /// Start the async function to run in the background.
            Task ts = longFunction(progressStatus);

            /// Instantiate & open the progress bar window.
            ProgressDialogWindow progressWindow = new ProgressDialogWindow("Example Progress Window", progressStatus);

            /// Wait for the async task to finish, handle cancelation exception.
            progressWindow.ShowDialog();
            try
            {
                await ts;
            }
            catch (OperationCanceledException)
            {

            }
        }

        /// <summary>Async function to execute.</summary>
        /// <param name="progressStatus">Status to update.</param>
        /// <returns></returns>
        public async Task longFunction(AbstractProgressStatus progressStatus)
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Run(() => { Thread.Sleep(1000); });
                progressStatus.Update("Steps completed " + (i + 1).ToString() + "/10", (i+1) * 10);
                progressStatus.CTS.Token.ThrowIfCancellationRequested();
            }
        }
    }
}
