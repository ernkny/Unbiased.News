using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Domain.DTOs
{
    public class AwsCredentials
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; } 
    }
}
