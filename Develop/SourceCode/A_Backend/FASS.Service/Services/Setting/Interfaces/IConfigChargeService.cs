using Common.Frame.Contexts;
using Common.Frame.Dtos.Frame;
using Common.Frame.Entities.Frame;
using Common.Service.Services.Interfaces;
using FASS.Service.Models.Set;

namespace FASS.Service.Services.Setting.Interfaces
{
    public interface IConfigChargeService : IBaseService<FrameContext, ConfigEntity, ConfigDto>
    {
        ConfigChargeSet GetDto();

        Task<ConfigChargeSet> GetDtoAsync();

        int SetDto(ConfigChargeSet configDto);

        Task<int> SetDtoAsync(ConfigChargeSet configDto);
    
    }

}
