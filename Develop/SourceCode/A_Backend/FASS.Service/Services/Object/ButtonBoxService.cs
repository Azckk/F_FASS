using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.EntityFramework.Extensions;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Pager;
using Common.Service.Pager.Models;
using Common.Service.Service;
using FASS.Service.Dtos.Object;
using FASS.Service.Entities.Object;
using FASS.Service.Services.Object.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace FASS.Service.Services.Object
{
    public class ButtonBoxService : AuditService<FrameContext, ButtonBoxEntity, ButtonBoxDto>,
            IButtonBoxService
    {
        public ButtonBoxService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, ButtonBoxEntity> repository,
            IMapper mapper,
            IValidator<ButtonBoxDto> validator
        )
            : base(unitOfWork, repository, mapper, validator) { }


        public async Task<IPage<ButtonBoxItemDto>> ItemsGetPageAsync(string keyValue, Page page)
        {
            var dto = await Repository
                .Set()
                .AsNoTracking()
                .Where(e => e.Id == keyValue)
                .SelectMany(e => e.buttonBoxItems)
                .ProjectTo<ButtonBoxItemDto>(Mapper.ConfigurationProvider)
                .ToPageAsync(page);
            return dto;
        }

    }
}
