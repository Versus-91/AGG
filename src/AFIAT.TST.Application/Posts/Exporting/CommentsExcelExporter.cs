using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using AFIAT.TST.DataExporting.Excel.NPOI;
using AFIAT.TST.Posts.Dtos;
using AFIAT.TST.Dto;
using AFIAT.TST.Storage;

namespace AFIAT.TST.Posts.Exporting
{
    public class CommentsExcelExporter : NpoiExcelExporterBase, ICommentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CommentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCommentForViewDto> comments)
        {
            return CreateExcelPackage(
                "Comments.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Comments"));

                    AddHeader(
                        sheet,
                        L("Description"),
                        L("Likes"),
                        (L("Item")) + L("Title")
                        );

                    AddObjects(
                        sheet, 2, comments,
                        _ => _.Comment.Description,
                        _ => _.Comment.Likes,
                        _ => _.ItemTitle
                        );

                });
        }
    }
}