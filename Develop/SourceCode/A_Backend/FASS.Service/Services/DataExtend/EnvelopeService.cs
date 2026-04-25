using AutoMapper;
using Common.EntityFramework.Services.Interfaces;
using Common.Frame.Contexts;
using Common.Service.Service;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Entities.DataExtend;
using FASS.Service.Services.DataExtend.Interfaces;
using FluentValidation;

namespace FASS.Service.Services.DataExtend
{
    public class EnvelopeService : AuditService<FrameContext, EnvelopeEntity, EnvelopeDto>, IEnvelopeService
    {
        public EnvelopeService(
            IUnitOfWork<FrameContext> unitOfWork,
            IRepository<FrameContext, EnvelopeEntity> repository,
            IMapper mapper,
            IValidator<EnvelopeDto> validator)
            : base(unitOfWork, repository, mapper, validator)
        {
        }
    }
}
