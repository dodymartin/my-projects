//using Microsoft.EntityFrameworkCore;
//using MinimalApi.App;
//using MinimalApi.App.Interfaces;
//using MinimalApi.Dom.Enumerations;
//using MinimalApi.Dom.Facilities.ValueObjects;
//using Stratos.Core.Query;
//using ApplicationId = MinimalApi.Dom.Applications.ValueObjects.ApplicationId;

//namespace MinimalApi.Infra.Persistence.Repositories;

//public class ControllerUriFacilityInfoByApplicationRepo : IControllerUriFacilityInfoByApplicationRepo
//{
//    private readonly IMinimalApiDbContext _dbContext;

//    public ControllerUriFacilityInfoByApplicationRepo(IMinimalApiDbContext dbContext)
//    {
//        _dbContext = dbContext;
//    }

//    public async Task<IList<ControllerUriFacilityInfoByApplication>> GetControllerUrisAsync(EnvironmentTypes environmentType, string uriName, ApplicationId applicationId, string applicationVersion, FacilityId facilityId)
//    {
//        // This query looks at web_api_apln_endpt for what endpoints to return to the
//        // particular application and application version by facility
//        var sql = $@"
//        select distinct
//            sys_guid() id,
//            waae.apln_id,
//            a.nm apln_nm,
//            waae.apln_ver,
//            waa.envir_tp_id,
//            wa.use_https, 
//            waa.addr, 
//            wav.port, 
//            wac.uri_nm,
//            case
//                when instr(wav.ver,'.',1,2) is not null
//                then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
//            else
//                wav.ver
//            end ver,
//            waa.ord,
//            waaf.fac_id
//        from
//            cmn_mstr.web_api_ctlr wac
//            join cmn_mstr.web_api_ver wav on wac.web_api_id = wav.web_api_id
//            join cmn_mstr.web_api_apln_endpt waae on wav.web_api_ver_id = waae.web_api_ver_id
//            join cmn_mstr.web_api wa on wav.web_api_id = wa.web_api_id
//            join cmn_mstr.web_api_addr_fac waaf on waae.fac_id = waaf.fac_id
//            join cmn_mstr.web_api_addr waa on waaf.web_api_addr_id = waa.web_api_addr_id and waa.use_https = case when wa.use_https = 1 then 1 else waa.use_https end
//            join cmn_mstr.web_api_addr_xref waax on wa.web_api_id = waax.web_api_id and waa.web_api_addr_id = waax.web_api_addr_id
//            join cmn_mstr.apln a on waae.apln_id = a.apln_id
//        where
//            waa.ord is not null
//            {QueryUtilities.GetWhereSql("wac.uri_nm", uriName)}
//            {QueryUtilities.GetWhereSql("waa.envir_tp_id", (int)environmentType)}
//            {QueryUtilities.GetWhereSql("waae.apln_id", applicationId.Value)}
//            {QueryUtilities.GetWhereSql("waae.fac_id", facilityId.Value)} ";

//        if (!string.IsNullOrEmpty(applicationVersion))
//            sql += QueryUtilities.GetWhereSql("waae.apln_ver", applicationVersion);

//        var endpoints = await
//            _dbContext.ControllerUriFacilityInfosByApplication
//            .FromSqlRaw(sql)
//            .ToListAsync();

//        if (!endpoints.Any())
//        {
//            // This query will return the endpoints for a web api the same version
//            // as the application by facility; and therefore, no need for records
//            // in web_api_apln_endpt
//            sql = $@"
//            select distinct
//                sys_guid() id,
//                a.apln_id,
//                a.nm apln_nm,
//                '{applicationVersion}' apln_ver,
//                waa.envir_tp_id,
//                wa.use_https, 
//                waa.addr, 
//                wav.port, 
//                wac.uri_nm, 
//                case
//                    when instr(wav.ver,'.',1,2) is not null
//                    then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
//                else
//                    wav.ver
//                end ver,
//                waa.ord,
//                waaf.fac_id
//            from
//                cmn_mstr.web_api_ctlr wac
//                join cmn_mstr.web_api wa on wac.web_api_id = wa.web_api_id
//                join cmn_mstr.web_api_ver wav on wa.web_api_id = wav.web_api_id and
//                    case
//                        when instr(wav.ver,'.',1,2) is not null
//                        then substr(wav.ver,1,instr(wav.ver,'.',1,2)-1)
//                    else
//                        wav.ver
//                    end = '{applicationVersion}'
//                join cmn_mstr.web_api_addr_fac waaf on waaf.fac_id = {facilityId.Value}
//                join cmn_mstr.web_api_addr waa on waaf.web_api_addr_id = waa.web_api_addr_id and waa.use_https = case when wa.use_https = 1 then 1 else waa.use_https end
//                join cmn_mstr.web_api_addr_xref waax on wa.web_api_id = waax.web_api_id and waa.web_api_addr_id = waax.web_api_addr_id
//                join cmn_mstr.apln a on a.apln_id = {applicationId.Value}
//            where
//                waa.ord is not null
//                {QueryUtilities.GetWhereSql("wac.uri_nm", uriName)}
//                {QueryUtilities.GetWhereSql("waa.envir_tp_id", (int)environmentType)} ";

//            endpoints = await
//                _dbContext.ControllerUriFacilityInfosByApplication
//                .FromSqlRaw(sql)
//                .ToListAsync();
//        }
//        return endpoints;
//    }
//}
