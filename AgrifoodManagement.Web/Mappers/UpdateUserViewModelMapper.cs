using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Web.Models.Settings;

namespace AgrifoodManagement.Web.Mappers
{
    public static class UpdateUserViewModelMapper
    {
        public static UpdateUserViewModel Map(UserSettingsDto dto)
        {
            return new UpdateUserViewModel
            {
                UserId = dto.UserId,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Avatar = dto.Avatar,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };
        }
    }
}
