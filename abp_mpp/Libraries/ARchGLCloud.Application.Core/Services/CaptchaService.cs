using ARchGLCloud.Application.Core.Interfaces;
using ARchGLCloud.Application.Core.ViewModels;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Commands;
using ARchGLCloud.Domain.Core.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ARchGLCloud.Application.Core.Services
{
    public class CaptchaService : ICaptchaService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        private readonly ICaptchaRepository _repository;
        public CaptchaService(IMapper mapper,
                              IMediatorHandler bus,
                              ICaptchaRepository repository)
        {
            this._mapper = mapper;
            this._bus = bus;
            this._repository = repository;
        }

        public async Task AddAsync(CaptchaViewModel entity)
        {
            var addCaptchaCommand = _mapper.Map<AddCaptchaCommand>(entity);
            await _bus.SendCommand(addCaptchaCommand);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool Exists(string key)
        {
            return _repository.Exists(key);
        }

        public async Task<CaptchaViewModel> GetAsync(string key)
        {
            var captcha = await _repository.GetAsync(key);
            var captchaViewModel = _mapper.Map<CaptchaViewModel>(captcha);
            return captchaViewModel;
        }

        public async Task<List<CaptchaViewModel>> GetEntities(string key, int pageIndex, int pageSize)
        {
            var start = (pageIndex - 1) * pageSize;
            var stop = pageIndex * pageSize - 1;

            var entities = await _repository.GetEntities(key, start, stop);
            var cvms = new List<CaptchaViewModel>();
            foreach(var entity in entities)
            {
                cvms.Add(JsonConvert.DeserializeObject<CaptchaViewModel>(entity));
            }

            return cvms;
        }

        public async Task RemoveAsync(string key)
        {
            var removeCaptchaComman = new RemoveCaptchaCommand(key);
            await _bus.SendCommand(removeCaptchaComman);
        }

        public async Task UpdateAsync(string key, CaptchaViewModel entity)
        {
            var updateCaptchaCommand = _mapper.Map<UpdateCaptchaCommand>(entity);
            await _bus.SendCommand(updateCaptchaCommand);
        }
    }
}
