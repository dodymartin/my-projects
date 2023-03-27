using System;
using System.Collections.Generic;
using Cap.Ipfs.Base;
using Cap.Ipfs.Base.DataAccess;
using Cap.Ipfs.Corporate;

namespace Set_Json_Blob
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var qe = new QueryEngine("Prd_Corporate");
            //using var qe = new QueryEngine("Dev_Corporate");
            //using var qe = new QueryEngine("Test_Corporate_V1");
            //using var qe = new QueryEngine("Dev_Corporate_Tcd");
            //using var qe = new QueryEngine("Test_Corporate_Tcd");
            //using var qe = new QueryEngine("Qa_Corporate_Tcd");
            //using var qe = new QueryEngine("Qa2_Corporate_Tcd");
            qe.SetSessionUser("DADAMS");
            var presets = qe.Queries<FilterOptionPreset>().GetList();
            //var presets = qe.Queries<FilterOptionPresetPublic>().GetList();
            foreach (var preset in presets)
            {
                if (preset.FilterSerJsonBlob == null)
                {
                    try
                    {
                        var filters = Utilities.BlobToObject<List<ReportFilters>>(preset.FilterSerBlob);
                        if (filters?.Count > 0)
                        {
                            preset.FilterSerJsonBlob = Utilities.ObjectToBlob(filters, filters.GetTypes(), true, true);
                            using var trans = qe.BeginTransaction();
                            trans.Save(preset);
                            trans.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        try
                        {
                            var filters = new List<ReportFilters>
                            {
                                Utilities.BlobToObject<ReportFilters>(preset.FilterSerBlob)
                            };
                            if (filters?.Count > 0)
                            {
                                preset.FilterSerJsonBlob = Utilities.ObjectToBlob(filters, filters.GetTypes(), true, true);
                                using var trans = qe.BeginTransaction();
                                trans.Save(preset);
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            Console.WriteLine("Done");
        }
    }
}
