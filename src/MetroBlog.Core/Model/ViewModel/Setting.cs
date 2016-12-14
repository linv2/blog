

namespace MetroBlog.Core.Model.ViewModel
{
    public class Setting : IDbModelface
    {
        public string Site { get; set; }
        public string Title { get; set; }
        public string KeyWord { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public bool Comment { get; set; }

        public string DesKey { get; set; }
        public string DesVi { get; set; }
    }
}
