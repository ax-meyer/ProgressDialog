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
        public ProgressDialogWindow(string progressWindowTitle, IProgressStatus ps)
        {
            ProgressStatus = ps ?? throw new ArgumentNullException(nameof(ps));
            DataContext = ps;
            Title = progressWindowTitle;
            InitializeComponent();
            Closing += ProgressDialogWindow_Closing;
        }

        private void ProgressDialogWindow_Closing(object sender, System.ComponentModel.CancelEventArgs ea)
        {
            ProgressStatus.CancelCommand.Execute(null);
        }
    }
}