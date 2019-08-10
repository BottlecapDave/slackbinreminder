using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlackBinReminder;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SlackBinReminder.Lambda
{
    public class Function
    {
        public const string ENVIRONMENT_VARIABLE_NAME_SLACK_API_TOKEN = "SLACK_API_TOKEN";
        public const string ENVIRONMENT_VARIABLE_NAME_TARGET_SLACK_CHANNEL = "TARGET_SLACK_CHANNEL";

        public const string ENVIRONMENT_VARIABLE_NAME_HOUSE_NUMBER = "HOUSE_NUMBER";
        public const string ENVIRONMENT_VARIABLE_NAME_POSTCODE = "POSTCODE";

        public const string ENVIRONMENT_VARIABLE_NAME_REMINDER_TARGET = "REMINDER_TARGET";

        public void FunctionHandler(ILambdaContext context)
        {
            var reminder = new SlackBinReminder(new Logger(context),
                                                Environment.GetEnvironmentVariable(Function.ENVIRONMENT_VARIABLE_NAME_SLACK_API_TOKEN),
                                                Environment.GetEnvironmentVariable(Function.ENVIRONMENT_VARIABLE_NAME_TARGET_SLACK_CHANNEL));

            var houseNumber = Environment.GetEnvironmentVariable(Function.ENVIRONMENT_VARIABLE_NAME_HOUSE_NUMBER);
            var postcode = Environment.GetEnvironmentVariable(Function.ENVIRONMENT_VARIABLE_NAME_POSTCODE);
            var target = Environment.GetEnvironmentVariable(Function.ENVIRONMENT_VARIABLE_NAME_REMINDER_TARGET);

            switch (target)
            {
                case "today":
                    Task.WaitAll(reminder.RemindTodayAsync(houseNumber, postcode));
                    break;
                case "tomorrow":
                    Task.WaitAll(reminder.RemindTomorrowAsync(houseNumber, postcode));
                    break;
                case "test":
                    Task.WaitAll(reminder.RemindTestAsync(houseNumber, postcode));
                    break;
                default:
                    context.Logger.Log(String.Format("Unexpected target {0}", target));
                    break;
            }
        }
    }
}
