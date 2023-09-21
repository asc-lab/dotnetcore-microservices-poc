using System;

namespace AuthService.Domain;

public record AuthResult
(
    string Token,
    string Login,
    string[] Roles,
    string Avatar,
    string UserType,
    DateTime ExpiryTimeStamp
);
