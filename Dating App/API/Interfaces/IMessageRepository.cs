using DatingApplication.DTOs;
using DatingApplication.Entities;
using DatingApplication.Helper;

namespace DatingApplication.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string reciverUserName);
        Task<bool> SaveAll();

        void AddGroup(Group group);
        Task<Group> GetMessageGroup(string name);
        Task<Connection> GetConnection(string connectionId);
        void RemoveConnection(Connection connection);
    }
}
