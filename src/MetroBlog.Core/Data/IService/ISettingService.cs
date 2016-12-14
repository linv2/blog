namespace MetroBlog.Core.Data.IService
{
    public interface ISettingService
    {
        bool SaveSetting(Model.ViewModel.Setting setting);
        Model.ViewModel.Setting GetSetting();
    }
}
