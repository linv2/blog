using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Model.ViewModel
{
    public class Menu : DbModel.Menu
    {
        public Category Category { get; set; }
    }
}
