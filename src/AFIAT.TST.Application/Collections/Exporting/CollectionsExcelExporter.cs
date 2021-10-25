using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using AFIAT.TST.DataExporting.Excel.NPOI;
using AFIAT.TST.Collections.Dtos;
using AFIAT.TST.Dto;
using AFIAT.TST.Storage;

namespace AFIAT.TST.Collections.Exporting
{
    public class CollectionsExcelExporter : NpoiExcelExporterBase, ICollectionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CollectionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCollectionForViewDto> collections)
        {
            return CreateExcelPackage(
                "Collections.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Collections"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("IsActive"),
                        L("CountOfItems")
                        );

                    AddObjects(
                        sheet, 2, collections,
                        _ => _.Collection.Name,
                        _ => _.Collection.Description,
                        _ => _.Collection.IsActive,
                        _ => _.Collection.CountOfItems
                        );

                });
        }
    }
}