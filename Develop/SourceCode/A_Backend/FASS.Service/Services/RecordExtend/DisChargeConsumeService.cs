using AutoMapper;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Data.Models.Data;
using FASS.Service.Dtos.RecordExtend;
using FASS.Service.Entities.RecordExtend;
using FASS.Service.Services.RecordExtend.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FASS.Service.Services.RecordExtend
{
    public class DisChargeConsumeService
        : AuditService<FrameContext, DisChargeConsumeEntity, DisChargeConsumeDto>,
            IDisChargeConsumeService
    {
        public DisChargeConsumeService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, DisChargeConsumeEntity> repository,
            IMapper mapper,
            IValidator<DisChargeConsumeDto> validator
        )
            : base(unitOfWork, repository, mapper, validator) { }

        public int AddModelsWithOutChargeInstance(IEnumerable<DisChargeConsumeEntity> models)
        {
            var entities = Mapper.Map<IEnumerable<DisChargeConsumeEntity>>(models);
            var result = 0;
            try
            {
                UnitOfWork.Begin();
                foreach (var entity in entities)
                {
                    var existEntity = UnitOfWork
                        .GetRepository<DisChargeConsumeEntity>()
                        .Set()
                        .Where(e =>
                            e.Id == entity.Id
                            && ((DateTime)e.CreateAt).Date == ((DateTime)entity.CreateAt).Date
                        )
                        .FirstOrDefault();
                    if (existEntity == null)
                    {
                        UnitOfWork.GetRepository<DisChargeConsumeEntity>().Add(entity);
                        result++;
                    }
                    else
                    {
                        existEntity.CurrentDN = entity.CurrentDN;
                        existEntity.ConsumeDN = entity.ConsumeDN;

                        existEntity.LastDN = entity.LastDN;

                        UnitOfWork.GetRepository<DisChargeConsumeEntity>().Update(existEntity);
                        result++;
                    }
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
            return result;
        }

        public int UpdateModelsWithOutChargeInstance(IEnumerable<DisChargeConsumeEntity> models)
        {
            var entities = Mapper.Map<IEnumerable<DisChargeConsumeEntity>>(models);
            var result = 0;
            try
            {
                UnitOfWork.Begin();
                foreach (var entity in entities)
                {
                    var existEntity = UnitOfWork
                        .GetRepository<DisChargeConsumeEntity>()
                        .Set()
                        .Where(e =>
                            e.Id == entity.Id
                            && ((DateTime)e.CreateAt).Date == ((DateTime)entity.CreateAt).Date
                        )
                        .FirstOrDefault();
                    if (existEntity != null)
                    {
                        existEntity.CurrentDN = entity.CurrentDN;
                        existEntity.ConsumeDN = entity.ConsumeDN;

                        existEntity.LastDN = entity.LastDN;
                        UnitOfWork.GetRepository<DisChargeConsumeEntity>().Update(existEntity);
                        result++;
                    }
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
            return result;
        }

        public async Task AddOrUpdateAsync(List<Car> cars)
        {
            try
            {
                // await UnitOfWork.BeginAsync();

                List<DisChargeConsumeEntity> addModelList = new List<DisChargeConsumeEntity>();
                List<DisChargeConsumeEntity> updateModelList = new List<DisChargeConsumeEntity>();
                //获取当天所有的记录
                var dischargeConsumeEntities = await UnitOfWork
                    .GetRepository<DisChargeConsumeEntity>()
                    .Set()
                    .Where(e => ((DateTime)e.CreateAt).Date == DateTime.Now.Date)
                    .ToListAsync();

                //遍历所有小车
                foreach (var car in cars)
                {
                    var dischargeConsumeEntity = dischargeConsumeEntities.FirstOrDefault(e =>
                        e.CarCode == car.Code
                    );
                    if (dischargeConsumeEntity != null)
                    {
                        //如果电量在增加则是放电，更新这条记录
                        if (car.Battery - dischargeConsumeEntity.CurrentDN < 0)
                        {
                            dischargeConsumeEntity.LastDN = dischargeConsumeEntity.CurrentDN;
                            dischargeConsumeEntity.CurrentDN = (float)car.Battery;
                            dischargeConsumeEntity.ConsumeDN += (float)(
                                dischargeConsumeEntity.CurrentDN - car.Battery
                            );
                            /* dischargeConsumeEntity.CurrentDN = (float)car.Battery;
                             dischargeConsumeEntity.ConsumeDN += (float)(car.Battery - dischargeConsumeEntity.CurrentDN);*/
                            /*       int i = await UnitOfWork
                                       .GetRepository<ChargeConsumeEntity>()
                                       .UpdateAsync(chargeConsumeEntity);
                                   if (i == 0)
                                   {
                                       await UnitOfWork.RollbackAsync();
                                   }*/
                            updateModelList.Add(dischargeConsumeEntity);
                        }
                    }
                    else
                    {
                        //如果没有这台车，则新这条记录
                        dischargeConsumeEntity = new DisChargeConsumeEntity
                        {
                            CarCode = car.Code,
                            CurrentDN = (float)car.Battery,
                            LastDN = 0,
                            ConsumeDN = 0
                        };
                        /*    int i = await UnitOfWork
                                .GetRepository<ChargeConsumeEntity>()
                                .AddAsync(chargeConsumeEntity);
                            if (i == 0)
                            {
                               await UnitOfWork.RollbackAsync();
                            }*/
                        addModelList.Add(dischargeConsumeEntity);
                    }
                }

                if (updateModelList.Count() > 0)
                {
                    UpdateModelsWithOutChargeInstance(updateModelList);
                }
                if (addModelList.Count() > 0)
                {
                    AddModelsWithOutChargeInstance(addModelList);
                }

                // await UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                // await UnitOfWork.RollbackAsync();
            }
        }
    }
}
