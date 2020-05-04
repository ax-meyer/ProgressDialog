# ProgressDialog
Simple, easy to use dialog with ProgressBar for WPF with MVVM pattern.

Simply add the nuget package https://www.nuget.org/packages/ProgressDialog/ project as dependency and use like this:

```
using ProgressDialog;

/// <summary>Async test function getting called by UI through delegate command.</summary>
private async void testfunction()
{
    /// Setup <see cref="ProgressStatus"/> object to propagte updates & cancel request between view and function
    ProgressStatus progressStatus = new ProgressStatus();

    /// Start the async function to run in the background.
    Task ts = longFunction(progressStatus);

    /// Instantiate & open the progress bar window asynchronously.
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
        progressStatus.Update("Steps completed " + (i + 1).ToString() + "/10", (i+1) * 10);
        
        // Check if cancealtion is requested and exit.
        progressStatus.CT.ThrowIfCancellationRequested();
    }
}
```
