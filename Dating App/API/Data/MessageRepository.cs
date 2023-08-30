using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApplication.DTOs;
using DatingApplication.Entities;
using DatingApplication.Helper;
using DatingApplication.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DatingApplication.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
           _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                        .OrderByDescending(m => m.DateSent)
                        .AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.ReciverUserName == messageParams.UserName && u.ReciverDelete == false),
                "Outbox" => query.Where(u => u.SenderUserName == messageParams.UserName && u.SenderDeleted == false),
                _ => query.Where(u => u.ReciverUserName == messageParams.UserName && u.DateRead == null && u.ReciverDelete == false)
            };
            var message = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(message, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups.Include(c => c.Connections).FirstOrDefaultAsync(n => n.Name == groupName);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string reciverUserName)
        {
            var message = await _context.Messages
                          .Include(s=> s.Sender).ThenInclude(p=> p.Photos)
                          .Include(r=> r.Reciver).ThenInclude(p=> p.Photos)
                          .Where(u=> u.ReciverUserName == currentUserName && u.SenderUserName == reciverUserName && 
                                u.ReciverDelete == false
                                 || u.SenderUserName == currentUserName && u.ReciverUserName == reciverUserName && u.SenderDeleted == false)
                          .OrderBy(ms=> ms.DateSent)
                          .ToListAsync();

            var unReadMessage = message.Where(m=> m.DateRead == null && currentUserName == m.ReciverUserName).ToList();
            if (unReadMessage.Any())
            {
                foreach (var item in unReadMessage)
                {
                    item.DateRead = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(message);  
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
