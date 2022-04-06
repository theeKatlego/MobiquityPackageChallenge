using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobiquityPackageChallenge.Infrastructure.Logging
{
    public class LoggingContext
    {
        public Guid CorrelationId { get; } = Guid.NewGuid();
        public string CommandOrQuery { get; private set; }
        public Exception Exception { get; set; }

        public void SetCommandOrQuery<T>(T commandOrQuery)
        {
            if (!string.IsNullOrEmpty(CommandOrQuery)) // Use the first command or query in this request
                return;

            var commandType = commandOrQuery.GetType(); // MobiquityPackageChallenge.Application.Features.Packer.Pack+Command
            CommandOrQuery = commandType.Name
                .Split('.')
                .Last()
                .Replace("Command", "")
                .Replace("Query", "")
                .Replace("Event", "");
        }
    }
}
