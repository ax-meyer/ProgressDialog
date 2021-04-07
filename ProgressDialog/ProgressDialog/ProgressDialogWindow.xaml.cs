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