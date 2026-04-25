using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Service.Dtos.Custom;
using FASS.Service.Entities.Custom;
using FASS.Service.Services.Custom.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.Custom
{
    public class DemoService : AuditService<FrameContext, DemoEntity, DemoDto>, IDemoService
    {
        public DemoService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, DemoEntity> repository,
            IMapper mapper,
            IValidator<DemoDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

        public override int Update(DemoDto dto)
        {
            var result = Update(
                dto,
                e => e.Code,
                e => e.Name!,
                e => e.Type,
                e => e.State,
                e => e.IsEnable);
            return result;
        }

        public override async Task<int> UpdateAsync(DemoDto dto)
        {
            var result = await UpdateAsync(
                dto,
                e => e.Code,
                e => e.Name!,
                e => e.Type,
                e => e.State,
                e => e.IsEnable);
            return result;
        }
    }
}
