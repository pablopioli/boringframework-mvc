using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;

namespace Boring
{
    public static class ExtensionMethods
    {
        public static string GetUserDisplayableDescription(this ValidationProblemDetails problem)
        {
            string problemDesc;

            if (problem.Errors != null && problem.Errors.Count > 0)
            {
                problemDesc = problem.Errors.Aggregate(
                                  new StringBuilder(),
                                  (current1, next1) => current1.Append(current1.Length == 0 ? "" : "<br>").Append(
                                     next1.Value.Aggregate(new StringBuilder(),
                                     (current2, next2) => current2.Append(current2.Length == 0 ? "" : "<br>").Append(
                                         next2 + (next2.EndsWith(".") ? "" : ".")))
                                  ))
                    .ToString();
            }
            else
            {
                problemDesc = problem.Title;
            }

            return problemDesc;
        }
    }
}
