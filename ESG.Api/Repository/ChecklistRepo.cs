using ESG.Api.Data;
using ESG.Api.DTOs;
using ESG.Api.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESG.Api.Repository
{
    public class ChecklistRepo : IChecklistRepo
    {
        private readonly AppDbContext _context;

        public ChecklistRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ChecklistItemDto>> GetChecklistAsync()
        {
            var items = await _context.ESG_CHECKLIST_ITEM
                //.Include(i => i.RESPONSETYPEID)
                //.Include(i => i.ESG_CHECKLIST_SCORE)
                .Select(item => new ChecklistItemDto
                {
                    ChecklistItemId = item.CHECKLISTITEMID,
                    Item = item.CHECKLISTITEM,
                    Category = item.CATEGORY,
                    indicatorType = item.INDICATORTYPE,
                    Weight = item.WEIGHT,
                    ResponseTypeId = item.RESPONSETYPEID,
                    ResponseType = item.RESPONSETYPEID == 1 ? "Yes/No" : "Low/Medium/high",
                    Responses = _context.ESG_CHECKLIST_ITEM_SCORE.Where(s => s.CHECKLISTITEMID == item.CHECKLISTITEMID)
                        .OrderBy(s => s.CHECKLISTITEMID)
                        .Select(s => new ChecklistResponseValueDto
                        {
                            ResponseValue = s.RESPONSETYPEVALUE,
                            Score = s.SCORE,
                            Label = item.RESPONSETYPEID == 1
                                ? (s.RESPONSETYPEVALUE == 1 ? "Yes" : "No")
                                : (s.RESPONSETYPEVALUE == 1 ? "Low"
                                    : s.RESPONSETYPEVALUE == 2 ? "Medium" : "High")
                        }).ToList()
                })
                .ToListAsync();

            return items;
        }
    }
}