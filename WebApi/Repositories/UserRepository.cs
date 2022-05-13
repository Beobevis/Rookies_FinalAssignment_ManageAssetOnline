namespace WebApi.Repositories;

using System;
using BCrypt.Net;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Options;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

public interface IUserRepository
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(int id);
    Task UpdatePasswordFirstTime(ChangePasswordRequest user);
    Task<CreateUserResponse> CreateUser(UserCreateModel user, string Location, int adminid);
    Task UpdateUser(UserUpdateModel user, string adminlocation);
    Task<bool> CheckTokenLoggedout(string token);
    Task Logout(DateTime exp, string token);
    Task DeleteUser(int id);
    Task DeleteExpirationDateToken();
    Task UpdatePassword(string oldpassword, string newpassword, int userid);


}


public class UserRepository : IUserRepository
{
    private DataContext _context;
    private IJwtUtils _jwtUtils;
    private readonly AppSettings _appSettings;

    public UserRepository(
        DataContext context,
        IJwtUtils jwtUtils,
        IOptions<AppSettings> appSettings)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
    }


    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        if (model.Username == null) throw new AppException("Username is required");
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == model.Username.Trim());

        // validate
        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
            throw new AppException("Username or password is incorrect");

        //check disable user
        if (user.IsDisabled == true) throw new AppException("User is disabled");

        // authentication successful so generate jwt token
        string jwtToken = _jwtUtils.GenerateJwtToken(user);

        return new AuthenticateResponse(user, jwtToken);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.Where(u => u.IsDisabled == false).ToListAsync();
    }

    public async Task<User> GetById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        if (user.IsDisabled == true) throw new AppException("User is disabled");
        return user;
    }


    public async Task UpdatePasswordFirstTime(ChangePasswordRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        var currentUser = await _context.Users.FindAsync(request.Id);
        if (currentUser == null) throw new ArgumentNullException(nameof(currentUser));
        if (currentUser.IsDisabled == true) throw new AppException("User is disabled");
        if (currentUser.IsFirstLogin == false) throw new AppException("You already changed your password");
        if (request.NewPassword == null) throw new AppException("New password is required");
        if (CheckChangePwd(request.NewPassword))
        {
            currentUser.PasswordHash = BCryptNet.HashPassword(request.NewPassword);
            currentUser.UpdateAt = DateTime.Now; // optional
            currentUser.UpdateBy = currentUser.Id; // optional
            currentUser.IsFirstLogin = (currentUser.IsFirstLogin && false);
            _context.Users.Update(currentUser);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new AppException("Password should be at least 8 characters and contain at least one number and have a mixture of uppercase and lowercase letters");
        }
    }

    public async Task UpdatePassword(string oldpassword, string newpassword, int userid)
    {
        if (String.IsNullOrEmpty(oldpassword) || String.IsNullOrEmpty(newpassword) || userid == 0) throw new ArgumentNullException();
        var currentUser = await _context.Users.FindAsync(userid);
        if (currentUser == null) throw new ArgumentNullException(nameof(currentUser));
        if (currentUser.IsDisabled == true) throw new AppException("User is disabled");
        if (!BCrypt.Verify(oldpassword, currentUser.PasswordHash)) throw new AppException("Your password is incorrect");
        if (oldpassword.Equals(newpassword)) throw new AppException("New password should be different from old password");
        if (!CheckChangePwd(newpassword)) throw new AppException("Password should be at least 8 characters and contain at least one number and have a mixture of uppercase and lowercase letters");

        currentUser.PasswordHash = BCryptNet.HashPassword(newpassword);
        currentUser.UpdateAt = DateTime.Now;
        currentUser.UpdateBy = userid;
        _context.Users.Update(currentUser);
        await _context.SaveChangesAsync();
    }

    public async Task<CreateUserResponse> CreateUser(UserCreateModel user, string location, int adminid)
    {
        var checkFirstname = true;
        var checkLastname = true;
        var checkWD = CheckJoinedDateWeekDay(user.JoinDate);
        var checkJDBod = CheckJoinedDateAndDoB(user.JoinDate, user.DoB);
        var checkDob = CheckAgeIsFrom18(user.DoB);
        var checkJD = CheckTimeSpanFrom18Years(user.DoB, user.JoinDate);

        if (user.Firstname != null) checkFirstname = CheckName(user.Firstname);
        if (user.Firstname != null && user.Firstname.Trim().Equals("")) throw new AppException("Firstname should not has only whitespace");
        if (user.Lastname != null && user.Lastname.Trim().Equals("")) throw new AppException("Lastname should not has only whitespace");
        if (user.Lastname != null) checkLastname = CheckName(user.Lastname);
        if (user.Type == null || user.Gender == null) throw new AppException("User's type and gender are required");


        if (!checkWD) throw new AppException("Joined date on Saturday or Sunday, please choose other date");
        if (!checkJDBod) throw new AppException("Join date is earlier than date of birth, please choose other date");
        if (!checkDob) throw new AppException("User must be greater than 18 Years old");
        if (!checkFirstname) throw new AppException("Firstname should be letters dot(.) apostrophe(') and space only");
        if (!checkLastname) throw new AppException("Lastname should be letters dot(.) apostrophe(') and space only");
        if (!checkJD) throw new AppException("Only accept staff from 18 years old");

        if (checkWD && checkJDBod && checkDob && checkFirstname && checkLastname && checkJD)
        {
            var username = GenerateUsername(user.Firstname, user.Lastname);
            var userEntity = new User
            {
                StaffCode = GenerateStaffCode(),
                Firstname = user.Firstname != null ? user.Firstname.Trim() : throw new AppException("Firstname is required"),
                Lastname = user.Lastname != null ? user.Lastname.Trim() : throw new AppException("Lastname is required"),
                Username = username,
                PasswordHash = BCryptNet.HashPassword(GeneratePassword(username, user.DoB)),
                JoinDate = user.JoinDate,
                Type = user.Type.Trim().Equals("Admin") ? Role.Admin : Role.Staff,
                DoB = user.DoB,
                Gender = GenerateGender(user.Gender),
                IsDisabled = false,
                IsFirstLogin = true,
                Location = location,
                CreateAt = DateTime.Now,
                CreateBy = adminid,
                UpdateAt = null,
                UpdateBy = null,
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return new CreateUserResponse
            {
                Username = username,
                Password = GeneratePassword(username, user.DoB)
            };
        }
        throw new AppException("Create user failed");
    }

    public async Task UpdateUser(UserUpdateModel user, string adminlocation)
    {
        var currentUser = await _context.Users.FindAsync(user.Id);

        if (currentUser == null) throw new AppException("User not found");
        if (currentUser.IsDisabled == true) throw new AppException("User is disabled");
        if (currentUser.Location == null) throw new AppException("User location is not valid");
        if (currentUser.Username == null) throw new AppException("User username is not valid");
        if (!currentUser.Location.Equals(adminlocation)) throw new AppException("Admin does not have permission to change Staff at diferrent location");

        var checkWD = CheckJoinedDateWeekDay(user.JoinDate);
        var checkJDBod = CheckJoinedDateAndDoB(user.JoinDate, user.DoB);
        var checkDob = CheckAgeIsFrom18(user.DoB);
        var checkJD = CheckTimeSpanFrom18Years(user.DoB, user.JoinDate);

        if (user.Type == null || user.Gender == null) throw new AppException("User type and gender are required");
        if (!checkWD) throw new AppException("Joined date on Saturday or Sunday, please choose other date");
        if (!checkJDBod) throw new AppException("Join date is earlier than date of birth, please choose other date");
        if (!checkDob) throw new AppException("User must be greater than 18 Years old");
        if (!checkJD) throw new AppException("Only accept staff from 18 years old");
        if (checkWD && checkJDBod && checkDob && checkJD)
        {
            currentUser.JoinDate = user.JoinDate;
            currentUser.Type = user.Type.Equals("Admin") ? Role.Admin : Role.Staff;
            currentUser.DoB = user.DoB;
            currentUser.Gender = GenerateGender(user.Gender);
            currentUser.UpdateAt = DateTime.Now; // optional
            if (currentUser.IsFirstLogin == true) currentUser.PasswordHash = BCryptNet.HashPassword(GeneratePassword(currentUser.Username, user.DoB));

            _context.Users.Update(currentUser);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CheckTokenLoggedout(string token)
    {
        return await _context.TokenLogouts.AnyAsync(t => t.Token.Equals(token));
    }

    public async Task Logout(DateTime exp, string token)
    {
        var check = await _context.TokenLogouts.AnyAsync(t => t.Token.Equals(token));
        if (check) throw new AppException("You are already logged out");
        await _context.TokenLogouts.AddAsync(new TokenLogout
        {
            ExpirationDate = exp,
            Token = token
        });
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(int id) // actually disable user
    {
        var user = await _context.Users.FindAsync(id);
        var checkAssignment = await _context.Assignments.AnyAsync(a => a.AssignToId == id && ((a.State == AssignmentState.Accepted && a.IsInProgress == true) || a.State == AssignmentState.WaitingForAcceptance));
        if (checkAssignment == true) throw new AppException("User has assignment, please delete assignment or return assignment first");
        if (user == null) throw new AppException("User does not exist");
        if (user.IsDisabled == true) throw new AppException("User is already disabled");
        user.IsDisabled = true;

        _context.Users.Update(user);
        //_context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExpirationDateToken()
    {
        var tokenLogouts = await _context.TokenLogouts.ToListAsync();
        foreach (var token in tokenLogouts)
        {
            if (DateTime.Compare(token.ExpirationDate, DateTime.Now) < 0)
            {
                _context.TokenLogouts.Remove(token);
            }
        }
        await _context.SaveChangesAsync();
    }

    private string GenerateStaffCode()
    {
        var lastUserId = _context.Users?.OrderByDescending(o => o.Id).FirstOrDefault()?.Id + 1;
        var userId = "SD" + String.Format("{0,0:D4}", lastUserId++);
        return userId;
    }

    private string GenerateUsername(string? firstname, string? lastname)
    {
        var prefix = "";
        var postfix = "";
        if (firstname == null)
        {
            prefix = "";
        }
        else
        {
            var firstnames = firstname.Trim().Split(' ');
            foreach (var fn in firstnames)
            {
                prefix += fn.Trim();
            }
        }

        if (lastname == null)
        {
            postfix = "";
        }
        else
        {
            var lastnames = lastname.Trim().Split(' ');
            foreach (var ln in lastnames)
            {
                if (ln != "") postfix += ln.Trim().Substring(0, 1);
            }
        }

        var rawusername = (prefix + postfix).ToLower();

        //generate code
        var check = _context.Users.Any(o => o.Username.Equals(rawusername));
        if (check)
        {
            var postNumber = 0;
            var flag = true;
            var username = "";
            do
            {
                postNumber++;
                username = rawusername + postNumber.ToString();
                flag = CheckUsernameDb(username);
            } while (flag);
            return username;
        }
        else
        {
            return rawusername;
        }

    }

    private string GeneratePassword(string username, DateTime dob)
    {
        return username + "@" + dob.ToString("ddMMyyyy");
    }

    private bool CheckUsernameDb(string username)
    {
        if (_context.Users.Any(o => o.Username.Equals(username)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckJoinedDateWeekDay(DateTime joindate)
    {
        if (joindate.DayOfWeek == DayOfWeek.Sunday || joindate.DayOfWeek == DayOfWeek.Saturday)
        {
            return false;
        }
        return true;
    }

    private bool CheckJoinedDateAndDoB(DateTime joindate, DateTime dob)
    {
        if (DateTime.Compare(joindate, dob) <= 0)
        {
            return false;
        }
        return true;
    }

    private bool CheckTimeSpanFrom18Years(DateTime beforedate, DateTime afterdate)
    {
        var newdate = beforedate.AddYears(18);
        if (DateTime.Compare(newdate, afterdate) <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckAgeIsFrom18(DateTime dob)
    {
        return CheckTimeSpanFrom18Years(dob, DateTime.Now);
    }

    private string GenerateGender(string gender)
    {
        if (gender.Trim().ToLower().Equals("male")) return "Male";
        return "Female";
    }

    private bool CheckChangePwd(string newpwd)
    {
        Regex rgx = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
        if (rgx.IsMatch(newpwd))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // check first and last name condition
    private bool CheckName(string name)
    {
        Regex rgx = new Regex(@"^[a-zA-Z.'\s]*$");
        if (rgx.IsMatch(name.Trim()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}