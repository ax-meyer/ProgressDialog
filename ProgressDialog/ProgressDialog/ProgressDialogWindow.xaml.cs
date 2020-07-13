using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgressDialog
{
    /// <summary>
    /// Interaction logic for ProgressDialogWindow.xaml
    /// </summary>
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