using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Stateful
{
    public class TestListGenerationResult
    {
        public static TestListGenerationResult Success (int count)
        {
            var res = new TestListGenerationResult();
            res.IsSuccess = true;
            res.Message = string.Format("{0} tests generated successfully.", count);
            return res;
        }

        public static TestListGenerationResult Failure(string message)
        {
            var res = new TestListGenerationResult();
            res.IsSuccess = false;
            res.Message = message;
            return res;
        }

        
        public string Message {  get; private set; }
        public bool IsSuccess {  get; private set; }
    }
}
