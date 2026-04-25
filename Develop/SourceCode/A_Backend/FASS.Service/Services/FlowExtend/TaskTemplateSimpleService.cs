using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Entities.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.FlowExtend
{
    public class TaskTemplateMdcsService : AuditService<FrameContext, TaskTemplateMdcsEntity, TaskTemplateMdcsDto>, ITaskTemplateMdcsService
    {
        public TaskTemplateMdcsService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, TaskTemplateMdcsEntity> repository,
            IMapper mapper,
            IValidator<TaskTemplateMdcsDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }
    }
}
