using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Data.Dtos.Base;
using FASS.Data.Entities.Base;
using FASS.Service.Services.BaseExtend.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.BaseExtend
{
    public class NodePositionService : AuditService<FrameContext, NodePositionEntity, NodePositionDto>, INodePositionService
    {
        public NodePositionService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, NodePositionEntity> repository,
            IMapper mapper,
            IValidator<NodePositionDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

    }
}
