 using ARchGLCloud.Application.Core.Filters;
using ARchGLCloud.Application.Core.Interfaces;
using ARchGLCloud.Application.Core.ViewModels;
using ARchGLCloud.Core.Extensions;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Repositories;
using ARchGLCloud.Domain.Core.Commands;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ARchGLCloud.Domain.Core.Events;
using ARchGLCloud.Domain.Core.Models;

namespace ARchGLCloud.Application.Core.Services
{
    public class TenantService : ITenantService
    {
        private readonly IMapper _mapper;
        private readonly ITenantRepository _tenantRepository;
        private readonly IMediatorHandler _bus;
        private readonly IConfiguration Configuration;
        public TenantService(IMapper mapper, ITenantRepository tenantRepository, IMediatorHandler bus, IConfiguration configuration)
        {
            _mapper = mapper;
            _tenantRepository = tenantRepository;
            _bus = bus;
            Configuration = configuration;
        }

        public TenantViewModel Find(Guid id)
        {
            return _mapper.Map<TenantViewModel>(_tenantRepository.Find(id));
        }

        public IEnumerable<TenantViewModel> FindAll()
        {
            return _tenantRepository.FindAll().ProjectTo<TenantViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<TenantViewModel> Pager(TenantQueryFilter query, out int count)
        {
            Expression<Func<Tenant, bool>> expression = s => true;
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                expression = expression.And(s => s.Name == query.Name);
            }

            return _tenantRepository.Pager(expression, query.PageNumber, query.PageSize, query.OrderParams, out count).ProjectTo<TenantViewModel>(_mapper.ConfigurationProvider);
        }

        public void Add(TenantViewModel tenantViewModel)
        {
            var registerCommand = _mapper.Map<AddTenantCommand>(tenantViewModel);
            _bus.SendCommand(registerCommand);
        }

        public void Update(TenantViewModel tenantViewModel)
        {
            var updateCommand = _mapper.Map<UpdateTenantCommand>(tenantViewModel);
            _bus.SendCommand(updateCommand);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public string UpdateDatabase(string sql, string connectionString)
        {
            if (Configuration.GetConnectionString("ProjectConnection") == connectionString)
            {
                var schemaNames = _tenantRepository.FindAll().Select(x => x.ConnectionString).ToList();
                var message = "";
                foreach (var schemaName in schemaNames)
                {
                    try
                    {
                        _tenantRepository.ExecuteSql(string.Format(sql, schemaName));
                    }
                    catch (Exception e)
                    {
                        message += e.Message;
                    }
                }
                _bus.RaiseEvent(new UpdateDatabaseEvent(sql));
                return message;
            }
            else
            {
                return "连接串错误";
            }
        }

        public TenantViewModel FindByTenantHost(string host)
        {
            return _mapper.Map<TenantViewModel>(_tenantRepository.FindByHost(host));
        }

        public TenantViewModel FindByTenantId(Guid tenantId)
        {
            return _mapper.Map<TenantViewModel>(_tenantRepository.FindByTenantId(tenantId));
        }

        public void Remove(Guid id)
        {
            var removeCommand = new RemoveTenantCommand(id);
            _bus.SendCommand(removeCommand);
        }
    }
}