using MetroBlog.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IService
{
    public interface IMenuService
    {
        Rsp AddMenu(Model.ViewModel.Menu mMenu);
        Rsp UpdateMenu(Model.ViewModel.Menu mMenu);

        Model.ViewModel.Menu SelectMenuById(int menuId);
        IList<Model.ViewModel.Menu> SelectMenuList();
    }
}
