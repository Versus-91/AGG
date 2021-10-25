using System.Collections.Generic;
using AFIAT.TST.Collections.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Collections.Exporting
{
    public interface ICollectionsExcelExporter
    {
        FileDto ExportToFile(List<GetCollectionForViewDto> collections);
    }
}