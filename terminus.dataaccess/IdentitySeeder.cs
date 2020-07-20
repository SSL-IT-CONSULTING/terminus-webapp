using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using terminus.shared.models;

namespace terminus.dataaccess
{
    public class IdentitySeeder
    {
        private UserManager<AppUser> _userMgr;
        private RoleManager<AppRole> _roleMgr;
        private AppDBContext _context;

        public IdentitySeeder(UserManager<AppUser> userMgr, RoleManager<AppRole> roleMgr, AppDBContext context)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
            _context = context;
        }

        public async Task Seed()
        {
            try
            {
               if(!_context.Companies.Any())
                {
                    _context.Companies.Add(new Company() { companyId = "ASRC", companyName = "ASRC" });
                    _context.Companies.Add(new Company() { companyId = "ADBCA", companyName = "ADBCA" });
                    _context.Companies.Add(new Company() { companyId = "APMI", companyName = "APMI" });

                    await _context.SaveChangesAsync();

                }


                //add roles
                if (!(await _roleMgr.RoleExistsAsync("admin")))
                {
                    var role = new AppRole();
                    role.Name = "admin";
                    await _roleMgr.CreateAsync(role);
                }

                if (!(await _roleMgr.RoleExistsAsync("users")))
                {
                    var role = new AppRole();
                    role.Name = "users";
                    await _roleMgr.CreateAsync(role);
                }

                var user = await _userMgr.FindByNameAsync("ASRC_Admin");
                // Add User
                if (user == null)
                {
                    var company = _context.Companies.Where(c=>c.companyId.Equals("ASRC")).FirstOrDefault();

                    user = new AppUser()
                    {
                        UserName = "ASRC_Admin",
                        firstName = "Admin",
                        lastName = "Terminus",

                        Email = "pvlucban81@yahoo.com",
                        company = company
                    };

                    var userResult = await _userMgr.CreateAsync(user, "P@ssw0rd!");
                    var roleResult = await _userMgr.AddToRoleAsync(user, "admin");


                    var claimResult = await _userMgr.AddClaimAsync(user, new Claim("SuperUser", "True"));

                    if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to build user and roles");
                    }
                }


                var user2 = await _userMgr.FindByNameAsync("ADBCA_Admin");
                // Add User
                if (user == null)
                {
                    var company = _context.Companies.Where(c => c.companyId.Equals("ADBCA")).FirstOrDefault();

                    user = new AppUser()
                    {
                        UserName = "ADBCA_Admin",
                        firstName = "Admin",
                        lastName = "Terminus",

                        Email = "pvlucban81@gmail.com",
                        company = company
                    };

                    var userResult = await _userMgr.CreateAsync(user, "P@ssw0rd!");
                    var roleResult = await _userMgr.AddToRoleAsync(user, "admin");


                    var claimResult = await _userMgr.AddClaimAsync(user, new Claim("SuperUser", "True"));

                    if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to build user and roles");
                    }
                }

                var user3 = await _userMgr.FindByNameAsync("APMI_Admin");
                // Add User
                if (user == null)
                {
                    var company = _context.Companies.Where(c => c.companyId.Equals("APMI")).FirstOrDefault();

                    user = new AppUser()
                    {
                        UserName = "APMI_Admin",
                        firstName = "Admin",
                        lastName = "Terminus",

                        Email = "pvlucban@hotmail.com",
                        company = company
                    };

                    var userResult = await _userMgr.CreateAsync(user, "P@ssw0rd!");
                    var roleResult = await _userMgr.AddToRoleAsync(user, "admin");


                    var claimResult = await _userMgr.AddClaimAsync(user, new Claim("SuperUser", "True"));

                    if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to build user and roles");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

        }
    
    
    }
}