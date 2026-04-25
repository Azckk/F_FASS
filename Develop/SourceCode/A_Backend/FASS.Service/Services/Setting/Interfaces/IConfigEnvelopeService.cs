using Common.Frame.Contexts;
using Common.Frame.Dtos.Frame;
using Common.Frame.Entities.Frame;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.Setting;
using FASS.Service.Entities.Setting;
using FASS.Service.Models.Set;



namespace FASS.Service.Services.Setting.Interfaces
{
    public  interface IConfigEnvelopeService : IBaseService<FrameContext, ConfigEnvelopeEntity, ConfigEnvelopeDto>
    {

        ConfigEnvelopeSet GetDto();

        Task<ConfigEnvelopeSet> GetDtoAsync();

        int SetDto(ConfigEnvelopeSet configDto);

        Task<int> SetDtoAsync(ConfigEnvelopeSet configDto);
    }
}
