using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Service;
using FASS.Data.Dtos.Warehouse;
using FASS.Service.Dtos.Object;
using FASS.Service.Entities.Object;
using FASS.Service.Services.Object.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.Object
{
    public class TrafficLightItemService
        : AuditService<FrameContext, TrafficLightItemEntity, TrafficLightItemDto>,
            ITrafficLightItemService
    {
        public TrafficLightItemService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, TrafficLightItemEntity> repository,
            IMapper mapper,
            IValidator<TrafficLightItemDto> validator
        )
            : base(unitOfWork, repository, mapper, validator) { }


       

    }
}
