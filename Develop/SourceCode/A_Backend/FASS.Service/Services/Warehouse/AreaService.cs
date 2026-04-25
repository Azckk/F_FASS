using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Service;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Entities.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FASS.Service.Services.Warehouse
{
    public class AreaService : AuditService<FrameContext, AreaEntity, AreaDto>, IAreaService
    {
        public AreaService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, AreaEntity> repository,
            IMapper mapper,
            IValidator<AreaDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

        public async Task<IPage<StorageDto>> StorageGetPageAsync(string? keyValue, Page page)
        {
            var dto = await Repository.Set()
                .AsNoTracking()
                .Where(e => !string.IsNullOrWhiteSpace(keyValue) ? (e.Id == keyValue && e.IsEnable) : e.IsEnable)
                .SelectMany(e => e.Storages)
                .ProjectTo<StorageDto>(Mapper.ConfigurationProvider)
                .Where(e => e.IsEnable)
                .OrderBy(e => e.Code.Length)
                .ToPageAsync(page);
            return dto;
        }

    }
}
