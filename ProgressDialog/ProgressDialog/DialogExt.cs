using System.Threading.Tasks;

namespace ProgressDialog
{
    public static class DialogExt
    {
        public static async Task<bool?> ShowDialogAsync(this ProgressDialogWindow @this)
        {
            await Task.Yield();
            return @this.ShowDialog();
        }
    }
}
