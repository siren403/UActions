using System.IO;
using System.Net;
using System.Text;
using NUnit.Framework;
using UActions.Editor;
using UnityEngine;
using YamlDotNet.Serialization;

namespace UActions.Tests.Editor
{
    public class WorkflowProviderTests
    {
        [TestCase(
            "https://gist.githubusercontent.com/qkrsogusl3/430a80902687a43d8026568530b32852/raw/5cc87b00b259db61e83ed080dadf93c73b76dd34/workflow.yaml")]
        public void SystemRequestTest(string url)
        {
            var request = WebRequest.CreateHttp(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            var workflow = string.Empty;
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                workflow = reader.ReadToEnd();
            }

            // TODO: runner validate
        }
    }
}