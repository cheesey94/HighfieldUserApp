using API.Models;
using API.Services.Interfaces;
using API.Utilities;
using System.Text.Json;

namespace API.Services
{
    #region Service result types
    public abstract class UserDataResult { }

    public sealed class UserInfoSuccessResult(IEnumerable<User> users, IEnumerable<UserAgePlusTwenty> userAgePlusTwenty, IEnumerable<UserTopColours> userTopColours) : UserDataResult
    {
        public IEnumerable<User> Users { get; } = users;
        public IEnumerable<UserAgePlusTwenty> UserAgesPlusTwenty { get; } = userAgePlusTwenty;
        public IEnumerable<UserTopColours> UserTopColours { get; } = userTopColours;
    }
    public sealed class UserDataFailureResult : UserDataResult { }

    #endregion

    #region DTOs

    public class UserDto
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime Dob { get; set; }
        public string? FavouriteColour { get; set; }
    }
    #endregion

    public class UserService(IHttpClientService httpClientService, JsonSerializerOptions jsonOptions)
    {
        public async Task<ApiResult<UserDataResult>> GetUsersInfo()
        {
            return await GetUsersInfoHelperAsync().SafeApiCall();
        }

        public async Task<UserDataResult> GetUsersInfoHelperAsync()
        {
            var httpClient = httpClientService.CreateClient();
            var endpoint = "api/test";

            var response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var userResponseDataList = JsonSerializer.Deserialize<List<UserDto>>(jsonResponse, jsonOptions);

                if (userResponseDataList != null)
                {
                    var users = new List<User>();
                    var userAgePlusTwenty = new List<UserAgePlusTwenty>();
                    var userTopColours = new List<UserTopColours>();

                    foreach (var userDto in userResponseDataList)
                    {
                        var user = new User
                        {
                            Id = userDto.Id,
                            FirstName = userDto.FirstName,
                            LastName = userDto.LastName,
                            Email = userDto.Email,
                            DateOfBirth = userDto.Dob,
                            FavouriteColour = userDto.FavouriteColour
                        };

                        users.Add(user);

                        var agePlusTwenty = CalculateAge(userDto.Dob) + 20;
                        userAgePlusTwenty.Add(new UserAgePlusTwenty
                        {
                            UserId = userDto.Id,
                            OriginalAge = CalculateAge(userDto.Dob),
                            AgePlusTwenty = agePlusTwenty
                        });

                        userTopColours.Add(new UserTopColours
                        {
                            Colour = userDto.FavouriteColour,
                            Count = 1
                        });
                    }
                    var colourFrequency = userTopColours
                        .GroupBy(c => c.Colour)
                        .Select(g => new UserTopColours
                        {
                            Colour = g.Key,
                            Count = g.Count()
                        })
                        .OrderByDescending(c => c.Count)
                        .ThenBy(c => c.Colour);

                    return new UserInfoSuccessResult(users, userAgePlusTwenty, colourFrequency);
                }
                else
                {
                    return new UserDataFailureResult();
                }
            }
            else
            {
                return new UserDataFailureResult();
            }
        }
        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
