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
using System.Windows;

namespace ProgressDialog
{
    /// Interaction logic for ProgressDialogWindow.xaml
    public partial class ProgressDialogWindow : Window
    {
        private readonly IProgressStatus ProgressStatus;

        /// <summary>
        /// Constructor for ProgressDialogWindow.
        /// </summary>
        /// <param name="progressWindowTitle">Window title.</param>
        /// <param name="ps">IProgressStatus providing the status information and updates.
        /// A sample implementation of the IProgressStatus Interface based on the Prism MVVM framework is provided in the seperate ProgressDialogStatus nuget package. This implementation is ready-to-use for projects using Prism for the MVMM pattern.
        /// </param>
        /// <param name="owner">Window owning this dialog. This dialog will be centered on the owner. If not provided, dialog will still work, but not be centered.</param>
        public ProgressDialogWindow(string progressWindowTitle, IProgressStatus ps, Window owner = null)
        {
            ProgressStatus = ps ?? throw new ArgumentNullException(nameof(ps));
            DataContext = ps;
            Title = progressWindowTitle;
            InitializeComponent();
            Closing += ProgressDialogWindow_Closing;
            Owner = owner;
        }

        private void ProgressDialogWindow_Closing(object sender, System.ComponentModel.CancelEventArgs ea)
        {
            ProgressStatus.CancelCommand.Execute(null);
        }
    }
}