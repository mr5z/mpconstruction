using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MPConstruction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPConstruction.Droid.Services
{
    internal class ToastService : IToastService
    {
        private readonly Context context;
        private Toast toast;

        public ToastService(Context context)
        {
            this.context = context;
        }

        public void Show(string format, params object[] values)
        {
            toast?.Cancel();

            var formattedMsg = string.Format(format, values);
            toast = Toast.MakeText(context, formattedMsg, ToastLength.Long);
            toast.Show();
        }
    }
}