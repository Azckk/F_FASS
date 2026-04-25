namespace FASS.Scheduler.Controllers.Extensions
{
    public static class CarExtension
    {
        public static Models.Response.State ToState(this Data.Models.Data.Car car)
        {
            var state = new Models.Response.State
            {
                Code = car.Code,
                Name = car.Name,
                CurrState = car.CurrState,
                Battery = car.Battery,
                X = car.X,
                Y = car.Y,
                Theta = car.Theta,
                Speed = car.Speed,
                CurrNodeCode = car.CurrNode?.Code,
                StartNodeCode = car.StartNode?.Code,
                EndNodeCode = car.EndNode?.Code,
                CurrEdgeCode = car.CurrEdge?.Code,
                StartEdgeCode = car.StartEdge?.Code,
                EndEdgeCode = car.EndEdge?.Code,
                Tasks = car.TaskInstances.Select(e1 => new Models.Response.Task()
                {
                    Code = e1.Code,
                    State = e1.State,
                    Nodes = e1.TaskInstanceProcesses.Select(p => new Models.Response.Node()
                    {
                        Code = p.Code,
                        Actions = p.TaskInstanceActions.Select(e => new Models.Response.Action()
                        {
                            Code = e.Id,
                            ActionType = e.ActionType,
                            BlockingType = e.BlockingType,
                            State = e.State
                        })
                    })
                })
            };
            return state;
        }

        public static Models.Response.TaskInstance ToTaskInstance(this Data.Models.Flow.TaskInstance taskInstance)
        {
            var response = new Models.Response.TaskInstance
            {
                Code = taskInstance.Code,
                Name = taskInstance.Name,
                Type = taskInstance.Type,
                State = taskInstance.State
            };
            return response;
        }

        public static Models.Response.CarInstantAction ToCarInstantAction(this Data.Models.Instant.CarInstantAction carInstantAction)
        {
            var response = new Models.Response.CarInstantAction
            {
                ActionType = carInstantAction.ActionType,
                BlockingType = carInstantAction.BlockingType,
                State = carInstantAction.State,
                Remark = carInstantAction.Remark
            };
            return response;
        }
    }
}
