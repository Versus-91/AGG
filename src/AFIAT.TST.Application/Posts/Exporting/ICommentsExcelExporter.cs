﻿using System.Collections.Generic;
using AFIAT.TST.Posts.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Posts.Exporting
{
    public interface ICommentsExcelExporter
    {
        FileDto ExportToFile(List<GetCommentForViewDto> comments);
    }
}