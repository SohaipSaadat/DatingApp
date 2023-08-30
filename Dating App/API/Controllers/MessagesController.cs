using AutoMapper;
using DatingApplication.DTOs;
using DatingApplication.Entities;
using DatingApplication.Extentions;
using DatingApplication.Helper;
using DatingApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [TypeFilter(typeof(LogUserActivity))]
    public class MessagesController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessage createMessage)
        {
            var userName = User.GetUserName();
            if (createMessage.ReciverUserName.ToLower() == userName) return BadRequest("You can't sent message to yourself");
            
            var sender = await  _userRepository.GetUserByUserNameAsync(userName);
            var reciver = await _userRepository.GetUserByUserNameAsync(createMessage.ReciverUserName);
            if (reciver == null) return NotFound();

            var message = new Message()
            {
                Sender = sender,
                Reciver = reciver,
                SenderUserName = sender.UserName,
                ReciverUserName = reciver.UserName,
                Content = createMessage.Content
            };

           _messageRepository.AddMessage(message);
            if(await _messageRepository.SaveAll()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessageForUsers([FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUserName();
            var message = await _messageRepository.GetMessageForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(message.CurrentPage, message.PageSize, message.TotalPages, message.TotalCount));
            return message;
        }

        [HttpGet("thread/{userName}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string userName)
        {
            var currentUserName = User.GetUserName();

            return Ok(await _messageRepository.GetMessageThread(currentUserName, userName));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var userName = User.GetUserName();
            var message = await _messageRepository.GetMessage(id);

            if(message.SenderUserName != userName && message.ReciverUserName != userName) return Unauthorized();

            if(message.SenderUserName == userName) message.SenderDeleted = true;
            if(message.ReciverUserName == userName) message.ReciverDelete = true;

            if(message.SenderDeleted && message.ReciverDelete)
            {
                 _messageRepository.DeleteMessage(message);
            }

            if (await _messageRepository.SaveAll()) return Ok();

            return BadRequest("Problem deleting the message");
        }
    }
}
