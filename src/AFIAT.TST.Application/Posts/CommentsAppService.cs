using AFIAT.TST.Pages;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using AFIAT.TST.Posts.Exporting;
using AFIAT.TST.Posts.Dtos;
using AFIAT.TST.Dto;
using Abp.Application.Services.Dto;
using AFIAT.TST.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using AFIAT.TST.Storage;

namespace AFIAT.TST.Posts
{
    [AbpAuthorize(AppPermissions.Pages_Comments)]
    public class CommentsAppService : TSTAppServiceBase, ICommentsAppService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly ICommentsExcelExporter _commentsExcelExporter;
        private readonly IRepository<Item, int> _lookup_itemRepository;

        public CommentsAppService(IRepository<Comment> commentRepository, ICommentsExcelExporter commentsExcelExporter, IRepository<Item, int> lookup_itemRepository)
        {
            _commentRepository = commentRepository;
            _commentsExcelExporter = commentsExcelExporter;
            _lookup_itemRepository = lookup_itemRepository;

        }

        public async Task<PagedResultDto<GetCommentForViewDto>> GetAll(GetAllCommentsInput input)
        {

            var filteredComments = _commentRepository.GetAll()
                        .Include(e => e.ItemFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.MinLikesFilter != null, e => e.Likes >= input.MinLikesFilter)
                        .WhereIf(input.MaxLikesFilter != null, e => e.Likes <= input.MaxLikesFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemTitleFilter), e => e.ItemFk != null && e.ItemFk.Title == input.ItemTitleFilter);

            var pagedAndFilteredComments = filteredComments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var comments = from o in pagedAndFilteredComments
                           join o1 in _lookup_itemRepository.GetAll() on o.ItemId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           select new
                           {

                               o.Description,
                               o.Likes,
                               Id = o.Id,
                               ItemTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString()
                           };

            var totalCount = await filteredComments.CountAsync();

            var dbList = await comments.ToListAsync();
            var results = new List<GetCommentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCommentForViewDto()
                {
                    Comment = new CommentDto
                    {

                        Description = o.Description,
                        Likes = o.Likes,
                        Id = o.Id,
                    },
                    ItemTitle = o.ItemTitle
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCommentForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCommentForViewDto> GetCommentForView(int id)
        {
            var comment = await _commentRepository.GetAsync(id);

            var output = new GetCommentForViewDto { Comment = ObjectMapper.Map<CommentDto>(comment) };

            if (output.Comment.ItemId != null)
            {
                var _lookupItem = await _lookup_itemRepository.FirstOrDefaultAsync((int)output.Comment.ItemId);
                output.ItemTitle = _lookupItem?.Title?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Comments_Edit)]
        public async Task<GetCommentForEditOutput> GetCommentForEdit(EntityDto input)
        {
            var comment = await _commentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCommentForEditOutput { Comment = ObjectMapper.Map<CreateOrEditCommentDto>(comment) };

            if (output.Comment.ItemId != null)
            {
                var _lookupItem = await _lookup_itemRepository.FirstOrDefaultAsync((int)output.Comment.ItemId);
                output.ItemTitle = _lookupItem?.Title?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCommentDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Comments_Create)]
        protected virtual async Task Create(CreateOrEditCommentDto input)
        {
            var comment = ObjectMapper.Map<Comment>(input);

            if (AbpSession.TenantId != null)
            {
                comment.TenantId = (int?)AbpSession.TenantId;
            }

            await _commentRepository.InsertAsync(comment);

        }

        [AbpAuthorize(AppPermissions.Pages_Comments_Edit)]
        protected virtual async Task Update(CreateOrEditCommentDto input)
        {
            var comment = await _commentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, comment);

        }

        [AbpAuthorize(AppPermissions.Pages_Comments_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _commentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCommentsToExcel(GetAllCommentsForExcelInput input)
        {

            var filteredComments = _commentRepository.GetAll()
                        .Include(e => e.ItemFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.MinLikesFilter != null, e => e.Likes >= input.MinLikesFilter)
                        .WhereIf(input.MaxLikesFilter != null, e => e.Likes <= input.MaxLikesFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemTitleFilter), e => e.ItemFk != null && e.ItemFk.Title == input.ItemTitleFilter);

            var query = (from o in filteredComments
                         join o1 in _lookup_itemRepository.GetAll() on o.ItemId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetCommentForViewDto()
                         {
                             Comment = new CommentDto
                             {
                                 Description = o.Description,
                                 Likes = o.Likes,
                                 Id = o.Id
                             },
                             ItemTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString()
                         });

            var commentListDtos = await query.ToListAsync();

            return _commentsExcelExporter.ExportToFile(commentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Comments)]
        public async Task<PagedResultDto<CommentItemLookupTableDto>> GetAllItemForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_itemRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var itemList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CommentItemLookupTableDto>();
            foreach (var item in itemList)
            {
                lookupTableDtoList.Add(new CommentItemLookupTableDto
                {
                    Id = item.Id,
                    DisplayName = item.Title?.ToString()
                });
            }

            return new PagedResultDto<CommentItemLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}