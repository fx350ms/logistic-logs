using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Logistics.Logs.AuditLogs.Dto;
using Logistics.Logs.Core;
using Logistics.Logs.Entities;
using Logistics.Logs.EntityAuditLogs.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Logistics.Logs.EntityAuditLogs;

public class EntityAuditLogAppService : AsyncCrudAppService<EntityAuditLog, EntityAuditLogDto, long, PagedResultRequestDto, EntityAuditLogDto, EntityAuditLogDto>, IEntityAuditLogAppService
{
    public EntityAuditLogAppService(IRepository<EntityAuditLog, long> repository)
        : base(repository)
    {
    }

    public override async Task<EntityAuditLogDto> CreateAsync(EntityAuditLogDto input)
    {
        //var id = await  ConnectDb.ExecuteNonQueryAsync<EntityAuditLogDto>( 
        var parameters = new[]
        {
            new SqlParameter("@EntityId", input.EntityId ?? (object)DBNull.Value),
            new SqlParameter("@TenantId", input.TenantId ?? (object)DBNull.Value),
            new SqlParameter("@ServiceName", input.ServiceName ?? (object)DBNull.Value),
            new SqlParameter("@MethodName", input.MethodName ?? (object)DBNull.Value),
            new SqlParameter("@EntityType", input.EntityType ?? (object)DBNull.Value),
            new SqlParameter("@Title", input.Title ?? (object)DBNull.Value),
            new SqlParameter("@Data", input.Data ?? (object)DBNull.Value),
            new SqlParameter("@UserId", input.UserId ?? (object)DBNull.Value),
            new SqlParameter("@UserName", input.UserName ?? (object)DBNull.Value)
        };

        var result = await ConnectDb.ExecuteScalarAsync(
            "SP_EntityAuditLogs_InsertAndGetId",
            CommandType.StoredProcedure,
            parameters
        ); // object


        input.Id = Convert.ToInt64(result);
        return input;
    }


    [HttpGet]
    public async Task<PagedResultDto<EntityAuditLogDataDto>> GetAllAuditLogData(PagedEntityAuditLogDataResultRequestDto input)
    {
        // logic kiểm tra dịch vụ
        var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        var parameters = new[]
       {
                new SqlParameter("@EntityId", input.EntityId  ),
                new SqlParameter("@TenantId", input.TenantId  ),
                new SqlParameter("@ServiceName", input.ServiceName ),
                new SqlParameter("@EntityType", input.EntityType  ),
                new SqlParameter("@UserId", input.UserId ??  -1),
                new SqlParameter("@SkipCount", input.SkipCount ),
                new SqlParameter("@MaxResultCount", input.MaxResultCount ),
                totalCountParam
            };

        var result = await ConnectDb.GetListAsync<EntityAuditLogDataDto>(
            "SP_EntityAuditLogs_GetAllAuditLogData",
            CommandType.StoredProcedure,
            parameters
        ); // object


        return new PagedResultDto<EntityAuditLogDataDto>(
            (int)totalCountParam.Value,
            result
        );

    }

    [HttpGet]
    public async Task<CompareEntityAuditLogDataResultDto> Compare(CompareEntityAuditLogDataRequestDto input)
    {
        var parameters = new[]
        {
            new SqlParameter("@EntityId", input.EntityId  ),
            new SqlParameter("@TenantId", input.TenantId  ),
            new SqlParameter("@ServiceName", input.ServiceName ),
            new SqlParameter("@EntityType", input.EntityType  ),
            new SqlParameter("@CreatationTimeInt", input.CreatationTimeInt  )
        };

        var list = await ConnectDb.GetListAsync<EntityAuditLogDataDto>(
        "SP_EntityAuditLogs_GetDataCompare",
        CommandType.StoredProcedure,
        parameters
        );

        var result = new CompareEntityAuditLogDataResultDto();

        // -------------------------------
        // Trường hợp không có bản ghi
        // -------------------------------
        if (list == null || !list.Any())
            return result;

        // -------------------------------
        // Trường hợp chỉ có 1 bản ghi
        // -------------------------------
        if (list.Count == 1)
        {
            result.OldVersion = list[0];

            var dictA = ParseJsonToDictionary(list[0].Data);

            foreach (var field in dictA)
            {
                result.OldOnly.Add(new JsonCompareItemDto
                {
                    Field = field.Key,
                    OldValue = field.Value,
                    NewValue = "",
                    Status = "OldOnly"
                });
            }

            return result;
        }

        // -------------------------------
        // Mặc định lấy 2 bản ghi mới nhất
        // -------------------------------
        var versionA = list.OrderByDescending(x => x.CreatationTimeInt).Skip(1).First();
        var versionB = list.OrderByDescending(x => x.CreatationTimeInt).First();

        result.OldVersion = versionA;
        result.NewVersion = versionB;

        var dictAFull = ParseJsonToDictionary(versionA.Data);
        var dictBFull = ParseJsonToDictionary(versionB.Data);

        var allKeys = dictAFull.Keys.Union(dictBFull.Keys).ToList();

        foreach (var key in allKeys)
        {
            dictAFull.TryGetValue(key, out var oldValue);
            dictBFull.TryGetValue(key, out var newValue);

            // =========== Giống nhau ===========
            if (dictAFull.ContainsKey(key) && dictBFull.ContainsKey(key) && oldValue == newValue)
            {
                result.Same.Add(new JsonCompareItemDto
                {
                    Field = key,
                    OldValue = oldValue,
                    NewValue = newValue,
                    Status = "Same"
                });
                continue;
            }

            // =========== Khác nhau giá trị ===========
            if (dictAFull.ContainsKey(key) && dictBFull.ContainsKey(key) && oldValue != newValue)
            {
                result.Different.Add(new JsonCompareItemDto
                {
                    Field = key,
                    OldValue = oldValue,
                    NewValue = newValue,
                    Status = "ValueDifferent"
                });
                continue;
            }

            // =========== A có – B không ===========
            if (dictAFull.ContainsKey(key) && !dictBFull.ContainsKey(key))
            {
                result.OldOnly.Add(new JsonCompareItemDto
                {
                    Field = key,
                    OldValue = oldValue,
                    NewValue = "",
                    Status = "OldOnly"
                });
                continue;
            }

            // =========== A không – B có ===========
            if (!dictAFull.ContainsKey(key) && dictBFull.ContainsKey(key))
            {
                result.NewOnly.Add(new JsonCompareItemDto
                {
                    Field = key,
                    OldValue = "",
                    NewValue = newValue,
                    Status = "NewOnly"
                });
                continue;
            }
        }

        return result;
    }


    private Dictionary<string, string> ParseJsonToDictionary(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new Dictionary<string, string>();

        return JsonConvert.DeserializeObject<Dictionary<string, object>>(json)
                          .ToDictionary(x => x.Key, x => x.Value?.ToString() ?? "");
    }

}

