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
    public class ElevatorItemService
        : AuditService<FrameContext, ElevatorItemEntity, ElevatorItemDto>,
            IElevatorItemService
    {
        public ElevatorItemService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, ElevatorItemEntity> repository,
            IMapper mapper,
            IValidator<ElevatorItemDto> validator
        )
            : base(unitOfWork, repository, mapper, validator) { }


       

    }
}
