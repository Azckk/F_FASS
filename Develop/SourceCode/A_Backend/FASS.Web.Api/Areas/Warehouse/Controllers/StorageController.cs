using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Warehouse.Controllers
{
    [Route("api/v1/Warehouse/[controller]/[action]")]
    [Tags("仓储管理-储位管理")]
    public class StorageController : BaseController
    {
        private readonly ILogger<StorageController> _logger;
        private readonly IStorageService _storageService;

        public StorageController(
            ILogger<StorageController> logger,
            IStorageService storageService)
        {
            _logger = logger;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _storageService.ToPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _storageService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] StorageDto storageDto)
        {
            if(!string.IsNullOrEmpty(keyValue))
            {
                //更新库位状态时,先获取库位坐标值
                var data = Guard.NotNull(await _storageService.FirstOrDefaultAsync(keyValue));
                storageDto.Extend = data.Extend;
            }
            var result = await _storageService.AddOrUpdateAsync(keyValue, storageDto);
            if (result == 0)
            {
                return BadRequest("修改库位状态违规，请移除容器或者确认容器状态后修改");
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            try
            {
                await _storageService.DeleteAsync(keyValues);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest("库位和容器存在绑定关系或者历史记录里面有数据");
            }
           
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _storageService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _storageService.DisableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Lock([FromBody] List<string> keyValues)
        {
            await _storageService.Repository.ExecuteUpdateAsync(e => keyValues.Contains(e.Id), s => s.SetProperty(b => b.IsLock, true));
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UnLock([FromBody] List<string> keyValues)
        {
            await _storageService.Repository.ExecuteUpdateAsync(e => keyValues.Contains(e.Id), s => s.SetProperty(b => b.IsLock, false));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _storageService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetListByAreaToSelect([FromQuery] string areaId)
        {
            var data = await _storageService.Set().Where(e => e.IsEnable && e.AreaId == areaId).OrderBy(e => e.Code.Length).ThenBy(e => e.Code).ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetListByAreaCodeToSelect([FromQuery] string areaCode)
        {
            var data = await _storageService.Set().Where(e => e.IsEnable && e.AreaCode == areaCode).OrderBy(e => e.Code.Length).ThenBy(e => e.Code).ToListAsync();
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Select()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> SelectGetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _storageService.SelectGetPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Container()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> ContainerGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _storageService.ContainerGetPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpPut]

        public async Task<IActionResult> ContainerAdd([FromQuery] string keyValue, [FromBody] List<ContainerDto> containerDtos)
        {
            if (containerDtos.Count() > 1)
            {
                return BadRequest("一个库位只容许添加一个容器");
            }
            await _storageService.ContainerAddAsync(keyValue, containerDtos);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> ContainerDelete([FromQuery] string keyValue, [FromBody] List<ContainerDto> containerDtos)
        {
            await _storageService.ContainerDeleteAsync(keyValue, containerDtos);
            return Ok();
        }

        //[HttpGet]
        //public IActionResult Material()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> MaterialGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _storageService.MaterialGetPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> MaterialAdd([FromQuery] string keyValue, [FromBody] List<MaterialDto> materialDtos)
        {
            //根据库位ID获取库位绑定的容器信息

            var result = await _storageService.MaterialAddAsync(keyValue, materialDtos);
            if (result == 0)
            {
                return BadRequest("库位没有绑定容器请先绑定容器");
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> MaterialDelete([FromQuery] string keyValue, [FromBody] List<MaterialDto> materialDtos)
        {
            await _storageService.MaterialDeleteAsync(keyValue, materialDtos);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetStorageByNode([FromQuery] string code)
        {
            var data = await _storageService.FirstOrDefaultAsync(e => e.NodeCode == code);
            return Ok(data);
        }

    }
}
