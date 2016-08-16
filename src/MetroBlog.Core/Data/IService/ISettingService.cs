using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IService
{
    public interface ISettingService
    {
        bool SaveSetting(Model.ViewModel.Setting setting);
        Model.ViewModel.Setting GetSetting();
    }
}
