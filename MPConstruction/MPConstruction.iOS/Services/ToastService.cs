using GlobalToast;
using MPConstruction.Services;

namespace MPConstruction.iOS.Services
{
    internal class ToastService : IToastService
    {
        private Toast toast;

        public void Show(string format, params object[] values)
        {
            toast?.Dismiss();

            var formattedMsg = string.Format(format, values);
            toast = Toast.ShowToast(formattedMsg);
        }
    }
}