using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Service.Dtos.Object;
using FASS.Service.Entities.Object;
using FASS.Service.Services.Object.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.Object
{
    public class AutoDoorService : AuditService<FrameContext, AutoDoorEntity, AutoDoorDto>, IAutoDoorService
    {
        public AutoDoorService(
          IUnitOfWork<FrameContext> unitOfWork,
          IRepository<FrameContext, AutoDoorEntity> repository,
          IMapper mapper,
          IValidator<AutoDoorDto> validator)
          : base(unitOfWork, repository, mapper, validator)
        {
        }
    }
}
