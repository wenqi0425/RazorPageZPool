using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZPool.Models;

namespace ZPool.Services.Interfaces
{
    public interface IMessageService
    {
        void CreateMessage(Message message);
        List<Message> GetMessagesByUserId(int userId);
        bool HasUnreadMessages(int userId);
        void SetStatusToRead(int mId);
        void DeleteMessagesByUserId(int userId);
    }
}
