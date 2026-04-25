using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Service.Dtos.Warehouse;
using FASS.Service.Entities.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.Warehouse
{
    public class TagService : AuditService<FrameContext, TagEntity, TagDto>, ITagService
    {
        public TagService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, TagEntity> repository,
            IMapper mapper,
            IValidator<TagDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }
    }


}
