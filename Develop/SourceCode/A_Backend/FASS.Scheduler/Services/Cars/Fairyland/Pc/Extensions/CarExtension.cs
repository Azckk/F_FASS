using Common.NETCore.Extensions;
using FASS.Data.Consts.Flow;
using FASS.Data.Models.Data;
using FASS.Service.Extends.Flow;

namespace FASS.Scheduler.Services.Cars.Fairyland.Pc.Extensions
{
    public static class CarExtension
    {
        private const string NULL = "NULL";
        public static Extend.Car.Fairyland.Pc.Models.Response.CarState ToCarState(this Car car)
        {
            var carState = new Extend.Car.Fairyland.Pc.Models.Response.CarState();
            carState.Code = car.Code;
            carState.Name = car.Name;
            carState.CurrState = car.CurrState;
            carState.Battery = car.Battery;
            carState.X = car.X;
            carState.Y = car.Y;
            carState.Theta = car.Theta;
            carState.Speed = car.Speed;
            carState.CurrNodeCode = car.CurrNode?.Code;
            carState.StartNodeCode = car.StartNode?.Code;
            carState.EndNodeCode = car.EndNode?.Code;
            carState.CurrEdgeCode = car.CurrEdge?.Code;
            carState.StartEdgeCode = car.StartEdge?.Code;
            carState.EndEdgeCode = car.EndEdge?.Code;
            foreach (var taskInstance in car.TaskInstances)
            {
                var task = new Extend.Car.Fairyland.Pc.Models.Response.Task()
                {
                    Code = taskInstance.Code,
                    State = taskInstance.State
                };
                foreach (var taskInstanceProcesse in taskInstance.TaskInstanceProcesses)
                {
                    var node = new Extend.Car.Fairyland.Pc.Models.Response.Node()
                    {
                        Code = taskInstanceProcesse.Code
                    };
                    foreach (var taskInstanceAction in taskInstanceProcesse.TaskInstanceActions)
                    {
                        var action = new Extend.Car.Fairyland.Pc.Models.Response.Action()
                        {
                            Code = taskInstanceAction.Id,
                            ActionType = taskInstanceAction.ActionType,
                            BlockingType = taskInstanceAction.BlockingType,
                            State = taskInstanceAction.State
                        };
                        node.Actions.Add(action);
                    }
                    task.Nodes.Add(node);
                }
                carState.Tasks.Add(task);
            }
            return carState;
        }

        public static Extend.Car.Fairyland.Pc.Models.Request.CarTask ToCarTask(this Car car)
        {
            var carTask = new Extend.Car.Fairyland.Pc.Models.Request.CarTask
            {
                CarCode = car.Code,
                CarType = car.CarType.Code,
                TaskCode = car.CurrTaskInstance?.Id ?? NULL,
                TaskType = car.CurrTaskInstance?.TaskTemplate?.Code ?? NULL,
                Priority = (int)(car.CurrTaskInstance?.Priority ?? 0)//模板的优先级
            };
            if (car.CurrTaskInstance?.Extend is not null && car.CurrTaskInstance.Extend.TryJsonTo<TaskInstanceExtend>(out var extend) && extend != null)
            {
                carTask.Material = extend.Material;
                carTask.ContainerSize = new Extend.Car.Fairyland.Pc.Models.Request.ContainerSize()
                {
                    Length = extend.ContainerSize?.Length ?? -1,
                    Width = extend.ContainerSize?.Width ?? -1,
                    Height = extend.ContainerSize?.Height ?? -1
                };
                //当实例中设置了优先级，优先下发设置的优先级
                if (extend.Priority != 0)
                {
                    carTask.Priority = extend.Priority;
                }
            }
            //var taskInstanceProcesses = car.CurrTaskInstance?.TaskInstanceProcesses.Where(e => TaskInstanceProcessConst.State.Execute.Contains(e.State)).Take(1) ?? [];
            var taskInstanceProcesses = car.CurrTaskInstance?.TaskInstanceProcesses.OrderBy(e => e.SortNumber).ToList() ?? [];
            foreach (var taskInstanceProcesse in taskInstanceProcesses)
            {
                var node = new Extend.Car.Fairyland.Pc.Models.Request.Node
                {
                    Code = taskInstanceProcesse.Node?.Code ?? NULL
                };
                //var taskInstanceActions = taskInstanceProcesse.TaskInstanceActions.Where(e => TaskInstanceActionConst.State.Execute.Contains(e.State)).Take(1);
                //foreach (var taskInstanceAction in taskInstanceProcesse.TaskInstanceActions)
                //{
                //    var action = new Extend.Car.Fairyland.Pc.Models.Request.Action();
                //    action.Code = taskInstanceAction.Id;
                //    action.ActionType = taskInstanceAction.ActionType;
                //    action.BlockingType = taskInstanceAction.BlockingType;
                //    foreach (var taskInstanceParameter in taskInstanceAction.TaskInstanceParameters)
                //    {
                //        var parameter = new Extend.Car.Fairyland.Pc.Models.Request.Parameter();
                //        parameter.Key = taskInstanceParameter.Key;
                //        parameter.Value = taskInstanceParameter.Value;
                //        action.Parameters.Add(parameter);
                //    }
                //    node.Actions.Add(action);
                //}
                carTask.Nodes.Add(node);
            }
            return carTask;
        }

        public static Extend.Car.Fairyland.Pc.Models.Request.CarAction ToCarAction(this Car car)
        {
            var carAction = new Extend.Car.Fairyland.Pc.Models.Request.CarAction
            {
                CarCode = car.Code
            };
            var action = new Extend.Car.Fairyland.Pc.Models.Request.Action
            {
                Code = car.CurrCarInstantAction?.Id ?? NULL,
                ActionType = car.CurrCarInstantAction?.ActionType ?? NULL,
                BlockingType = car.CurrCarInstantAction?.BlockingType ?? NULL
            };
            if (car.CurrCarInstantAction is not null)
            {
                foreach (var carInstantParameter in car.CurrCarInstantAction.CarInstantParameters)
                {
                    var parameter = new Extend.Car.Fairyland.Pc.Models.Request.Parameter
                    {
                        Key = carInstantParameter.Key,
                        Value = carInstantParameter.Value
                    };
                    action.Parameters.Add(parameter);
                }
            }
            return carAction;
        }
    }
}