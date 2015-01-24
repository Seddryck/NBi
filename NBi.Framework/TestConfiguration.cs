using NBi.Framework.FailureMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework
{
    public class TestConfiguration: ITestConfiguration
    {

        private readonly IFailureReportProfile failureReportProfile;
        public IFailureReportProfile FailureReportProfile
        {
            get
            {
                return failureReportProfile;
            }
        }

        public TestConfiguration(IFailureReportProfile profile)
        {
            failureReportProfile = profile;
        }

        private static ITestConfiguration @default;
        public static ITestConfiguration Default
        {
            get
            {
                if (@default == null)
                {
                    var profile = NBi.Framework.FailureMessage.FailureReportProfile.Default;
                    @default = new TestConfiguration(profile);
                }
                    
                return @default;
            }
        }

    }
}
