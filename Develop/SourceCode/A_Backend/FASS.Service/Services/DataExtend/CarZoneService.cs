using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Service;
using FASS.Data.Dtos.Base;
using FASS.Data.Entities.Base;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Entities.DataExtend;
using FASS.Service.Services.DataExtend.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FASS.Service.Services.DataExtend
{
    public class CarZoneService : AuditService<FrameContext, CarZoneEntity, CarZoneDto>, ICarZoneService
    {
        public CarZoneService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, CarZoneEntity> repository,
            IMapper mapper,
            IValidator<CarZoneDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

        public async Task<IPage<CarZoneDto>> ZoneGetPageAsync(string keyValue, Page page)
        {
            var dto = await Repository.Set()
                .AsNoTracking()
                .Where(e => e.CarId == keyValue)
                .ProjectTo<CarZoneDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return dto;
        }

        public async Task<int> ZoneAddAsync(string keyValue, IEnumerable<ZoneDto> zoneDtos)
        {
            var ZoneIds = zoneDtos.Select(e => e.Id);
            return await ZoneAddAsync(keyValue, ZoneIds);
        }

        public async Task<int> ZoneAddAsync(string keyValue, IEnumerable<string> zoneIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                var zoneEntities = await UnitOfWork.GetRepository<ZoneEntity>().ToListAsync(e => e.IsEnable);
                foreach (var ZoneId in zoneIds)
                {
                    var entity = new CarZoneEntity() { CarId = keyValue, ZoneId = ZoneId };
                    var isExist = await UnitOfWork.GetRepository<CarZoneEntity>().AnyAsync(e => e.CarId == entity.CarId && e.ZoneId == entity.ZoneId);
                    if (!isExist)
                    {
                        entity.Id = Guid.NewGuid().ToString();
                        entity.Remark = zoneEntities.Where(e => e.Id == ZoneId).FirstOrDefault()?.Name;
                        await UnitOfWork.GetRepository<CarZoneEntity>().AddAsync(entity);
                        result++;
                    }
                }
                await UnitOfWork.CommitAsync();
            }
            catch
            {
                await UnitOfWork.RollbackAsync();
                throw;
            }
            return result;
        }

        public async Task<int> ZoneDeleteAsync(string keyValue, IEnumerable<string> carZoneIds)
        {
            var result = 0;
            try
            {
                await UnitOfWork.BeginAsync();
                foreach (var carZoneId in carZoneIds)
                {
                    var isExist = await UnitOfWork.GetRepository<CarZoneEntity>().AnyAsync(e => e.Id == carZoneId);
                    if (isExist)
                    {
                        await UnitOfWork.GetRepository<CarZoneEntity>().DeleteAsync(e => e.Id == carZoneId);
                        result++;
                    }
                }
                await UnitOfWork.CommitAsync();
            }
            catch
            {
                await UnitOfWork.RollbackAsync();
                throw;
            }
            return result;
        }

        public async Task<int> ZoneDeleteAsync(string keyValue, IEnumerable<CarZoneDto> carZoneDtos)
        {
            var carZoneIds = carZoneDtos.Select(e => e.Id);
            return await ZoneDeleteAsync(keyValue, carZoneIds);
        }

    }
}
