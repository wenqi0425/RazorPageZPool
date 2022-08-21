using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Services.EFServices
{
    public class EFMessageService : IMessageService
    {
        private AppDbContext _context;

        public EFMessageService(AppDbContext context)
        {
            _context = context;
        }

        public void CreateMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }


        public void SetStatusToRead(int mId)
        {
            Message message = _context.Messages.FirstOrDefault(m => m.Id == mId);
            if (message != null)
            {
                message.IsRead = true;
                _context.Update(message);
                _context.SaveChanges();
            }
        }

        public List<Message> GetMessagesByUserId(int userId)
        {
            return _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m=>m.SendingDate)
                .ToList();
        }

        public bool HasUnreadMessages(int userId)  // receiver
        {
            var messages = _context.Messages
                .AsNoTracking()
                .Where(m => m.ReceiverId == userId)
                .Where(m => m.IsRead == false)
                .ToList();
            return messages.Count > 0 ? true : false;
        }

        public void DeleteMessagesByUserId(int userId)  // store procedure 3
        {
            _context.Messages.FromSqlRaw("spDeleteMessagesByID {0}", userId)
                .ToList()
                .FirstOrDefault();
            _context.SaveChanges();
        }


    }
}
