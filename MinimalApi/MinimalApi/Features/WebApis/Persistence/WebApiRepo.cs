using Microsoft.EntityFrameworkCore;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.WebApis;

public class WebApiRepo : IWebApiRepo
{
    private readonly IWebApiDbContext _dbContext;

    public WebApiRepo(IWebApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Application?> GetApplicationAsync(int applicationId, CancellationToken cancellationToken)
    {
        return await
            (from a in _dbContext.Applications
             where a.Id == ApplicationId.Create(applicationId)
             select a)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Application?> GetApplicationAsync(string applicationName, CancellationToken cancellationToken)
    {
        return await
            (from a in _dbContext.Applications
             where a.Name == applicationName
             select a)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<ControllerUriInfoByApplicationDto>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, CancellationToken cancellationToken)
    {
        // This query looks at web_api_apln_endpt for what endpoints to return to the
        // particular application and application version
        var sql = $"""
        select distinct
            waae.apln_id "{nameof(ControllerUriInfoByApplicationDto.ApplicationId)}",
            a.nm "{nameof(ControllerUriInfoByApplicationDto.ApplicationName)}",
            waae.apln_ver "{nameof(ControllerUriInfoByApplicationDto.ApplicationVersion)}",
            waa.envir_tp_id "{nameof(ControllerUriInfoByApplicationDto.EnvironmentType)}",
            wa.use_https "{nameof(ControllerUriInfoByApplicationDto.UseHttps)}", 
            null "{nameof(ControllerUriInfoByApplicationDto.MachineName)}",  
            waa.addr "{nameof(ControllerUriInfoByApplicationDto.Address)}", 
            wav.port "{nameof(ControllerUriInfoByApplicationDto.Port)}", 
            wac.uri_nm "{nameof(ControllerUriInfoByApplicationDto.UriName)}",
            case
                when instr(wav.ver,'.',1,2) is not null
                then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
            else
                wav.ver
            end "{nameof(ControllerUriInfoByApplicationDto.Version)}",
            waa.ord "{nameof(ControllerUriInfoByApplicationDto.Order)}"
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
        and wac.uri_nm = '{uriName}'
        and waa.envir_tp_id = {(int)environmentType}
        and waae.apln_id = {applicationId} 
        """;

        if (!string.IsNullOrEmpty(applicationVersion))
            sql += $""" and waae.apln_ver = '{applicationVersion}' """;

        var endpoints = await
            _dbContext.ControllerUriInfoByApplicationDtos
            .FromSqlRaw(sql)
            .ToListAsync(cancellationToken);

        if (!endpoints.Any())
        {
            // This query will return the endpoints for a web api the same version
            // as the application; and therefore, no need for records
            // in web_api_apln_endpt
            sql = $"""
            select distinct
                a.apln_id "{nameof(ControllerUriInfoByApplicationDto.ApplicationId)}",
                a.nm "{nameof(ControllerUriInfoByApplicationDto.ApplicationName)}",
                '{applicationVersion}' "{nameof(ControllerUriInfoByApplicationDto.ApplicationVersion)}",
                waa.envir_tp_id "{nameof(ControllerUriInfoByApplicationDto.EnvironmentType)}",
                wa.use_https "{nameof(ControllerUriInfoByApplicationDto.UseHttps)}",
                null "{nameof(ControllerUriInfoByApplicationDto.MachineName)}",
                waa.addr "{nameof(ControllerUriInfoByApplicationDto.Address)}", 
                wav.port "{nameof(ControllerUriInfoByApplicationDto.Port)}", 
                wac.uri_nm "{nameof(ControllerUriInfoByApplicationDto.UriName)}", 
                case
                    when instr(wav.ver,'.',1,2) is not null
                    then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
                else
                    wav.ver
                end "{nameof(ControllerUriInfoByApplicationDto.Version)}",
                waa.ord "{nameof(ControllerUriInfoByApplicationDto.Order)}"
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
            and wac.uri_nm = '{uriName}'
            and waa.envir_tp_id = {(int)environmentType}
            """;

            endpoints = await
                _dbContext.ControllerUriInfoByApplicationDtos
                .FromSqlRaw(sql)
                .ToListAsync(cancellationToken);
        }
        return endpoints;
    }

    public async Task<IList<ControllerUriInfoByApplicationDto>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, string machineName, CancellationToken cancellationToken)
    {
        // This query looks at web_api_apln_endpt for what endpoints to return to the
        // particular application and application version
        var sql = $"""
        select distinct
            waae.apln_id "{nameof(ControllerUriInfoByApplicationDto.ApplicationId)}",
            a.nm "{nameof(ControllerUriInfoByApplicationDto.ApplicationName)}",
            waae.apln_ver "{nameof(ControllerUriInfoByApplicationDto.ApplicationVersion)}",
            {(int)environmentType} "{nameof(ControllerUriInfoByApplicationDto.EnvironmentType)}",
            wa.use_https "{nameof(ControllerUriInfoByApplicationDto.UseHttps)}",
            '{machineName}' "{nameof(ControllerUriInfoByApplicationDto.MachineName)}",
            '{machineName}' "{nameof(ControllerUriInfoByApplicationDto.Address)}", 
            wav.port "{nameof(ControllerUriInfoByApplicationDto.Port)}", 
            wac.uri_nm "{nameof(ControllerUriInfoByApplicationDto.UriName)}",
            case
                when instr(wav.ver,'.',1,2) is not null
                then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
            else
                wav.ver
            end "{nameof(ControllerUriInfoByApplicationDto.Version)}",
            1 "{nameof(ControllerUriInfoByApplicationDto.Order)}"
        from
            cmn_mstr.web_api_ctlr wac
            join cmn_mstr.web_api_ver wav on wac.web_api_id = wav.web_api_id
            join cmn_mstr.web_api_apln_endpt waae on wav.web_api_ver_id = waae.web_api_ver_id
            join cmn_mstr.web_api wa on wav.web_api_id = wa.web_api_id
            join cmn_mstr.apln a on waae.apln_id = a.apln_id
        where
            wac.uri_nm = '{uriName}'
        and waae.apln_id = {applicationId} 
        """;

        if (!string.IsNullOrEmpty(applicationVersion))
            sql += $""" and waae.apln_ver = '{applicationVersion}' """;

        return await
            _dbContext.ControllerUriInfoByApplicationDtos
            .FromSqlRaw(sql)
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<ControllerUriFacilityInfoByApplicationDto>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, int applicationId, string applicationVersion, int facilityId, CancellationToken cancellationToken)
    {
        // This query looks at web_api_apln_endpt for what endpoints to return to the
        // particular application and application version by facility
        var sql = $"""
        select distinct
            waae.apln_id "{nameof(ControllerUriFacilityInfoByApplicationDto.ApplicationId)}",
            a.nm "{nameof(ControllerUriFacilityInfoByApplicationDto.ApplicationName)}",
            waae.apln_ver "{nameof(ControllerUriFacilityInfoByApplicationDto.ApplicationVersion)}",
            waa.envir_tp_id "{nameof(ControllerUriFacilityInfoByApplicationDto.EnvironmentType)}",
            wa.use_https "{nameof(ControllerUriFacilityInfoByApplicationDto.UseHttps)}", 
            waa.addr "{nameof(ControllerUriFacilityInfoByApplicationDto.Address)}", 
            wav.port "{nameof(ControllerUriFacilityInfoByApplicationDto.Port)}", 
            wac.uri_nm "{nameof(ControllerUriFacilityInfoByApplicationDto.UriName)}",
            case
                when instr(wav.ver,'.',1,2) is not null
                then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
            else
                wav.ver
            end "{nameof(ControllerUriFacilityInfoByApplicationDto.Version)}",
            waa.ord "{nameof(ControllerUriFacilityInfoByApplicationDto.Order)}",
            waaf.fac_id "{nameof(ControllerUriFacilityInfoByApplicationDto.FacilityId)}"
        from
            cmn_mstr.web_api_ctlr wac
            join cmn_mstr.web_api_ver wav on wac.web_api_id = wav.web_api_id
            join cmn_mstr.web_api_apln_endpt waae on wav.web_api_ver_id = waae.web_api_ver_id
            join cmn_mstr.web_api wa on wav.web_api_id = wa.web_api_id
            join cmn_mstr.web_api_addr_fac waaf on waae.fac_id = waaf.fac_id
            join cmn_mstr.web_api_addr waa on waaf.web_api_addr_id = waa.web_api_addr_id and waa.use_https = case when wa.use_https = 1 then 1 else waa.use_https end
            join cmn_mstr.web_api_addr_xref waax on wa.web_api_id = waax.web_api_id and waa.web_api_addr_id = waax.web_api_addr_id
            join cmn_mstr.apln a on waae.apln_id = a.apln_id
        where
            waa.ord is not null
        and wac.uri_nm = '{uriName}'
        and waa.envir_tp_id = {(int)environmentType}
        and waae.apln_id = {applicationId}
        and waae.fac_id = {facilityId} 
        """;

        if (!string.IsNullOrEmpty(applicationVersion))
            sql += $""" and waae.apln_ver = '{applicationVersion}' """;

        var endpoints = await
            _dbContext.ControllerUriFacilityInfoByApplicationDtos
            .FromSqlRaw(sql)
            .ToListAsync(cancellationToken);

        if (!endpoints.Any())
        {
            // This query will return the endpoints for a web api the same version
            // as the application by facility; and therefore, no need for records
            // in web_api_apln_endpt
            sql = $"""
            select distinct
                a.apln_id "{nameof(ControllerUriFacilityInfoByApplicationDto.ApplicationId)}",
                a.nm "{nameof(ControllerUriFacilityInfoByApplicationDto.ApplicationName)}",
                '{applicationVersion}' "{nameof(ControllerUriFacilityInfoByApplicationDto.ApplicationVersion)}",
                waa.envir_tp_id "{nameof(ControllerUriFacilityInfoByApplicationDto.EnvironmentType)}",
                wa.use_https "{nameof(ControllerUriFacilityInfoByApplicationDto.UseHttps)}", 
                waa.addr "{nameof(ControllerUriFacilityInfoByApplicationDto.Address)}", 
                wav.port "{nameof(ControllerUriFacilityInfoByApplicationDto.Port)}", 
                wac.uri_nm "{nameof(ControllerUriFacilityInfoByApplicationDto.UriName)}", 
                case
                    when instr(wav.ver,'.',1,2) is not null
                    then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
                else
                    wav.ver
                end "{nameof(ControllerUriFacilityInfoByApplicationDto.Version)}",
                waa.ord "{nameof(ControllerUriFacilityInfoByApplicationDto.Order)}",
                waaf.fac_id "{nameof(ControllerUriFacilityInfoByApplicationDto.FacilityId)}"
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
                join cmn_mstr.web_api_addr_fac waaf on waaf.fac_id = {facilityId}
                join cmn_mstr.web_api_addr waa on waaf.web_api_addr_id = waa.web_api_addr_id and waa.use_https = case when wa.use_https = 1 then 1 else waa.use_https end
                join cmn_mstr.web_api_addr_xref waax on wa.web_api_id = waax.web_api_id and waa.web_api_addr_id = waax.web_api_addr_id
                join cmn_mstr.apln a on a.apln_id = {applicationId}
            where
                waa.ord is not null
            and wac.uri_nm = '{uriName}'
            and waa.envir_tp_id = {(int)environmentType} 
            """;

            endpoints = await
                _dbContext.ControllerUriFacilityInfoByApplicationDtos
                .FromSqlRaw(sql)
                .ToListAsync(cancellationToken);
        }
        return endpoints;
    }

    public async Task<WebApiVersionDto?> GetOneVersionAsync(int applicationId, string applicationVersion, CancellationToken cancellationToken)
    {
        var versionLength = applicationVersion.Length;

        var sql = $"""
        select distinct
            wa.apln_id "{nameof(WebApiVersionDto.ApplicationId)}",
            wav.port "{nameof(WebApiVersionDto.Port)}",
            wa.use_https "{nameof(WebApiVersionDto.UseHttps)}",
            wav.ver "{nameof(WebApiVersionDto.Version)}",
            wa.web_api_id "{nameof(WebApiVersionDto.WebApiId)}"
        from
            cmn_mstr.web_api wa
            join cmn_mstr.web_api_ver wav on wa.web_api_id = wav.web_api_id
        where
            wa.apln_id = {applicationId}
        and substr(wav.ver,0,{versionLength}) = '{applicationVersion}'
        """;

        return await
            _dbContext.WebApiVersionDtos
            .FromSqlRaw(sql)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<string>> GetPingUrisAsync(string applicationName, string applicationVersion, CancellationToken cancellationToken)
    {
        FormattableString sql = $"""
        select
            'http://localhost:'||wav.port||'/api/'||wac.uri_nm||'/ping'
        from
            cmn_mstr.apln a
            join cmn_mstr.web_api wa on a.apln_id = wa.apln_id
            join cmn_mstr.web_api_ctlr wac on wac.web_api_id = wa.web_api_id
            join cmn_mstr.web_api_ver wav on wav.web_api_id = wa.web_api_id
        where
            a.nm = {applicationName}
        and lower(a.from_dir_nm) like '%publish'
        and wa.use_https != 1
        and wav.ver like {applicationVersion + '%'}
        and not exists (
            select
                0
            from
                cmn_mstr.apln_ver
            where
                ver = {applicationVersion}
            and lower(from_dir_nm) not like '%publish')
        """;

        return await
            _dbContext.Database
            .SqlQuery<string>(sql)
            .ToListAsync(cancellationToken);
    }
}
