using System;
using ProjectsBaseShared.Models;

namespace ProjectsBaseWebApplication.Models
{
    public class ProjectValidator : IValidator<Project>
    {
        public bool Validate(Project entity)
        {
            return entity.ProjectStartDate.IsDateFutureOrToday() && entity.ProjectEndDate.IsDateFutureOrToday() &&
                   entity.ProjectEndDate.IsAfterDate(entity.ProjectStartDate);
        }
    }

    public static class DateTimeExtension
    {
        public static bool IsDateFutureOrToday(this DateTime dateTime)
        {
            return dateTime > DateTime.Now;
        }

        public static bool IsAfterDate(this DateTime dateTime, DateTime other)
        {
            return dateTime >= other;
        }
    }
}