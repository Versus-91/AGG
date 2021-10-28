using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AFIAT.TST.Posts.Dtos;
using AFIAT.TST.Dto;

namespace AFIAT.TST.Posts
{
    public interface ICommentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCommentForViewDto>> GetAll(GetAllCommentsInput input);

        Task<GetCommentForViewDto> GetCommentForView(int id);

        Task<GetCommentForEditOutput> GetCommentForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCommentDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCommentsToExcel(GetAllCommentsForExcelInput input);

        Task<PagedResultDto<CommentItemLookupTableDto>> GetAllItemForLookupTable(GetAllForLookupTableInput input);

    }
}