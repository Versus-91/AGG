﻿using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using AFIAT.TST.DynamicEntityProperties.Dto;

namespace AFIAT.TST.DynamicEntityProperties
{
    public interface IDynamicEntityPropertyAppService
    {
        Task<DynamicEntityPropertyDto> Get(int id);

        Task<ListResultDto<DynamicEntityPropertyDto>> GetAllPropertiesOfAnEntity(DynamicEntityPropertyGetAllInput input);


        Task<ListResultDto<DynamicEntityPropertyDto>> GetAll();

        Task Add(DynamicEntityPropertyDto dto);

        Task Update(DynamicEntityPropertyDto dto);

        Task Delete(int id);
        
        Task<ListResultDto<GetAllEntitiesHasDynamicPropertyOutput>> GetAllEntitiesHasDynamicProperty();
    }
}
