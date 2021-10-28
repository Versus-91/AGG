using AFIAT.TST.Pages;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
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
    [AbpAuthorize(AppPermissions.Pages_Posts)]
    public class PostsAppService : TSTAppServiceBase, IPostsAppService
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Item, int> _lookup_itemRepository;

        public PostsAppService(IRepository<Post> postRepository, IRepository<Item, int> lookup_itemRepository)
        {
            _postRepository = postRepository;
            _lookup_itemRepository = lookup_itemRepository;

        }

        public async Task<PagedResultDto<GetPostForViewDto>> GetAll(GetAllPostsInput input)
        {

            var filteredPosts = _postRepository.GetAll()
                        .Include(e => e.ItemFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.IsActive.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IsActiveFilter), e => e.IsActive == input.IsActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemTitleFilter), e => e.ItemFk != null && e.ItemFk.Title == input.ItemTitleFilter);

            var pagedAndFilteredPosts = filteredPosts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var posts = from o in pagedAndFilteredPosts
                        join o1 in _lookup_itemRepository.GetAll() on o.ItemId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        select new
                        {

                            o.Title,
                            o.Description,
                            o.IsActive,
                            Id = o.Id,
                            ItemTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString()
                        };

            var totalCount = await filteredPosts.CountAsync();

            var dbList = await posts.ToListAsync();
            var results = new List<GetPostForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPostForViewDto()
                {
                    Post = new PostDto
                    {

                        Title = o.Title,
                        Description = o.Description,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    },
                    ItemTitle = o.ItemTitle
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPostForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPostForViewDto> GetPostForView(int id)
        {
            var post = await _postRepository.GetAsync(id);

            var output = new GetPostForViewDto { Post = ObjectMapper.Map<PostDto>(post) };

            if (output.Post.ItemId != null)
            {
                var _lookupItem = await _lookup_itemRepository.FirstOrDefaultAsync((int)output.Post.ItemId);
                output.ItemTitle = _lookupItem?.Title?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Posts_Edit)]
        public async Task<GetPostForEditOutput> GetPostForEdit(EntityDto input)
        {
            var post = await _postRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPostForEditOutput { Post = ObjectMapper.Map<CreateOrEditPostDto>(post) };

            if (output.Post.ItemId != null)
            {
                var _lookupItem = await _lookup_itemRepository.FirstOrDefaultAsync((int)output.Post.ItemId);
                output.ItemTitle = _lookupItem?.Title?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPostDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Posts_Create)]
        protected virtual async Task Create(CreateOrEditPostDto input)
        {
            var post = ObjectMapper.Map<Post>(input);

            if (AbpSession.TenantId != null)
            {
                post.TenantId = (int?)AbpSession.TenantId;
            }

            await _postRepository.InsertAsync(post);

        }

        [AbpAuthorize(AppPermissions.Pages_Posts_Edit)]
        protected virtual async Task Update(CreateOrEditPostDto input)
        {
            var post = await _postRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, post);

        }

        [AbpAuthorize(AppPermissions.Pages_Posts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _postRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Posts)]
        public async Task<PagedResultDto<PostItemLookupTableDto>> GetAllItemForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_itemRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var itemList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PostItemLookupTableDto>();
            foreach (var item in itemList)
            {
                lookupTableDtoList.Add(new PostItemLookupTableDto
                {
                    Id = item.Id,
                    DisplayName = item.Title?.ToString()
                });
            }

            return new PagedResultDto<PostItemLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}