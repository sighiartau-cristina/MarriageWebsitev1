using System;
using System.Collections.Generic;
using System.Linq;
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

            try
            {
                
                dataEntity = ConvertToDataEntity(entity);
                if (dataEntity == null)
                {
                    return new ResponseEntity<MessageEntity>
                    {
                        CompletedRequest = false,
                        ErrorMessage = ErrorConstants.NullConvertedEntityError
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseEntity<MessageEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.MessageGetError
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

        public ResponseEntity<ICollection<MessageEntity>> GetAllForSenderId(int senderId)
        {
            DbModel dbModel = new DbModel();
            ICollection<MessageEntity> list;

            try
            {
                list = dbModel.Messages.Where(m => m.SenderId==senderId).OrderBy(m => m.SendDate).ToList().Select(x => ConvertToEntity(x)).ToList();
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

        /*private bool CheckExisting(string name)
        {
            DbModel dbModel = new DbModel();
            var entity = dbModel.Genders.Where(e => e.GenderName == name).FirstOrDefault();
            return entity != null;
        }*/

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
                ReadDate = message.ReadDate,
                SendDate = message.SendDate,
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
                list = dbModel.Messages.Where(m => (m.SenderId == senderId && m.ReceiverId==receiverId) || (m.SenderId == receiverId && m.ReceiverId == senderId)).OrderBy(m => m.SendDate).ToList().Select(x => ConvertToEntity(x)).ToList();
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
    }
}

