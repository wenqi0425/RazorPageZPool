using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ZPool.Models;
using ZPool.Services.Interfaces;

namespace ZPool.Services.EFServices
{
    public class EFReviewService : IReviewService
    {

        private AppDbContext _context;

        public EFReviewService(AppDbContext context)
        {
            _context = context;
        }

        public void CreateReview(Review Review)
        {
            _context.Reviews.Add(Review);
            _context.SaveChanges();
        }

        public List<Review> GetReviewsByUserId(int userId)
        {
            return _context.Reviews.Include(r => r.Reviewer).Include(r => r.Reviewee).Include(r=>r.Ride)
                .Where(r => r.RevieweeId == userId)
                .OrderByDescending(r => r.ReviewDate)
                .ToList();
        }


        public async Task DeleteReviewAsync(int reviewId)
        {
            Review review = _context.Reviews.FirstOrDefault(r=>r.ReviewId==reviewId);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }

        public double GetRatingForUser(int userId)
        {
            List<Review> reviews = GetReviewsByUserId(userId);
            var mean = reviews.Count>0 ? reviews.Select(r => r.RateValue).Average() : 0;
            double rating = Math.Round(mean, 1, MidpointRounding.AwayFromZero);
            return rating;
        }
    }
}
