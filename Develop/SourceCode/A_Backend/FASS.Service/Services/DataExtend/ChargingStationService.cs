using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Entities.DataExtend;
using FASS.Service.Services.DataExtend.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.DataExtend
{
    public class ChargingStationService : AuditService<FrameContext, ChargingStationEntity, ChargingStationDto>, IChargingStationService
    {
        public ChargingStationService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, ChargingStationEntity> repository,
            IMapper mapper,
            IValidator<ChargingStationDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }
    }
}
