using Common.Frame.Contexts;
using Common.Service.Services.Interfaces;
using FASS.Service.Dtos.Object;
using FASS.Service.Entities.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FASS.Service.Services.Object.Interfaces
{
    public interface IElevatorItemService : IAuditService<FrameContext, ElevatorItemEntity, ElevatorItemDto>
    {
    
    }
}
