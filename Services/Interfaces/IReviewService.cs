using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZPool.Models;

namespace ZPool.Services.Interfaces
{
    public interface IReviewService
    {
        void CreateReview(Review Review);
        List<Review> GetReviewsByUserId(int userId);
        Task DeleteReviewAsync(int reviewId);
        double GetRatingForUser(int userId);

    }
}
