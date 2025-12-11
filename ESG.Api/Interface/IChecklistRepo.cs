using ESG.Api.DTOs;

namespace ESG.Api.Interface
{
    public interface IChecklistRepo
    {
        Task<List<ChecklistItemDto>> GetChecklistAsync();
    }
}