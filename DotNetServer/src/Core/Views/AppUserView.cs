﻿using System;
using Common.Enumerations;
using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;
using Newtonsoft.Json;

namespace Core.Views
{
    [ViewName("AppUserView")]
    public class AppUserView : AuditedView
    {
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public int FailedAttemptCount { get; set; }

        [JsonIgnore]
        public Role Role { get; set; }

        public string RoleValue
        {
            get { return Role == null ? "" : Role.Value; }
            set { Role = Role.FromValue(value); }
        }

        public string RoleName
        {
            get { return Role == null ? "" : Role.DisplayName; }
            set { Role = Role.FromDisplay(value); }
        }

        [JsonIgnore]
        public UserStatus UserStatus { get; set; }

        public string UserStatusValue
        {
            get { return UserStatus == null ? "" : UserStatus.Value; }
            set { UserStatus = UserStatus.FromValue(value); }
        }

        public string UserStatusName
        {
            get { return UserStatus == null ? "" : UserStatus.DisplayName; }
            set { UserStatus = UserStatus.FromDisplay(value); }
        }

        public string PasswordRetrievalToken { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? PasswordRetrievalTokenExpirationDate { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }

        public bool IsAdmin()
        {
            return Equals(Role, Role.Owner);
        }
    }
}