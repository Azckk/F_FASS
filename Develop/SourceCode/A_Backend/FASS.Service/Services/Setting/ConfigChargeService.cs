using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Frame.Dtos.Frame;
using Common.Frame.Entities.Frame;
using Common.Service.Service;
using EFCore.BulkExtensions;
using FASS.Service.Dtos.Setting;
using FASS.Service.Entities.Setting;
using FASS.Service.Models.Set;
using FASS.Service.Services.Setting.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FASS.Service.Services.Setting
{
    public class ConfigChargeService
        : AuditService<FrameContext, ConfigEntity, ConfigDto>,
            IConfigChargeService
    {
        public ConfigChargeService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, ConfigEntity> repository,
            IMapper mapper, IValidator<ConfigDto> validator)
            : base(unitOfWork, repository, mapper, validator) { 
        
        }

        public ConfigChargeSet GetDto()
        {
            var dto = new ConfigChargeSet();
            var props = dto.GetType().GetProperties();
            var keys = props.Select(e => e.Name);
            var data = ToList(e => keys.Contains(e.Key));
            foreach (var prop in props)
            {
                var value = data.FirstOrDefault(e => e.Key == prop.Name)?.Value;

                // 如果属性类型是 bool，则尝试进行转换
                if (prop.PropertyType == typeof(bool))
                {
                    if (string.Equals("true", value, StringComparison.OrdinalIgnoreCase))
                    {
                        prop.SetValue(dto, true);
                    }
                    else {
                        prop.SetValue(dto, false);

                    }
               
                }
                else
                {
                    // 如果属性类型不是 string，则尝试进行相应的类型转换
                    if (prop.PropertyType != typeof(string) && value != null)
                    {
                        var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                        prop.SetValue(dto, convertedValue);
                    }
                    else
                    {
                        prop.SetValue(dto, value);
                    }
                }
            }

            return dto;
        }

        public async Task<ConfigChargeSet> GetDtoAsync()
        {
            var dto = new ConfigChargeSet();
            var props = dto.GetType().GetProperties();
            var keys = props.Select(e => e.Name);
            var data = await ToListAsync(e => keys.Contains(e.Key));
            foreach (var prop in props)
            {
                var value = data.FirstOrDefault(e => e.Key == prop.Name)?.Value;

                // 如果属性类型是 bool，则尝试进行转换
                if (prop.PropertyType == typeof(bool))
                {
                    if (string.Equals("true", value, StringComparison.OrdinalIgnoreCase))
                    {
                        prop.SetValue(dto, true);
                    }
                    else
                    {
                        prop.SetValue(dto, false);

                    }

                }
                else
                {
                    // 如果属性类型不是 string，则尝试进行相应的类型转换
                    if (prop.PropertyType != typeof(string) && value != null)
                    {
                        var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                        prop.SetValue(dto, convertedValue);
                    }
                    else
                    {
                        prop.SetValue(dto, value);
                    }
                }
            }

            return dto;
        }

        public int SetDto(ConfigChargeSet configDto)
        {
            var dtos = configDto
                .GetType()
                .GetProperties()
                .Select(e => new ConfigDto
                {
                    Key = e.Name,
                    Value = e.GetValue(configDto)?.ToString()
                });
            var entities = Mapper.Map<List<ConfigEntity>>(dtos);

            try
            {
                UnitOfWork.Begin();
                UnitOfWork
                    .DbContext.Set<ConfigEntity>()
                    .Where(e => entities.Select(e => e.Key).Contains(e.Key))
                    .ExecuteDelete();
                UnitOfWork.DbContext.BulkInsert(entities);
                return UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
        }

        public async Task<int> SetDtoAsync(ConfigChargeSet configDto)
        {
            var dtos = configDto
                .GetType()
                .GetProperties()
                .Select(e => new ConfigDto
                {
                    Key = e.Name,
                    Value = e.GetValue(configDto)?.ToString()
                });
            var entities = Mapper.Map<List<ConfigEntity>>(dtos);
            try
            {
                await UnitOfWork.BeginAsync();
                await UnitOfWork
                    .DbContext.Set<ConfigEntity>()
                    .Where(e => entities.Select(e => e.Key).Contains(e.Key))
                    .ExecuteDeleteAsync();
                await UnitOfWork.DbContext.BulkInsertAsync(entities);
                return await UnitOfWork.CommitAsync();
            }
            catch
            {
                await UnitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
