# ProgressDialog
Simple, easy to use dialog with ProgressBar for WPF with MVVM pattern.

Simply add the `ProgressDialog` project as dependency and use like this:
```
/// Setup status object.
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
```
