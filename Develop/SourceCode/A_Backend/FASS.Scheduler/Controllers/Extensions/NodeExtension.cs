using FASS.Data.Models.Data;

namespace FASS.Scheduler.Controllers.Extensions
{
    public static class NodeExtension
    {
        public static Models.Response.ToSomeWhere ToStandby(this Data.Models.Data.Standby standby)
        {
            var response = new Models.Response.ToSomeWhere
            {
                NodeId = standby.Node.Id,
                NodeCode = standby.Node.Code
            };
            return response;
        }
        public static Models.Response.ToSomeWhere ToCharge(this Data.Models.Data.Charge charge)
        {
            var response = new Models.Response.ToSomeWhere
            {
                NodeId = charge.Node.Id,
                NodeCode = charge.Node.Code
            };
            return response;
        }
        public static Models.Response.ToSomeWhere ToAvoid(this Data.Models.Data.Avoid avoid)
        {
            var response = new Models.Response.ToSomeWhere
            {
                NodeId = avoid.Node.Id,
                NodeCode = avoid.Node.Code
            };
            return response;
        }
    }
}
