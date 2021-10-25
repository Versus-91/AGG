using System.Collections.Generic;
using AFIAT.TST.Pages.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Pages.Exporting
{
    public interface ITagsExcelExporter
    {
        FileDto ExportToFile(List<GetTagForViewDto> tags);
    }
}