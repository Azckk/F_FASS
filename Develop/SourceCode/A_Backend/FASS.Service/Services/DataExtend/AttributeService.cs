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
    public class AttributeService : AuditService<FrameContext, AttributeEntity, AttributeDto>, IAttributeService
    {
        public AttributeService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, AttributeEntity> repository,
            IMapper mapper,
            IValidator<AttributeDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }
    }
}
