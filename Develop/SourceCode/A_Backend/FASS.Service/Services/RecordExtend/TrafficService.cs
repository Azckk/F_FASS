using AutoMapper;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Service.Dtos.RecordExtend;
using FASS.Service.Entities.RecordExtend;
using FASS.Service.Models.RecordExtend;
using FASS.Service.Services.RecordExtend.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.RecordExtend
{
    public class TrafficService : AuditService<FrameContext, TrafficEntity, TrafficDto>, ITrafficService
    {
        public TrafficService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, TrafficEntity> repository,
            IMapper mapper,
            IValidator<TrafficDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }

        public override int Update(TrafficDto trafficDto)
        {
            var result = Update(
                trafficDto,
                e => e.LockedNodes,
                e => e.State!,
                e => e.StartTime,
                e => e.EndTime!,
                e => e.IsFinish,
                e => e.IsEnable);
            return result;
        }

        public int AddEntity(TrafficEntity entity)
        {
            return AddEntities(new[] { entity }.ToList());
        }

        public int AddEntities(IEnumerable<TrafficEntity> entities)
        {
            var historyEntities = Repository.Set()
                .Where(e => e.CreateAt > DateTime.Now.AddMinutes(-5))
                .ToList();
            var result = entities.Where(e => !historyEntities.Any(c => c.FromCarCode == e.FromCarCode && c.ToCarCode == e.ToCarCode && c.StartTime == e.StartTime)).ToList();
            return Repository.Add(entities);
        }

        public int AddModel(Traffic model)
        {
            return AddModels(new[] { model }.ToList());
        }

        public int UpdateModel(Traffic model)
        {
            var dto = FirstOrDefault(e => e.FromCarCode == model.FromCarCode);
            if (dto != null) {
                dto.ToCarCode = model.ToCarCode;
                dto.ToCarName = model.ToCarName;
                dto.LockedNodes = model.LockedNodes;
                dto.State = model.State;
                dto.StartTime = model.StartTime;
                dto.Info = model.Info;
                var result = Update(dto);
                return result;
            }
            return 0;
        }

        public int AddModels(IEnumerable<Traffic> models)
        {
            var entities = Mapper.Map<IEnumerable<TrafficEntity>>(models);
            return Repository.Add(entities);
        }

        public int DeleteByCarCode(string carCode)
        {
            return Repository.ExecuteDelete(e => e.FromCarCode == carCode);
        }

        public async Task<int> DeleteM3Async()
        {
            return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddMonths(-3));
        }

        public async Task<int> DeleteM1Async()
        {
            return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddMonths(-1));
        }

        public async Task<int> DeleteW1Async()
        {
            return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddDays(-7));
        }

        public async Task<int> DeleteD1Async()
        {
            return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddDays(-1));
        }

        public async Task<int> DeleteAllAsync()
        {
            return await Repository.ExecuteDeleteAsync(e => true);
        }

        public async Task<int> DeleteDayAsync(int day = 90)
        {
            return await Repository.ExecuteDeleteAsync(e => e.CreateAt < DateTime.Now.AddDays(-day));
        }
    }
}
