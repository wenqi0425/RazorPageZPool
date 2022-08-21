using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Services.EFServices
{
    public class EFCarService : ICarService
    {
        AppDbContext _context;
        public EFCarService(AppDbContext service)
        {
            _context = service;
        }

        public void AddCar(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public void DeleteCar(Car car)
        {
            // Stored procedures is not tied to DB or object
            // data integrity(is-a relationship, FK cannot be null), reusability
            _context.Cars.FromSqlRaw("spDeleteCarByID {0}", car.CarID)
                .ToList()
                .FirstOrDefault();
            _context.SaveChanges();
        }

        public IEnumerable<Car> GetCars()
        {
            return _context.Cars;
        }

        public Car GetCar(int carId)
        {
            return _context.Cars
                .Include(c => c.AppUser) // Navigation Properties, MyCar   
                .FirstOrDefault(c => c.CarID == carId);
        }

        public void UpdateCar(Car car)
        {
            Car oldCar = _context.Cars.Find(car.CarID);
            oldCar.CarID = car.CarID;
            oldCar.Brand = car.Brand;
            oldCar.Model = car.Model;
            oldCar.NumberOfSeats = car.NumberOfSeats;
            oldCar.NumberPlate = car.NumberPlate;
            oldCar.Color = car.Color;
            _context.SaveChanges();
        }

        public IEnumerable<Car> GetCarsByUser(int userId)
        {
            return _context.Cars.AsNoTracking()
                .Where(c => c.AppUserID == userId);
        }

        public IEnumerable<Car> GetCarsNumberPlate(string numberPlate)
        {
            var cars = _context.Cars.AsNoTracking().Where(c => c.NumberPlate == numberPlate);
            return cars;
        }
    }
}
