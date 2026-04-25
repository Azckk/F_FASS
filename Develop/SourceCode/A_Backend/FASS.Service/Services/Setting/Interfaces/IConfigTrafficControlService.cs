using Common.Frame.Contexts;
using Common.Frame.Dtos.Frame;
using Common.Frame.Entities.Frame;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.Setting;
using FASS.Service.Entities.Setting;
using FASS.Service.Models.Set;

namespace FASS.Service.Services.Setting.Interfaces
{
    public interface IConfigTrafficControlService : IBaseService<FrameContext, ConfigTrafficControlEntity, ConfigTrafficControlDto>
    {
        ConfigTrafficControlSet GetDto();

        Task<ConfigTrafficControlSet> GetDtoAsync();

        int SetDto(ConfigTrafficControlSet configDto);

        Task<int> SetDtoAsync(ConfigTrafficControlSet configDto);
    }
}