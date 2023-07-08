[![BuildTest](https://github.com/ax-meyer/ProgressDialog/actions/workflows/BuildAndDeploy.yml/badge.svg?branch=main)](https://github.com/ax-meyer/ProgressDialog/actions/workflows/BuildAndDeploy.yml?query=branch%3Amain)

# ProgressDialog
Simple, easy to use dialog with ProgressBar for WPF & Avalonia with the MVVM pattern, that can either be embedded into an existing window or be displayed as a dialog in it's own window.

The package consists of two parts:
* The user interface side, either as standalone `ProgressDialogWindow`, or as `ProgressDialogUserControl` to be embedded into an existing window. 
* A `ProgressStatus` object implementing the `IProgressStatus` interface. The `ProgressStatus` object can be passed to the model and any long running task can use it to update the status in the view, receive a cancellation request etc. The `ProgressStatus` object is also passed as the `DataContext` to the `ProgressDialogUserControl`, so the view can bind to it's properties in a clean MVVM-fashion.

## How to use

Simply add one of these nuget packages to your project: 

[![Wpf](https://img.shields.io/nuget/v/ProgressDialog.Wpf.svg?label=Wpf)](https://www.nuget.org/packages/ProgressDialog.Wpf/) 

[![Avalonia](https://img.shields.io/nuget/v/ProgressDialog.Avalonia.svg?label=Avalonia)](https://www.nuget.org/packages/ProgressDialog.Avalonia/)

If you want to integrate the `ProgressStatus` object into a model in e.g. a `netStandard` library without support for UI frameworks, use this package in the model:

[![nuget](https://img.shields.io/nuget/v/ProgressDialogStatus.svg?label=nuget)](https://www.nuget.org/packages/ProgressDialogStatus/)

It provides the `ProgressStatus` class - and the `IProgressStatus` interface, in case you want to implement it yourself - seperately from the UI components. 

## Example
https://github.com/ax-meyer/ProgressDialog/tree/master/ProgressDialog/ProgressDialogExample provides a sample view model showing the usage of the ProgressDialog.

The dialog itself is just a plain, standard dialog per default:
![Screenshot of the progress dialog](resources/screenshot.png)

Since it just uses standard components (button, progress bar etc.) you can adapt the style however you like.
