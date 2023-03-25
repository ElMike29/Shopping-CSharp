﻿using Shopping.Data.Entities;
using Shopping.Enums;
using Shopping.Helpers;

namespace Shopping.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Juan", "Zuluaga", "zulu@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);
        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "México",
                    States = new List<State>
                    {
                        new State {
                            Name="Chihuahua",
                            Cities = new List<City>
                            {
                                new City { Name = "Ciudad Juárez"},
                                new City { Name = "Chihuahua"},
                                new City { Name = "Samalayuca"},
                                new City { Name = "Villa Ahumada"},
                            }
                        },
                        new State {
                            Name="Zacatecas",
                            Cities = new List<City>
                            {
                                new City { Name = "Fresnillo"},
                                new City { Name = "Zacatecas"},
                                new City { Name = "Sombrerete"},
                                new City { Name = "Rio Grande"},
                                new City { Name = "Guadalupe"},
                            }
                        }
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Florida",
                            Cities = new List<City>() {
                                new City() { Name = "Orlando" },
                                new City() { Name = "Miami" },
                                new City() { Name = "Tampa" },
                                new City() { Name = "Fort Lauderdale" },
                                new City() { Name = "Key West" },
                            }
                        },
                        new State()
                        {
                            Name = "Texas",
                            Cities = new List<City>() {
                                new City() { Name = "Houston" },
                                new City() { Name = "San Antonio" },
                                new City() { Name = "Dallas" },
                                new City() { Name = "Austin" },
                                new City() { Name = "El Paso" },
                            }
                        },
                    }
                });
            }
            await _context.SaveChangesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            if(!_context.Categories.Any())
            {
                _context.Categories.Add(new Entities.Category { Name = "Tecnología" });
                _context.Categories.Add(new Entities.Category { Name = "Ropa" });
                _context.Categories.Add(new Entities.Category { Name = "Calzado" });
                _context.Categories.Add(new Entities.Category { Name = "Belleza" });
                _context.Categories.Add(new Entities.Category { Name = "Nutrición" });
                _context.Categories.Add(new Entities.Category { Name = "Deportes" });
                _context.Categories.Add(new Entities.Category { Name = "Apple" });
                _context.Categories.Add(new Entities.Category { Name = "Mascotas" });
                await _context.SaveChangesAsync();
            }
        }
    }
}