using Microsoft.EntityFrameworkCore;
using MinimalApi.Dal;
using Stratos.Core.Query;

namespace MinimalApi.Repositories;

public class ControllerUriInfoByApplicationRepo : IControllerUriInfoByApplicationRepo
{
    private readonly MinimalApiDbContext _dbContext;

    public ControllerUriInfoByApplicationRepo(MinimalApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<ControllerUriInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion)
    {
        // This query looks at web_api_apln_endpt for what endpoints to return to the
        // particular application and application version
        var sql = $@"
        select distinct
            sys_guid() id,
            waae.apln_id,
            a.nm apln_nm,
            waae.apln_ver,
            waa.envir_tp_id,
            wa.use_https, 
            null mach_nm,  
            waa.addr, 
            wav.port, 
            wac.uri_nm,
            case
                when instr(wav.ver,'.',1,2) is not null
                then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
            else
                wav.ver
            end ver,
            waa.ord
        from
            cmn_mstr.web_api_ctlr wac
            join cmn_mstr.web_api_ver wav on wac.web_api_id = wav.web_api_id
            join cmn_mstr.web_api_apln_endpt waae on wav.web_api_ver_id = waae.web_api_ver_id
            join cmn_mstr.web_api wa on wav.web_api_id = wa.web_api_id
            join cmn_mstr.apln a on waae.apln_id = a.apln_id
            join cmn_mstr.web_api_addr_xref waax on wa.web_api_id = waax.web_api_id
            join cmn_mstr.web_api_addr waa on waax.web_api_addr_id = waa.web_api_addr_id
        where
            waa.is_dflt = 1
        and waae.fac_id is null
            {QueryUtilities.GetWhereSql("wac.uri_nm", uriName)}
            {QueryUtilities.GetWhereSql("waa.envir_tp_id", (int)environmentType)}
            {QueryUtilities.GetWhereSql("waae.apln_id", applicationId)} ";

        if (!string.IsNullOrEmpty(applicationVersion))
            sql += QueryUtilities.GetWhereSql("waae.apln_ver", applicationVersion);

        var endpoints = await
            _dbContext.ControllerUriInfosByApplication
            .FromSqlRaw(sql)
            .ToListAsync();

        if (!endpoints.Any())
        {
            // This query will return the endpoints for a web api the same version
            // as the application; and therefore, no need for records
            // in web_api_apln_endpt
            sql = $@"
            select distinct
                sys_guid() id,
                a.apln_id,
                a.nm apln_nm,
                '{applicationVersion}' apln_ver,
                waa.envir_tp_id,
                wa.use_https,
                null mach_nm,
                waa.addr, 
                wav.port, 
                wac.uri_nm, 
                case
                    when instr(wav.ver,'.',1,2) is not null
                    then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
                else
                    wav.ver
                end ver,
                waa.ord
            from
                cmn_mstr.web_api_ctlr wac
                join cmn_mstr.web_api wa on wac.web_api_id = wa.web_api_id
                join cmn_mstr.web_api_ver wav on wa.web_api_id = wav.web_api_id and
                    case
                        when instr(wav.ver,'.',1,2) is not null
                        then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
                    else
                        wav.ver
                    end = '{applicationVersion}'
                join cmn_mstr.apln a on a.apln_id = {applicationId}
                join cmn_mstr.web_api_addr_xref waax on wa.web_api_id = waax.web_api_id
                join cmn_mstr.web_api_addr waa on waax.web_api_addr_id = waa.web_api_addr_id
            where
                waa.is_dflt = 1
                {QueryUtilities.GetWhereSql("wac.uri_nm", uriName)}
                {QueryUtilities.GetWhereSql("waa.envir_tp_id", (int)environmentType)} ";

            endpoints = await
                _dbContext.ControllerUriInfosByApplication
                .FromSqlRaw(sql)
                .ToListAsync();
        }
        return endpoints;
    }

    public async Task<IList<ControllerUriInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, string machineName)
    {
        // This query looks at web_api_apln_endpt for what endpoints to return to the
        // particular application and application version
        var sql = $@"
        select distinct
            sys_guid() id,
            waae.apln_id,
            a.nm apln_nm,
            waae.apln_ver,
            1 envir_tp_id,
            wa.use_https,
            '{machineName}' mach_nm,
            '{machineName}' addr, 
            wav.port, 
            wac.uri_nm,
            case
                when instr(wav.ver,'.',1,2) is not null
                then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
            else
                wav.ver
            end ver,
            1 ord
        from
            cmn_mstr.web_api_ctlr wac
            join cmn_mstr.web_api_ver wav on wac.web_api_id = wav.web_api_id
            join cmn_mstr.web_api_apln_endpt waae on wav.web_api_ver_id = waae.web_api_ver_id
            join cmn_mstr.web_api wa on wav.web_api_id = wa.web_api_id
            join cmn_mstr.apln a on waae.apln_id = a.apln_id
        where 1=1
            {QueryUtilities.GetWhereSql("wac.uri_nm", uriName)}
            {QueryUtilities.GetWhereSql("waae.apln_id", applicationId)} ";

        if (!string.IsNullOrEmpty(applicationVersion))
            sql += QueryUtilities.GetWhereSql("waae.apln_ver", applicationVersion);

        return await
            _dbContext.ControllerUriInfosByApplication
            .FromSqlRaw(sql)
            .ToListAsync();
    }
}
