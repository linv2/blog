﻿

namespace MetroBlog.Core.Model.DbModel
{
    public class User : IDbModelface
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }
        
        public string PassWord { get; set; }
        
        public string Email { get; set; }
    }
}
