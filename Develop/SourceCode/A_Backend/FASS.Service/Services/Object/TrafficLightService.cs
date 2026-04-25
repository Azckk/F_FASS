using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Service;
using FASS.Service.Dtos.Object;
using FASS.Service.Entities.Object;
using FASS.Service.Services.Object.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace FASS.Service.Services.Object
{
    public class TrafficLightService : AuditService<FrameContext, TrafficLightEntity, TrafficLightDto>,
            ITrafficLightService
    {
        public TrafficLightService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, TrafficLightEntity> repository,
            IMapper mapper,
            IValidator<TrafficLightDto> validator
        )
            : base(unitOfWork, repository, mapper, validator) { }


        public async Task<IPage<TrafficLightItemDto>> ItemsGetPageAsync(string keyValue, Page page)
        {
            var dto = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e.Id == keyValue)
                .SelectMany(e => e.TrafficLightItems)
                .ProjectTo<TrafficLightItemDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return dto;
        }

    }
}
