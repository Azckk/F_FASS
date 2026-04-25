using Common.AspNetCore.Extensions;
using DotNetCore.CAP;
using FASS.Boot.Services;
using FASS.Data.Consts.Flow;
using FASS.Data.Dtos.Data;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Scheduler.Controllers.Models.Request;
using System.Diagnostics;

namespace FASS.Scheduler.Services.EventBus.Subscribes
{
    public class DefaultSubscribe : ICapSubscribe
    {
        private readonly static Lock _lock = new Lock();

        public EventBusService EventBusService { get; }

        public IBootService BootService { get; }

        private readonly ITaskInstanceService _taskInstanceService;

        public DefaultSubscribe(
            EventBusService eventBusService)
        {
            EventBusService = eventBusService;

            BootService = EventBusService.ServiceProvider.GetRequiredService<IBootService>();

            _taskInstanceService = EventBusService.ServiceProvider.GetScopeService<ITaskInstanceService>();
        }

        [CapSubscribe("CarController.Enable")]
        public void CarEnable(List<string> keyValues)
        {
            Debug.WriteLine(string.Join(",", keyValues));
            BootService.Cars.FindAll(e => keyValues.Contains(e.Id)).ForEach(p => p.IsEnable = true);
        }

        [CapSubscribe("CarController.Disable")]
        public void CarDisable(List<string> keyValues)
        {
            Debug.WriteLine(string.Join(",", keyValues));
            BootService.Cars.FindAll(e => keyValues.Contains(e.Id)).ForEach(p => p.IsEnable = false);
        }

        [CapSubscribe("CarController.CarDto")]
        public void CarUpdate(CarDto carDto)
        {
            Debug.WriteLine(string.Join(",", carDto));

            var car = BootService.Cars.FirstOrDefault(e => carDto.Id == e.Id);
            if (car != null)
            {
                lock (_lock)
                {
                    car.IpAddress = carDto.IpAddress;
                    car.Port = carDto.Port;
                    car.Name = carDto.Name;
                    car.Code = carDto.Code;
                    car.CarTypeId = carDto.CarTypeId;
                    car.Type = carDto.Type;
                    car.Width = carDto.Width;
                    car.Length = carDto.Length;
                    car.Height = carDto.Height;
                    car.ControlType = carDto.ControlType;
                    car.AvoidType = carDto.AvoidType;
                    car.MinBattery = carDto.MinBattery;
                    car.MaxBattery = carDto.MaxBattery;
                    car.IsEnable = carDto.IsEnable;
                }
            }
        }

        [CapSubscribe("TaskInstanceController.Cancel")]
        public void TaskInstanceCancel(TaskInstanceParam task)
        {
            EventBusService.Logger.LogInformation($"Cancel  carId: {task.CarId}, taskInstanceId: {task.TaskInstanceId}");
            lock (_lock)
            {
                if (!string.IsNullOrEmpty(task.CarId))
                {
                    var car = BootService.Cars.FindLast(e => e.Id == task.CarId);
                    if (car is not null && car.TaskInstances != null)
                    {
                        var taskInstance = car.TaskInstances.Where(e => e.Id == task.TaskInstanceId).FirstOrDefault();
                        if (taskInstance != null)
                        {
                            taskInstance.State = TaskInstanceConst.State.Canceling;
                            _taskInstanceService.Repository.ExecuteUpdate(e => e.Id == taskInstance.Id, s => s.SetProperty(b => b.State, TaskInstanceConst.State.Canceling));
                        }
                    }
                }
                else
                {
                    _taskInstanceService.Repository.ExecuteUpdate(e => e.Id == task.TaskInstanceId, s => s.SetProperty(b => b.State, TaskInstanceConst.State.Canceling));
                }
            }
        }

        [CapSubscribe("TaskInstanceController.Pause")]
        public void TaskInstancePause(TaskInstanceParam task)
        {
            EventBusService.Logger.LogInformation($"Pause  carId: {task.CarId}, taskInstanceId: {task.TaskInstanceId}");
            foreach (var car in BootService.Cars)
            {
                if (car.TaskInstances.Where(e => e.Id == task.TaskInstanceId).Count() > 0)
                {
                    var taskInstance = car.TaskInstances.Where(e => e.Id == task.TaskInstanceId).FirstOrDefault();
                    if (taskInstance is not null)
                        taskInstance.State = TaskInstanceConst.State.Created;
                }
            }
        }

        [CapSubscribe("TaskInstanceController.Resume")]
        public void TaskInstanceResume(TaskInstanceParam task)
        {
            EventBusService.Logger.LogInformation($"Resume  carId: {task.CarId}, taskInstanceId: {task.TaskInstanceId}");
            foreach (var car in BootService.Cars)
            {
                if (car.TaskInstances.Where(e => e.Id == task.TaskInstanceId).Count() > 0)
                {
                    var taskInstance = car.TaskInstances.Where(e => e.Id == task.TaskInstanceId).FirstOrDefault();
                    if (taskInstance is not null)
                        taskInstance.State = TaskInstanceConst.State.Created;
                }
            }
        }

        public Lock GetObjectLock()
        {
            return _lock;
        }

        [CapSubscribe("NodeController.Enable")]
        public void NodeEnable(List<string> keyValues)
        {
            Debug.WriteLine(string.Join(",", keyValues));
            BootService.Nodes.FindAll(e => keyValues.Contains(e.Code)).ForEach(p => p.IsLock = false);
        }

        [CapSubscribe("NodeController.Disable")]
        public void NodeDisable(List<string> keyValues)
        {
            Debug.WriteLine(string.Join(",", keyValues));
            BootService.Nodes.FindAll(e => keyValues.Contains(e.Code)).ForEach(p => p.IsLock = true);
        }
    }
}