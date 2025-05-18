using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Web.Models.Settings;

namespace AgrifoodManagement.Web.Mappers
{
    public static class UpdateUserViewModelMapper
    {
        public static UpdateUserViewModel Map(UserSettingsDto dto)
        {
            var parts = (dto.Address ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .ToArray();

            return new UpdateUserViewModel
            {
                UserId = dto.UserId,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Avatar = dto.Avatar,
                PhoneNumber = dto.PhoneNumber,
                Street = parts.Length > 0 ? parts[0] : string.Empty,
                Number = parts.Length > 1 ? parts[1] : string.Empty,
                City = parts.Length > 2 ? parts[2] : string.Empty,
                Country = parts.Length > 3 ? parts[3] : string.Empty,
                Address = dto.Address
            };
        }
    }
}
