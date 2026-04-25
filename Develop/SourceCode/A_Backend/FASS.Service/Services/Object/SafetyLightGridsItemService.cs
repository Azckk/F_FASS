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
    public class SafetyLightGridsItemService
        : AuditService<FrameContext, SafetyLightGridsItemEntity, SafetyLightGridsItemDto>,
            ISafetyLightGridsItemService
    {
        public SafetyLightGridsItemService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, SafetyLightGridsItemEntity> repository,
            IMapper mapper,
            IValidator<SafetyLightGridsItemDto> validator
        )
            : base(unitOfWork, repository, mapper, validator) { }


       

    }
}
