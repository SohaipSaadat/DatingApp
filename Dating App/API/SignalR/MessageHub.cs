using AutoMapper;
using DatingApplication.DTOs;
using DatingApplication.Entities;
using DatingApplication.Extentions;
using DatingApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.AccessControl;

namespace DatingApplication.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;

        public MessageHub(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper, IHubContext<PresenceHub> presenceHub)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _presenceHub = presenceHub;
        }
        public override async Task OnConnectedAsync()
        {

            var httpContext = Context.GetHttpContext();

            var otherUser = httpContext?.Request.Query["user"];

            var groupName = GetGroupName(Context.User?.GetUserName(), otherUser);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await AddToGroup(groupName);

            var message = await _messageRepository.GetMessageThread(Context.User.GetUserName(), otherUser);

            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", message);
           
        }

        public async Task SendMessage(CreateMessage message)
        {
            var userName = Context.User.GetUserName();

            if (message.ReciverUserName.ToLower() == userName) throw new HubException("You can not send message to yourself");

            var sender = await _userRepository.GetUserByUserNameAsync(userName);

            var recevier = await _userRepository.GetUserByUserNameAsync(message.ReciverUserName);

            if (recevier is null) throw new HubException("Not found");

            var newMessage = new Message()
            {
                Sender = sender,
                Reciver = recevier,
                ReciverUserName = message.ReciverUserName,
                SenderUserName = userName,
                Content = message.Content
            };

            var groupName = GetGroupName(sender.UserName, recevier.UserName);

            var group =  await _messageRepository.GetMessageGroup(groupName);

            if(group.Connections.Any(c=> c.UserName == recevier.UserName))
            {
                newMessage.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connection = await PresenceTracker.GetConnectionForUsers(recevier.UserName);
                if(connection is not null)
                {
                    await _presenceHub.Clients.Clients(connection).SendAsync("newMessageRecevied", new {id = sender.Id, knownAs = sender.KnownAs });
                }
            }
            _messageRepository.AddMessage(newMessage);

            if(await _messageRepository.SaveAll() )
            {
                
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(newMessage));
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await RemoveFromGroupMessage();
            await base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private async Task<bool> AddToGroup(string groupName)
        {
            var group = await _messageRepository.GetMessageGroup(groupName);

            var connection = new Connection(Context.ConnectionId, Context.User.GetUserName());

            if (group == null)
            {
                group = new Group(groupName);
                _messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            return await _messageRepository.SaveAll();
        }
       private async Task RemoveFromGroupMessage()
        {
            var connection = await _messageRepository.GetConnection(Context.ConnectionId);
             _messageRepository.RemoveConnection(connection);
            await _messageRepository.SaveAll();
        }
    }
}
