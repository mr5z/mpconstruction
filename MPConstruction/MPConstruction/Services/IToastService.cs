using System;
using System.Collections.Generic;
using System.Text;

namespace MPConstruction.Services
{
    public interface IToastService
    {
        void Show(string format, params object[] values);
    }
}
