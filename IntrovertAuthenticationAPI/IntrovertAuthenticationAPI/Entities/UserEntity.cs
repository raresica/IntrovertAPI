/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace IntrovertAuthenticationAPI.Entities
{
    public class UserEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("passwordHash")]
        public byte[] PasswordHash { get; set; }

        [BsonElement("passwordSalt")]
        public byte[] PasswordSalt { get; set; }

        [BsonElement("updatedOn")]
        public DateTime UpdatedOn { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }
    }
}
