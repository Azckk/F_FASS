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
    public class ThirdpartySystemService : AuditService<FrameContext, ThirdpartySystemEntity, ThirdpartySystemDto>, IThirdpartySystemService
    {
        public ThirdpartySystemService(
           IUnitOfWork<FrameContext> unitOfWork,
           IRepository<FrameContext, ThirdpartySystemEntity> repository,
           IMapper mapper,
           IValidator<ThirdpartySystemDto> validator)
           : base(unitOfWork, repository, mapper, validator)
        {
        }
    }


}
