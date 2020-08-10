using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using DataAccess;

namespace BusinessModel.Handlers
{
    public class MessageHandler : IBusinessAccess<MessageEntity>
    {
        public ResponseEntity<MessageEntity> Add(MessageEntity entity)
        {
            DbModel dbModel = new DbModel();
            Message dataEntity;

            if (entity == null)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }
 
            dataEntity = ConvertToDataEntity(entity);
            
            if (dataEntity == null)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullConvertedEntityError
                };
            }

            try
            {
                dbModel.Messages.Add(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageInsertError
                };
            }

            return new ResponseEntity<MessageEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(dataEntity)
            };
        }

        public ResponseEntity<MessageEntity> Delete(int id)
        {
            DbModel dbModel = new DbModel();
            Message dataEntity;
            try
            {
                dataEntity = dbModel.Messages.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageNotFound
                };
            }

            try
            {
                dbModel.Messages.Remove(dataEntity);
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageDeleteError
                };
            }

            return new ResponseEntity<MessageEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<MessageEntity> Update(MessageEntity entity)
        {
            if (entity == null)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            DbModel dbModel = new DbModel();
            Message dataEntity;

            try
            {
                dataEntity = dbModel.Messages.Find(entity.MessageId);
            }
            catch (Exception)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageNotFound
                };
            }

            try
            {
                dataEntity.ReadDate = entity.ReadDate;
                dataEntity.Status = entity.Status.ToString();
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageUpdateError
                };
            }

            return new ResponseEntity<MessageEntity>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<MessageEntity> Get(int id)
        {
            DbModel dbModel = new DbModel();
            Message entity;
            try
            {
                entity = dbModel.Messages.Find(id);

            }
            catch (Exception)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageGetError
                };
            }
            if (entity == null)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageNotFound
                };
            }

            return new ResponseEntity<MessageEntity>
            {
                CompletedRequest = true,
                Entity = ConvertToEntity(entity)
            };
        }

        private Message ConvertToDataEntity(MessageEntity messageEntity)
        {
            if (messageEntity == null)
            {
                return null;
            }

            return new Message
            {
                MessageText = messageEntity.MessageText,
                ReceiverId = messageEntity.ReceiverId,
                SenderId = messageEntity.SenderId,
                ReadDate = messageEntity.ReadDate,
                SendDate = messageEntity.SendDate,
                Status = messageEntity.Status.ToString()
            };
        }

        private MessageEntity ConvertToEntity(Message message)
        {
            if (message == null)
            {
                return null;
            }

            Enum.TryParse(message.Status, out MessageStatus result);

            return new MessageEntity
            {
                MessageId = message.MessageId,
                MessageText = message.MessageText,
                ReceiverId = message.ReceiverId,
                SenderId = message.SenderId,
                SendDate = message.SendDate,
                ReadDate = message.ReadDate,
                Status = result
            };
        }

        public ResponseEntity<ICollection<MessageEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public ResponseEntity<ICollection<MessageEntity>> GetChatHistory(int senderId, int receiverId)
        {
            DbModel dbModel = new DbModel();
            ICollection<MessageEntity> list;

            try
            {
                list = dbModel.Messages.Where(m => ((m.SenderId == senderId && m.ReceiverId==receiverId && !m.Status.Equals(MessageStatus.Archived.ToString()) && !m.Status.Equals("Deleted")) || (m.SenderId == receiverId && m.ReceiverId == senderId && !m.Status.Equals(MessageStatus.Archived.ToString()) && !m.Status.Equals("Deleted")))).OrderBy(m => m.SendDate).ToList().Select(x => ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<MessageEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageGetError
                };
            }

            return new ResponseEntity<ICollection<MessageEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        public ResponseEntity<bool> UpdateMessageStatus(int senderId, int receiverId)
        {
            DbModel dbModel = new DbModel();

            try
            {
                dbModel.Database.ExecuteSqlCommand("exec updateMessageStatus @senderId, @receiverId", new SqlParameter("@senderId", senderId), new SqlParameter("@receiverId", receiverId));
            }
            catch (Exception)
            {
                return new ResponseEntity<bool>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MatchUpdateError
                };
            }

            return new ResponseEntity<bool>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<bool> ArchiveMessage(int id)
        {

            DbModel dbModel = new DbModel();
            Message dataEntity;

            try
            {
                dataEntity = dbModel.Messages.Find(id);
            }
            catch (Exception)
            {
                return new ResponseEntity<bool>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageGetError
                };
            }

            if (dataEntity == null)
            {
                return new ResponseEntity<bool>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageNotFound
                };
            }

            try
            {
                dataEntity.Status = MessageStatus.Archived.ToString();
                dbModel.SaveChanges();
            }
            catch (Exception)
            {
                return new ResponseEntity<bool>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageUpdateError
                };
            }

            return new ResponseEntity<bool>
            {
                CompletedRequest = true
            };
        }

        public ResponseEntity<ICollection<MessageEntity>> GetAllArchivedForSenderId(int senderId)
        {
            DbModel dbModel = new DbModel();
            ICollection<MessageEntity> list;

            try
            {
                list = dbModel.Messages.Where(m => m.SenderId == senderId && m.Status.Equals(MessageStatus.Archived.ToString())).OrderBy(m => m.SendDate).ToList().Select(x => ConvertToEntity(x)).ToList();
            }
            catch (Exception)
            {
                return new ResponseEntity<ICollection<MessageEntity>>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageGetError
                };
            }

            return new ResponseEntity<ICollection<MessageEntity>>
            {
                CompletedRequest = true,
                Entity = list
            };
        }

        public ResponseEntity<bool> ArchiveAllForUsers(int user, int match)
        {

            DbModel dbModel = new DbModel();

            try
            {
                //TODO unmatched -> deleted?
                dbModel.Database.ExecuteSqlCommand("Update Messages SET Status = 'Deleted' WHERE (SenderId = @user AND ReceiverId = @match) OR (SenderId = @match AND ReceiverId = @user)", new SqlParameter("@user", user), new SqlParameter("@match", match));
            }
            catch (Exception)
            {
                return new ResponseEntity<bool>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageGetError
                };
            }

            return new ResponseEntity<bool>
            {
                CompletedRequest = true
            };
        }
    }
}

