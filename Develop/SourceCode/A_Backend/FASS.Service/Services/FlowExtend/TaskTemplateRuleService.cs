using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Service;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Entities.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.FlowExtend
{
    public class TaskTemplateRuleService : AuditService<FrameContext, TaskTemplateRuleEntity, TaskTemplateRuleDto>, ITaskTemplateRuleService
    {
        public TaskTemplateRuleService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, TaskTemplateRuleEntity> repository,
            IMapper mapper,
            IValidator<TaskTemplateRuleDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

        public async Task<IPage<TaskTemplateRuleDto>> ToPageAsync(string taskTemplateId, Page page)
        {
            var model = await Repository.Set()
                //.Include(e => e.TaskTemplateProcess)
                .ProjectTo<TaskTemplateRuleDto>(Mapper.ConfigurationProvider)
                .Where(e => e.TaskTemplateId == taskTemplateId)
                .ToPageAsync(page);
            return model;
        }

    }
}
