using Common.Frame.Contexts;
using Common.Frame.Dtos.Frame;
using Common.Frame.Entities.Frame;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.Setting;

namespace FASS.Service.Services.Setting.Interfaces
{
    public interface IConfigServiceService : IBaseService<FrameContext, ConfigEntity, ConfigDto>
    {
        ConfigServiceDto GetDto();

        Task<ConfigServiceDto> GetDtoAsync();

        int SetDto(ConfigServiceDto configDto);

        Task<int> SetDtoAsync(ConfigServiceDto configDto);
    }
}