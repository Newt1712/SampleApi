using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Common.Helpers
{
    public static class CacheHelper
    {
        // User
        public static string UserCacheKey = "User-{0}";
        public static string AllUsersCacheKey = "All-Users";


        // Label
        public static string LabelCacheKey = "Label-{0}";
        public static string AllLabelsCacheKey = "All-Labels";

        // Lesson
        public static string LessonCacheKey = "Lesson-{0}";
        public static string AllLessonsCacheKey = "All-Labels";

    }
}
