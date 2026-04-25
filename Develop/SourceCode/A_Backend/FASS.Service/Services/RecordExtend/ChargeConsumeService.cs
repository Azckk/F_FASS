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
    public class ChargeConsumeService
        : AuditService<FrameContext, ChargeConsumeEntity, ChargeConsumeDto>,
            IChargeConsumeService
    {
        public ChargeConsumeService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, ChargeConsumeEntity> repository,
            IMapper mapper,
            IValidator<ChargeConsumeDto> validator
        )
            : base(unitOfWork, repository, mapper, validator) { }

        public int AddModelsWithOutChargeInstance(IEnumerable<ChargeConsumeEntity> models)
        {
            var entities = Mapper.Map<IEnumerable<ChargeConsumeEntity>>(models);
            var result = 0;
            try
            {
                UnitOfWork.Begin();
                foreach (var entity in entities)
                {
                    var existEntity = UnitOfWork
                        .GetRepository<ChargeConsumeEntity>()
                        .Set()
                        .Where(e =>
                            e.Id == entity.Id
                            && ((DateTime)e.CreateAt).Date == ((DateTime)entity.CreateAt).Date
                        )
                        .FirstOrDefault();
                    if (existEntity == null)
                    {
                        UnitOfWork.GetRepository<ChargeConsumeEntity>().Add(entity);
                        result++;
                    }
                    else
                    {
                        existEntity.CurrentDN = entity.CurrentDN;
                        existEntity.ConsumeDN = entity.ConsumeDN;

                        existEntity.LastDN = entity.LastDN;

                        UnitOfWork.GetRepository<ChargeConsumeEntity>().Update(existEntity);
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

        public int UpdateModelsWithOutChargeInstance(IEnumerable<ChargeConsumeEntity> models)
        {
            var entities = Mapper.Map<IEnumerable<ChargeConsumeEntity>>(models);
            var result = 0;
            try
            {
                UnitOfWork.Begin();
                foreach (var entity in entities)
                {
                    var existEntity = UnitOfWork
                        .GetRepository<ChargeConsumeEntity>()
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
                        UnitOfWork.GetRepository<ChargeConsumeEntity>().Update(existEntity);
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
                //await UnitOfWork.BeginAsync();
                List<ChargeConsumeEntity> addModelList = new List<ChargeConsumeEntity>();
                List<ChargeConsumeEntity> updateModelList = new List<ChargeConsumeEntity>();
                //获取当天所有的记录
                var chargeConsumeEntities = await UnitOfWork
                    .GetRepository<ChargeConsumeEntity>()
                    .Set()
                    .Where(e => ((DateTime)e.CreateAt).Date == DateTime.Now.Date)
                    .ToListAsync();

                //遍历所有小车
                foreach (var car in cars)
                {
                    var chargeConsumeEntity = chargeConsumeEntities.FirstOrDefault(e =>
                        e.CarCode == car.Code
                    );
                    if (chargeConsumeEntity != null)
                    {
                        //如果电量在增加则是充电，更新这条记录
                        if (car.Battery - chargeConsumeEntity.CurrentDN > 0)
                        {
                            chargeConsumeEntity.LastDN = chargeConsumeEntity.CurrentDN;

                            chargeConsumeEntity.CurrentDN = (float)car.Battery;
                            chargeConsumeEntity.ConsumeDN += (float)(
                                car.Battery - chargeConsumeEntity.CurrentDN
                            );

                            /*     int i = await UnitOfWork
                                     .GetRepository<ChargeConsumeEntity>()
                                     .UpdateAsync(chargeConsumeEntity);
                                 if (i == 0)
                                 {
                                   await  UnitOfWork.RollbackAsync();
                                     return;
                                 }*/
                            updateModelList.Add(chargeConsumeEntity);
                        }
                    }
                    else
                    {
                        //如果没有这台车，则新这条记录
                        chargeConsumeEntity = new ChargeConsumeEntity
                        {
                            CarCode = car.Code,
                            CurrentDN = (float)car.Battery,
                            LastDN = 0,
                            ConsumeDN = 0
                        };
                        /*     int i = await UnitOfWork
                                 .GetRepository<ChargeConsumeEntity>()
                                 .AddAsync(chargeConsumeEntity);
                             if (i == 0)
                             {
                                 UnitOfWork.RollbackAsync();
                                 return;
                             }*/
                        addModelList.Add(chargeConsumeEntity);
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
                //await UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                // await UnitOfWork.RollbackAsync();
            }
        }


    }
}
