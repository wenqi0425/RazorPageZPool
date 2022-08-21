using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZPool.Models;

namespace ZPool.Services.Interfaces
{
    public interface ICarService
    {
        IEnumerable<Car> GetCars();
        void AddCar(Car car);
        void DeleteCar(Car car);
        Car GetCar(int carId);
        void UpdateCar(Car car);
        IEnumerable<Car> GetCarsByUser(int userId);
        IEnumerable<Car> GetCarsNumberPlate(string numberPlate);
    }
}
