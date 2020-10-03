namespace Tetra_API.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class Tetra : DbContext
    {
        public Tetra()
            : base("name=Tetra")
        {
        }

        public virtual DbSet<AdminPermission> AdminPermissions { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<BlockedAccount> BlockedAccounts { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<GroupMessage> GroupMessages { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Medium> Media { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<NotActivatedAccount> NotActivatedAccounts { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual async Task<int> ActivateMyAccount(int userID, string code)
            => await this.Database.ExecuteSqlCommandAsync($"ActivateMyAccount {userID}, {code}");

        public virtual async Task<int> AddAFriend(int userID, int friendID)
            => await this.Database.ExecuteSqlCommandAsync($"AddAFriend {userID} , {friendID}");

        public virtual async Task<int> GoOffline(int userID) 
            => await this.Database.ExecuteSqlCommandAsync($"GoOffline {userID}");

        public virtual async Task<int> GoOnline(int userID)
            => await this.Database.ExecuteSqlCommandAsync($"GoOnline {userID}");

        public virtual async Task<int> AddAnAdmin(int groupID, int requesterID, int userID)
            => await this.Database.ExecuteSqlCommandAsync($"AddAnAdmin {userID}, {requesterID}, {groupID}");

        public virtual async Task<int> AddAUser(int groupID, int requesterID, int userID)
            => await this.Database.ExecuteSqlCommandAsync($"AddAUser {groupID}, {userID}, {requesterID}");

        public virtual async Task<int> BlockAUser(int blockerID, int blockedID)
            => await this.Database.ExecuteSqlCommandAsync($"BlockAUser {blockerID}, {blockedID}");

        public virtual async Task<int> CreateAConversation(int userID, int friendID)
            => await this.Database.ExecuteSqlCommandAsync($"CreateAConversation {userID}, {friendID}");

        public virtual async Task<int> CreateAGroup(int userID, string groupName)
            => await this.Database.ExecuteSqlCommandAsync($"CreateAGroup {userID}, {groupName}");

        public virtual async Task<int> CreateAUser(User user)
            => await this.Database.ExecuteSqlCommandAsync($"CreateAUser {user.FullName}, {user.NickName}, {user.Password}, {user.Bio}, {user.Email}");


        public virtual async Task<int> DeleteAGroup(int groupID, int requesterID)
           => await this.Database.ExecuteSqlCommandAsync($"DeleteAGroup {groupID}, {requesterID}");

        public virtual async Task<int> DeleteAConversation(int groupID, int requesterID)
           => await this.Database.ExecuteSqlCommandAsync($"DeleteAConversation {groupID}, {requesterID}");

        public virtual async Task<int> DeleteFromContacts(int userID, int friendID)
           => await this.Database.ExecuteSqlCommandAsync($"DeleteFromContacts {userID}, {friendID}");

        public virtual async Task<int> DeleteMessage(int requesterID, int messageID)
           => await this.Database.ExecuteSqlCommandAsync($"DeleteMessage {requesterID}, {messageID}");

        public virtual async Task<int> DeleteUser(int userID)
           => await this.Database.ExecuteSqlCommandAsync($"DeleteUser {userID}");

        public virtual async Task<int> EditGroupDescription(int groupID, string description)
           => await this.Database.ExecuteSqlCommandAsync($"EditGroupDescription {groupID}, {description}");

        public virtual async Task<int> EditGroupName(int groupID, string name)
           => await this.Database.ExecuteSqlCommandAsync($"EditGroupName {groupID}, {name}");

        public virtual async Task<int> EditGroupPicture(int groupID, int imageID, string path, string name)
                   => await this.Database.ExecuteSqlCommandAsync($"EditGroupPicture {groupID}, {imageID}, {path} , {name}");

        public virtual async Task<int> EditUserBio(int userID, string bio)
           => await this.Database.ExecuteSqlCommandAsync($"EditUserBio {userID}, {bio}");

        public virtual async Task<int> EditUserEmail(string oldEmail, string newEmail)
           => await this.Database.ExecuteSqlCommandAsync($"EditUserEmail {oldEmail}, {newEmail}");

        public virtual async Task<int> EditUserNickName(int userID, string nickname)
           => await this.Database.ExecuteSqlCommandAsync($"EditUserNickName {userID}, {nickname}");

        public virtual async Task<int> EditUserPassword(int userID, string oldPassword, string newPassword)
           => await this.Database.ExecuteSqlCommandAsync($"EditUserPassword {userID}, {oldPassword}, {newPassword}");

        public virtual async Task<int> RemoveAnAdmin(int groupID, int requesterID, int userID)
           => await this.Database.ExecuteSqlCommandAsync($"RemoveAnAdmin {groupID}, {requesterID}, {userID}");

        public virtual async Task<int> RemoveAUser(int groupID, int requesterID, int userID)
           => await this.Database.ExecuteSqlCommandAsync($"RemoveAUser {requesterID}, {userID}, {groupID}");

        public virtual async Task<int> RemoveBlock(int blockerID, int blockedID)
           => await this.Database.ExecuteSqlCommandAsync($"RemoveBlock {blockerID}, {blockedID}");

        public virtual async Task<int> SaveMedia(int mediaID, string path, string name)
           => await this.Database.ExecuteSqlCommandAsync($"SaveMedia {mediaID}, {name}, {path}");

        public virtual async Task<int> SendMessage(int groupID, int senderID, string content, string attachmentPath, string attachmentName, int? replayedMessageID, DateTime sentTime)
           => await this.Database.ExecuteSqlCommandAsync($"SendMessage {groupID}, {senderID}, {content}, {attachmentPath}, {attachmentName}, {replayedMessageID}, {sentTime}");

        public virtual async Task<int> SetAnImage(int userID, int mediaID, string path, string name)
            => await this.Database.ExecuteSqlCommandAsync($"SetAnImage {userID}, {path}, {name}, {mediaID}");

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasMany(e => e.AdminPermissions)
                .WithOptional(e => e.Admin)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Group>()
                .Property(e => e.GroupType)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.GroupMessages)
                .WithOptional(e => e.Group)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Participants)
                .WithOptional(e => e.Group)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Medium>()
                .HasMany(e => e.Groups)
                .WithOptional(e => e.Medium)
                .HasForeignKey(e => e.GroupPicture)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Medium>()
                .HasMany(e => e.Messages)
                .WithOptional(e => e.Medium)
                .HasForeignKey(e => e.Attachment);

            modelBuilder.Entity<Medium>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Medium)
                .HasForeignKey(e => e.ProfilePicture);

            modelBuilder.Entity<Message>()
                .HasMany(e => e.GroupMessages)
                .WithOptional(e => e.Message)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Message>()
                .HasMany(e => e.Messages1)
                .WithOptional(e => e.Message1)
                .HasForeignKey(e => e.ReplyMessageID);

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.AdminPermissions)
                .WithOptional(e => e.Permission)
                .WillCascadeOnDelete();

            modelBuilder.Entity<User>()
                .Property(e => e.Activated)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.BlockedAccounts)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.BlockerID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.BlockedAccounts1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.BlockedID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Contacts)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.UserID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Contacts1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.ContactID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.GroupMessages)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.SenderID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Groups)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Owner)
                .WillCascadeOnDelete();

            modelBuilder.Entity<User>()
                .HasMany(e => e.NotActivatedAccounts)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }



    }
}
