using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.NETCore;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Service;


using FASS.Service.Dtos.RecordExtend;
using FASS.Service.Entities.RecordExtend;
using FASS.Service.Models.RecordExtend;
using FASS.Service.Services.RecordExtend.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FASS.Service.Services.RecordExtend
{
    public class AlarmMdcsService : AuditService<FrameContext, AlarmMdcsEntity, AlarmMdcsDto>, IAlarmMdcsService
    {
        public AlarmMdcsService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, AlarmMdcsEntity> repository,
            IMapper mapper,
            IValidator<AlarmMdcsDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }


        public async Task<IEnumerable<AlarmMdcsDto>> GetAlarmMdcsDtos()
        {
            // 获取当前时间的一个月前
            // var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var oneMonthAgo = DateTime.Now.AddDays(-15);

            // 查询最近一个月的数据
            var Entities = await Repository
                .Set()
                .Where(e =>
                    e.CreateAt >= oneMonthAgo // 筛选条件：CreateAt 在一个月内
                )
              
                .ToListAsync();
            var Dtos = Mapper.Map<IEnumerable<AlarmMdcsDto>>(Entities);
            return Dtos;
        }



        public int AddModel(AlarmMdcs model)
        {
            return AddModels(new[] { model }.ToList());
        }

        public int AddModels(IEnumerable<AlarmMdcs> models)
        {
            var entities = Mapper.Map<IEnumerable<AlarmMdcsEntity>>(models);
            return Repository.Add(entities);
        }

        public int UpdateModel(AlarmMdcs model)
        {
            var result = 0;
            //var dto = ToList(e => e.CarCode == model.CarCode && e.Code == model.Code && !e.IsFinish).OrderByDescending(e => e.StartTime).FirstOrDefault();
            //if (dto != null)
            //{
            //    dto.EndTime = model.EndTime;
            //    dto.IsFinish = true;
            //    result = Update(
            //    dto,
            //    e => e.EndTime,
            //    e => e.IsFinish);
            //}
            Repository.ExecuteUpdate(e => e.CarCode == model.CarCode && e.Code == model.Code && !e.IsFinish, s => s.SetProperty(b => b.IsFinish, true));
            return result;
        }

        public int UpdateModels(IEnumerable<AlarmMdcs> models)
        {
            var result = 0;
            var dtos = ToList(e => !e.IsFinish && models.Select(e => e.CarCode).ToList().Contains(e.CarCode));
            List<AlarmMdcsDto> alarmMdcs = new List<AlarmMdcsDto>();
            foreach (var model in models)
            {
                var dto = dtos.Where(e => e.CarCode == model.CarCode && e.Code == model.Code && !e.IsFinish).OrderByDescending(e => e.StartTime).FirstOrDefault();
                if (dto != null)
                {
                    dto.EndTime = model.EndTime;
                    dto.IsFinish = true;
                    alarmMdcs.Add(dto);
                }
            }
            if (alarmMdcs.Count > 0)
            {
                //result = Update(
                //alarmMdcs,
                //e => e.EndTime!,
                //e => e.IsFinish);
                Repository.ExecuteUpdate(e => alarmMdcs.Select(e=> e.Id).Contains(e.Id), s => s.SetProperty(b => b.IsFinish, true).SetProperty(b => b.EndTime, DateTime.Now));
            }
            return result;
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
