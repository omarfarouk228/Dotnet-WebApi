using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1.Data.Interfaces
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAllGroups(PaginationParams query);
        Task<Group?> GetGroupById(int id);
        Task<Group?> GetGroupWithMedias(int id);
        Task<int> CreateGroup(Group group);
        Task<int> UpdateGroup(Group group);
        Task<int> DeleteGroup(int id);
    }
}