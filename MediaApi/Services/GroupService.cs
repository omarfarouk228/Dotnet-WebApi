using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Models;

namespace MediaApi.Services
{
    public class GroupService(IGroupRepository groupRepository) : IGroupService
    {
        private readonly IGroupRepository _groupRepository = groupRepository;

        public Task<IEnumerable<Group>> GetAllGroups(PaginationParams query) => _groupRepository.GetAllGroups(query);

        public Task<Group?> GetGroupById(int id) => _groupRepository.GetGroupById(id);

        public Task<Group?> GetGroupWithMedias(int id) => _groupRepository.GetGroupWithMedias(id);

        public Task<int> CreateGroup(Group group) => _groupRepository.CreateGroup(group);

        public Task<int> UpdateGroup(Group group) => _groupRepository.UpdateGroup(group);

        public Task<int> DeleteGroup(int id) => _groupRepository.DeleteGroup(id);

    }
}